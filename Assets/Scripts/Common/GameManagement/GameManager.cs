using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoftwareKingdom;
using TheraBytes.BetterUi;
using Cinemachine;
//using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.Assertions;
using DG.Tweening;
using System.Linq.Expressions;
using GameAnalyticsSDK;


public class GameManager : MonoBehaviour
{
    const float CAM_TRANSITION_TIME = 2.0f;

    const int UPG_INDEX_SQUEEZE_POWER = 0;
    const int UPG_INDEX_HEALING_RATE = 1;
    const int UPG_INDEX_EARNING = 2;

    const int CHEAT_MONEY = 100000;
    const int DANGER_SQUEEZE_SOUND_INDEX = 2;
    // Settings
    public bool spawnLevel = true;
    public int tutorialLevels;
    public float earningLengthPeriod = 2.0f;
    public float blackHeadDestroyIncomeMultiplier = 0.5f;
    public float minorIncomeCoefficient = 1; // 1 cent 
    public float perPimpleIncomeCoefficient = 1000; // 10 dollars per pimple
    public float perCustomerIncomeCoefficient = 20000; // 200 dollars per girl
    public int experiencePerCustomer = 10;
    public float pimpleSwitchTime = 1.0f;

    public int minNumberOfPimplesPerGirl;
    public int maxNumberOfPimplesPerGirl;

    public float maxPimpleLength;
    public float minPimpleLength;
    public float pimpleLengthPerLevel;
    public float pimpleLengthPerPimple;
    public float gameStartTime = 2.0f;
    public float minAcneForBonusLevel = 200;
    public float cinemachineBlendTime;
    public bool tapMode;

    public float customerEndPanelDelay = 1.0f;
    public float nextCustomerDelay = 3.0f;
    public float bonusLevelWholeTime = 10.0f;
    public float bonusLevelStartDelay = 2.0f;
    // Connections
    public UserProgressDatabase userProgress;
    public GameObject[] levels;
    public UIManager ui;
   // public UIManagerRev uiRev;
    private CustomerLoader customerLoader;
    public HandManagerRev handManager;
    //public CinemachineVirtualCamera topVirtualCam;
    //public CinemachineVirtualCamera handVirtualCam;
    public CameraFXManager cameraFXManager;
    public Transform girlPlacement;
    public Transform environmentPlacement;
    public Transform bedPoint;
    public Transform layDownPoint;
    public OnboardingList onboardingList;
    private IncrementalUpgradeManager upgradeManager;
    private UserProgressDatabase userProgressDatabase;
    private CinemachineBrain cinemachineBrain;
    private BackgroundMusicManager bgMusicManager;
    public GameObject[] backgroundSettings;
    private ItemUnlockManager itemUnlockManager;
    private BonusLevelUnlockManager bonusLevelUnlockManager;
    private ClinicUpgradeManager clinicUpgradeManager;
    private Inventory inventory;
    // State variables
    int levelIndex;
    int score;
    CustomerManager currentCustomer;

    float previousLength;
    bool firstLay = true;
    int nPimpleSuccess;
    int nBonusPimpleHealed;
    int nPimpleFail;
    int nPimpleHealedTotal;
    int nCustomersTotal;
    float totalBonusAcne;
    int totalXP;
    bool bonusLevelWillBeLoaded = false;
    bool inBonusLevel;
    bool lastPimpleBonus = false;

    float bonusLevelRemainingTime;

    float customerSuccessValue;
    float customerEarnedMoney;

    private void Awake()
    {
       
        InitConnections();
    }

    private void Start()
    {
        InitState();
    }

    void InitState()
    {

        userProgressDatabase = upgradeManager.GetDatabase();

        
        float squeezePowerValue = userProgressDatabase.GetValue(UPG_INDEX_SQUEEZE_POWER);
        handManager.SetSqueezePower(squeezePowerValue);
        handManager.tapMode = tapMode;

        nPimpleHealedTotal = PlayerPrefs.GetInt("nPimpleHealedTotal", 0);
        nCustomersTotal = PlayerPrefs.GetInt("nCustomersTotal", 0); // TODO: Which may be in initState
        totalBonusAcne = PlayerPrefs.GetFloat("totalBonusAcne", 0);
        totalXP = PlayerPrefs.GetInt("totalXP", 0);
        ui.SetBonusFillerOldValue(totalBonusAcne);
        ui.SetCustomerIndex(nCustomersTotal + 1);
        bgMusicManager.InGameMusic();

       // SetBonusLevelModeOn();

        LoadCustomer();

        RefreshClinicUpgradeUIState();

        //GoToPimple(currentGirl.currentPimple.transform);
    }

    private void RefreshClinicUpgradeUIState()
    {
        clinicUpgradeManager.LoadState();
        bool canUpgradeClinic = userProgressDatabase.totalMoney >= clinicUpgradeManager.GetUpgradeCost();
        ui.SetClinicUpgradeInfo(clinicUpgradeManager.clinicLevel,
            clinicUpgradeManager.GetUpgradeCost(),
            canUpgradeClinic);
    }


    void InitConnections()
    {
      
        DOTween.KillAll();

        levelIndex = PlayerPrefs.GetInt("Level", 0);

        InitUIConnections();

        customerLoader = GetComponent<CustomerLoader>();

        // Initialize the analytics
        handManager.OnRopeLanded += OnRopeLandedHand;

        KingdomAnalytics.Init();
        upgradeManager = GetComponent<IncrementalUpgradeManager>();

        upgradeManager.OnUpgradeStateChangedSignal += OnUpgradeStateChanged;
        upgradeManager.OnUpgradeRequest += OnUpgradeRequest;

        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachineBlendTime = cinemachineBrain.m_DefaultBlend.BlendTime;

        bgMusicManager = GetComponent<BackgroundMusicManager>();


        inventory = GetComponent<Inventory>();
        inventory.OnItemUsed += OnItemUsed;

        itemUnlockManager = GetComponent<ItemUnlockManager>();
        bonusLevelUnlockManager = GetComponent<BonusLevelUnlockManager>();
        clinicUpgradeManager = GetComponent<ClinicUpgradeManager>();
        Debug.Log("Cinemachine blend time: " + cinemachineBlendTime);

        //LoadGirl();

    }


    private void InitUIConnections()
    {
        ui.OnLevelStart += OnLevelStart;
        ui.OnNextLevel += OnNextLevel;
        ui.OnLevelRestart += OnLevelRestart;
        ui.OnNextCustomer += OnNextCustomer;
        ui.OnGamePaused += OnGamePaused;
        ui.SetUserInputButtonListener(handManager);
    }

    void LoadCustomer()
    {
        customerLoader = GetComponent<CustomerLoader>();

        if (currentCustomer != null)
        {
            currentCustomer.ClearEvents();
            Destroy(currentCustomer.gameObject);
        }

        bool manUnlocked = itemUnlockManager.IsUnlocked("maleaudience");

        bool blackHeadUnlocked = itemUnlockManager.IsUnlocked("blackhead");

        currentCustomer = customerLoader.LoadCustomer(girlPlacement, minNumberOfPimplesPerGirl, maxNumberOfPimplesPerGirl, isBonusLevel: bonusLevelWillBeLoaded, manUnlocked: manUnlocked, blackHeadUnlocked: blackHeadUnlocked); // TODO: fazla dolambacli yol
        currentCustomer.PlaceToBed(bedPoint,layDownPoint.position);
        currentCustomer.OnLayDownPlacementEnd += OnGirlLayDownEnded;
        currentCustomer.OnNextPimpleReady += OnNextPimpleReady;
        nPimpleFail = 0;
        nPimpleSuccess = 0;
        nBonusPimpleHealed = 0;
        inventory.UpdateUI();
    }

    //void EnableGameStart()
    //{
    //    ui.EnableGameStart();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            upgradeManager.OnMoneyEarned(CHEAT_MONEY);
            ui.PlayMoneyAdd(CHEAT_MONEY); // 10 cm'de bir para ekleme kismi
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //bonusLevelWillBeLoaded = true;
            SetBonusLevelModeOn();
            handManager.SetBonusMode(true);
        }

        if (bonusLevelWillBeLoaded && inBonusLevel)
        {
            CheckBonusLevelTimer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            itemUnlockManager.EarnExperience(experiencePerCustomer);
        }

    }


    #region UI Handlers
    public void OnLevelStart()
    {
        Debug.Log("LEVEL STARTED");

       //  KingdomAnalytics.LevelStarted(userProgressDatabase.levelIndex);
      //  SupersonicWisdom.Api.NotifyLevelStarted(nCustomersTotal,null);

        currentCustomer.RequestPimple();

        //GoToCurrentPimple();
        // bgMusicManager.InGameMusic();
        if (bonusLevelWillBeLoaded)
            OnBonusLevelStart();
    }

    public void OnBonusLevelStart()
    {
        bonusLevelRemainingTime = bonusLevelWholeTime;
        
        ui.SetNBonusPimples(0);
        Invoke(nameof(StartBonusLevelTimer), bonusLevelStartDelay);
        currentCustomer.GetBonusLevelCam().gameObject.SetActive(true);
        bonusLevelUnlockManager.OnBonusLevelOpened();
        // TODO: ui. show tutorial
    }
    
    public void OnClinicUpgradeRequest()
    {
        bool couldUpgrade = clinicUpgradeManager.TryUpgrade();
        if (couldUpgrade)
        {
            bool canUpgradeClinic = userProgressDatabase.totalMoney >= clinicUpgradeManager.GetUpgradeCost();
            ui.SetClinicUpgradeInfo(clinicUpgradeManager.clinicLevel,
                clinicUpgradeManager.GetUpgradeCost(),
                canUpgradeClinic); // TODO: Tekrar eden bir kod
        }
    }

    void StartBonusLevelTimer()
    {
        inBonusLevel = true;
    }

    private void SetBonusLevelModeOn()
    {
        bonusLevelRemainingTime = bonusLevelWholeTime;
        bonusLevelWillBeLoaded = true; // TODO: FOR ONLY TEST

    }

    void CheckBonusLevelTimer()
    {
        bonusLevelRemainingTime -= Time.deltaTime;
        ui.SetBonusTimerPercent(bonusLevelRemainingTime / bonusLevelWholeTime);
        if(bonusLevelRemainingTime <= 0)
        {
            bonusLevelRemainingTime = 0;
            OnBonusLevelTimeIsUp();
        }
    }

    void OnBonusLevelTimeIsUp()
    {

        currentCustomer.HealAllBonusPimples();


        customerEarnedMoney = userProgressDatabase.GetValue(UPG_INDEX_EARNING)
           * perPimpleIncomeCoefficient
           * nBonusPimpleHealed
           * userProgressDatabase.GetLevelIncomeCofficient();

   
        float bonusLevelSuccess = nBonusPimpleHealed == 0 ? 0.0f : 1.0f;
        //upgradeManager.OnMoneyEarned((int)customerEarnedMoney);
        currentCustomer.GetBonusLevelCam().gameObject.SetActive(false);
        currentCustomer.OnOperationsEnd(bonusLevelSuccess);
        ui.ShowBonusLevelCustomerCanvas((int)customerEarnedMoney);
        inBonusLevel = false;
        bonusLevelWillBeLoaded = false;
    }

    void OnLevelRestart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnNextLevel()
    {
        PlayerPrefs.SetInt("Level", levelIndex + 1);
        KingdomAnalytics.LevelCompleted(userProgressDatabase.levelIndex);
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //PlayerPrefs.SetInt("showStart", 0);
    }
    void OnUpgradeRequest()
    {
        handManager.DisplayUpgradeFX();
    }

    void OnGamePaused()
    {
        // TODO: Oyunu durdurma islemleri.
    }

    #endregion

    #region Pimple Handler Functions
    void OnPimplePain(float painPercentage)
    {

    }

    void OnPimpleReleased()
    {

    }

    void OnPimpleIsInDanger()
    {
        handManager.OnDanger();
        ui.OpenWarningSign();
       // soundFXManager.StartSqueeze(DANGER_SQUEEZE_SOUND_INDEX);
    }

    void OnPimpleDangerClear()
    {
        ui.CloseWarningSign();
    }

    void OnPimpleBlackheadHealed()
    {
        float earnedMoney = userProgressDatabase.GetValue(UPG_INDEX_EARNING)
                      * perPimpleIncomeCoefficient
                      * blackHeadDestroyIncomeMultiplier
                      * userProgressDatabase.GetLevelIncomeCofficient();
        upgradeManager.OnMoneyEarned((int)earnedMoney);

        ui.PlayMoneyAdd((int)earnedMoney); 
    }

    void OnPimpleKilled()
    {
        nPimpleFail++;
        ui.ShowInGameSplat();
        ui.HideLength();
        NextPimpleWithDelay();
        //currentLevelManager.GoNextPimple();
    }

    void OnPimpleHealedSuccesfully()
    {
        float earnedMoney = userProgressDatabase.GetValue(UPG_INDEX_EARNING) 
            * perPimpleIncomeCoefficient 
            * userProgressDatabase.GetLevelIncomeCofficient();
        upgradeManager.OnMoneyEarned((int)earnedMoney);
       
        ui.PlayMoneyAdd((int)earnedMoney, isBig:true); // 10 cm'de bir para ekleme kismi
         
        ui.ShowPimpleSuccess();
        ui.HideLength();
        ui.PunchProgressBar();
        nPimpleSuccess++;
        nPimpleHealedTotal++;
        PlayerPrefs.SetInt("nPimpleHealedTotal", nPimpleHealedTotal);
    }

    void OnPimpleBonusHeal()
    { 
       
        //ui.ShowPimpleSuccess();
        nBonusPimpleHealed++;
        ui.SetNBonusPimples(nBonusPimpleHealed);
    }

    void OnRopeLandedHand()
    {
        NextPimpleWithDelay();
    }

    
    void OnPimpleRopeLength(float length,float maxLength)
    {
        
        if (length - previousLength >= earningLengthPeriod || tapMode)
        {
            float lengthDifference = length - previousLength;
            UpdateBonusAcne(lengthDifference);

            previousLength = length;

            float earnedMoney = userProgressDatabase.GetValue(UPG_INDEX_EARNING) 
                * minorIncomeCoefficient
                * userProgressDatabase.GetLevelIncomeCofficient();
            upgradeManager.OnMoneyEarned((int)earnedMoney);

            ui.PlayMoneyAdd((int)earnedMoney); // 10 cm'de bir para ekleme kismi
        }
        ui.ShowLength(length, maxLength);
    }

    void UpdateBonusAcne(float lengthDifference)
    {
        totalBonusAcne += lengthDifference;
        PlayerPrefs.SetFloat("totalBonusAcne", totalBonusAcne);
    }

    void OnItemUsed(int itemID)
    {
        handManager.UseTool(itemID);
        // Maybe some analytic
        
    }


    #endregion

    #region Gameplay Functions

    void GoToCurrentPimple(float delay = 0)
    {

        if (customerLoader.testingAllPimples) // for only test
        {
            cameraFXManager.focusObject = currentCustomer.currentPimple.transform;
        }

        //cameraFXManager.GoToCam(VirtualCamTarget.Hand, delay);
        if (!bonusLevelWillBeLoaded)
        {
            GoToPimple(currentCustomer.currentPimple.transform);
            currentCustomer.currentPimple.SetVirtualCamActive();
            ConnectToPimple(currentCustomer.currentPimple);
        }
        else
        {
            ConnectToBonusPimples(currentCustomer.pimples);
        }
    }

    void ConnectToPimple(AcneManagerRev pimple)
    {
        pimple.OnReleased += OnPimpleReleased;
        pimple.OnKilled += OnPimpleKilled;
        pimple.OnPain += OnPimplePain;
        pimple.OnHealedSuccessfully += OnPimpleHealedSuccesfully;
        pimple.OnRopeLength += OnPimpleRopeLength;
        pimple.OnIsInDanger += OnPimpleIsInDanger;
        pimple.OnDangerClear += OnPimpleDangerClear;
        pimple.OnBlackheadHealed += OnPimpleBlackheadHealed;
        pimple.OnBonusHeal += OnPimpleBonusHeal;
       // pimple.tapMode = tapMode;
        float healingRate = userProgressDatabase.GetValue(UPG_INDEX_HEALING_RATE);
        pimple.SetHealingRate(healingRate);

        float maxAcneLengthForPimple = Random.Range(minPimpleLength, maxPimpleLength) + (userProgressDatabase.levelIndex * pimpleLengthPerLevel) + (pimpleLengthPerPimple * nPimpleHealedTotal);
        pimple.SetMaxAcneLength(maxAcneLengthForPimple);

        ui.ShowLength(0, maxAcneLengthForPimple);
    }

    void ConnectToBonusPimples(List<AcneManagerRev> bonusPimples)
    {
 
        for(int i= 0; i < bonusPimples.Count; i++)
        {
            bonusPimples[i].OnBonusHeal += OnPimpleBonusHeal;
        }
    }


    private void NextPimpleWithDelay()
    {
       
        handManager.ExitPimple();

        // GoToTopCam();
        //handVirtualCam.gameObject.SetActive(false);
        SwitchToNextPimple();
        //Invoke(nameof(OpenHandCam), cinemachineBlendTime);
    }

    void GoToTopCam()
    {
        //topVirtualCam.gameObject.SetActive(true);
        //handVirtualCam.gameObject.SetActive(false);
        Invoke(nameof(OnTopCamLive), cinemachineBlendTime);
    }

    public void OnTopCamLive()
    {
     
        SwitchToNextPimple();
    }

    void SwitchToNextPimple()
    {
        currentCustomer.currentPimple.SetVirtualCamInactive();
        bool thereArePimplesRemaining = currentCustomer.NextPimple();

        if (!thereArePimplesRemaining)
        {
            OnCustomerEnded();
        }
    }

    void OnNextPimpleReady()
    {
        ui.HideLength();

        //topVirtualCam.gameObject.SetActive(false);
        //handVirtualCam.gameObject.SetActive(true);
        // go to the next pimple
        GoToCurrentPimple(cinemachineBlendTime);
    }

    void GoToPimple(Transform pimplePoint)
    {

        PimpleWayPoints pimpleWayPoints = pimplePoint.GetComponent<PimpleWayPoints>();
        handManager.GoToPimple(pimpleWayPoints, cinemachineBlendTime);

        //topVirtualCam.gameObject.SetActive(false);
        //handVirtualCam.gameObject.SetActive(true);
        previousLength = 0; // Reset pimple acne length
        Invoke(nameof(UnlockHandInput), CAM_TRANSITION_TIME); // TODO: Cam transition time can be linked somehow

    }
     
    void OnCustomerEnded() 
    {
        cameraFXManager.GoToEndGameCam();
        ui.SetBonusFillerOldValue(bonusLevelUnlockManager.currentBonusXP);
        bonusLevelWillBeLoaded = bonusLevelUnlockManager.EarnBonusXP(experiencePerCustomer); // TODO: Ayni experience variable used.
        Invoke(nameof(DisplayCustomerEndPanel), customerEndPanelDelay);

        // On Girl Cleared

        customerSuccessValue = (float)nPimpleSuccess / (float)(nPimpleFail + nPimpleSuccess);

        customerEarnedMoney = userProgressDatabase.GetValue(UPG_INDEX_EARNING)
          * perCustomerIncomeCoefficient
          * userProgressDatabase.GetLevelIncomeCofficient()
          * customerSuccessValue;
        //upgradeManager.OnMoneyEarned((int)customerEarnedMoney);
        currentCustomer.OnOperationsEnd(customerSuccessValue);
        totalXP += Mathf.FloorToInt(customerSuccessValue * 100);
        PlayerPrefs.SetInt("totalXP", totalXP);
        //KingdomAnalytics.LevelCompleted(nCustomersTotal);
        //SupersonicWisdom.Api.NotifyLevelCompleted(nCustomersTotal,null);
    }

    void DisplayCustomerEndPanel()
    {
        ui.DisplayNextCustomerCanvas(customerSuccessValue * 100, (int)customerEarnedMoney, 50, 100, userProgressDatabase.totalMoney); // TODO: irin biriktirme
        ui.SetBonusFillerNewValue(bonusLevelUnlockManager.currentBonusXP, bonusLevelUnlockManager.requiredBonusXPForNextLevel);
        // currentGirl = currentLevelManager.RandomizeGirl(girlPlacement, minNumberOfPimplesPerGirl, maxNumberOfPimplesPerGirl);
        //cameraFXManager.GoToTopCam();
       // cameraFXManager.GoToEndGameCam();
    }


    void OnNextCustomer()
    {
       

        currentCustomer.StandUp(bedPoint.position);
        nCustomersTotal++;
        PlayerPrefs.SetInt("nCustomersTotal", nCustomersTotal);
        //KingdomAnalytics.LevelStarted(nCustomersTotal);
        //SupersonicWisdom.Api.NotifyLevelStarted(nCustomersTotal,null);
        itemUnlockManager.EarnExperience(experiencePerCustomer);
        
        inventory.UpdateStates();
        if (customerEarnedMoney > 0)
            ui.PlayMoneyAnimation(animationEndEvent: OnClaimAnimationEnd);
        else
        {
            OnClaimAnimationEnd();
        }
        // bgMusicManager.InGameMusic();
    }

    void OnClaimAnimationEnd()
    {
        upgradeManager.OnMoneyEarned((int)customerEarnedMoney);
        Invoke(nameof(LoadNextCustomer), nextCustomerDelay);
        itemUnlockManager.UpdateUI();
    }

    void LoadNextCustomer()
    {
        
        ui.HideCustomerScreens();
        cameraFXManager.GoToTopCam();
        LoadCustomer();
        ui.SetCustomerIndex(nCustomersTotal);
    }



    void OnGirlLayDownEnded()
    {
        ui.EnableGameStart(bonusStart: bonusLevelWillBeLoaded);
        
    }
    void UnlockHandInput()
    {
        handManager.SetSqueezingEnabled(true);
    }

    void IncreaseXP(int xp)
    {
        totalXP += xp;
        PlayerPrefs.SetInt("totalXP", totalXP);
    }

   

    #endregion

    #region Database Handler Functions
    void OnUpgradeStateChanged(int upgradeIndex)
    {
        if (upgradeManager == null) Debug.Log("upgradeManager == null");

        if (userProgressDatabase == null) userProgressDatabase = upgradeManager.GetDatabase();

        // Test debug to understand the problem
        if (userProgressDatabase == null) Debug.Log("userProgressDatabase == null");
        if (handManager == null) Debug.Log("handManager == null");
        if (currentCustomer == null)
        {
            Debug.Log("currentGirl == null"); // TODO: Burasi incelenecek.
            return;
        }
        try{

            if (currentCustomer.currentPimple == null) Debug.Log("currentGirl.currentPimple == null");
        }catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        if (upgradeIndex == UPG_INDEX_SQUEEZE_POWER)
        {
            float squeezePowerValue = userProgressDatabase.GetValue(UPG_INDEX_SQUEEZE_POWER);
            handManager.SetSqueezePower(squeezePowerValue);

        }
        else if (upgradeIndex == UPG_INDEX_HEALING_RATE)
        {
            float healingRate = userProgressDatabase.GetValue(UPG_INDEX_HEALING_RATE);
            currentCustomer.currentPimple.SetHealingRate(healingRate);
        }
    }
    #endregion

    #region Cinemachine Handler Functions
    public void OnCameraCut()
    {
        Debug.Log("Camera Cut");
    }

    public void OnCameraLive()
    {
        Debug.Log("Camera Live");
    }

    public void OnCamActivated(ICinemachineCamera cam1, ICinemachineCamera cam2)
    {
      
        Debug.Log("OnCamActivated");
    }

    #endregion



}