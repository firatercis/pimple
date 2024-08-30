using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using SoftwareKingdom;
using UnityEngine.UI;
using Obi;

public class UIManager : MonoBehaviour
{
    const float DEFAULT_START_DELAY = 0.2f;
    const int CENT_DOLLAR_FACTOR = 100;
    const int NO_ITEM_UNLOCKED = -1;

    private float totalMoney;

    public Action OnLevelStart,
                    OnNextLevel,
                    OnLevelRestart,
                    OnGamePaused,
                    OnGameResumed,
                    OnInGameRestart,
                    OnClinicUpgradePage;
    public Action<bool> OnNextCustomer;
    public Action<bool, bool, bool> OnSettingsChanged;


    [Header("Settings")]
    public bool defaultPauseOperations = true;
    public float inGameSplatTime = 1.0f;
    public float successfulPunchFactor = 1.2f;
    public float successfulPunchDuration = 1.0f;
    [Header("Screens")]
    public GameObject startCanvas;
    public GameObject ingameCanvas;
   // public GameObject pauseMenu;
    public NextCustomerCanvasManager nextCustomerCanvas;
    public PauseMenu pauseMenu;
    ScreensManager screensManager;
    [Header("Start Game")]
    public TextMeshProUGUI totalMoneyText;
    
    [Header("In Game")]
    public TextMeshProUGUI inGameMoneyText;
    public Image inGameSplat;
    public TextMeshProUGUI inGameLength;
    public UserInputButton userInputButton;
    public Image inGameProgressFillImage;
    private IncrementalUpgradeUIManager incrementUpgradeUIManager;
    public TextMeshProUGUI[] patientTexts;
    public TextMeshProUGUI patientIndexTextStartMenu; // TODO: Start Canvas ve In Game Canvas'i ayri ayri scriptleyebiliriz.
    public TextMeshProUGUI bonusPimpleCountText;
    public BonusInGameCanvas bonusInGameCanvas;
    public BonusCustomerEndCanvas bonusCustomerEndCanvas;
    public ClinicUpgradeMenu clinicUpgradeMenu;
    public AnimatedMoneyIndicator animatedMoneyIndicator;
    [Header("Next Customer Canvas")]
    
    public BonusFillerUI bonusFillerUI;
    public ItemUnlockPanel itemUnlockPanel;
    [Header("Other")]

    public MoneyAddingTextAnimation moneyAnim;
    public ButtonManager buttonManager;
    private DamageNumberManager damageNumberManager;

  //  public Clinic clinicManager;
    public int newClinicCost;
    
    // State variables
    float timeScale;
    bool showingWarning;
    int unlockedItemID;
    bool buyToolsReturnToStart;
    Vector3 bonusPimpleCountTextInitialScale;

    private void Awake()
    {
        InitConnections();
    }

    void InitConnections()
    {
        HideScreens();
        damageNumberManager = GetComponent<DamageNumberManager>();
        nextCustomerCanvas.OnNextCustomerRequest += OnNextCustomerButtonPressed;
        bonusCustomerEndCanvas.OnClaimed += OnBonusLevelClaimButtonPressed;
        incrementUpgradeUIManager = GetComponent<IncrementalUpgradeUIManager>();
        screensManager = GetComponent<ScreensManager>();

    }

    void HideScreens()
    {
        startCanvas.SetActive(false);
        ingameCanvas.SetActive(false);
        //pauseMenu.SetActive(false);
        nextCustomerCanvas.gameObject.SetActive(false);
        bonusCustomerEndCanvas.gameObject.SetActive(false);
    }

    void Start()
    {
       // Instantiate(clinicManager.clinics[PlayerPrefs.GetInt("ClinicIndex", 0)]);
        PlayerPrefs.SetInt("ClinicCost", newClinicCost);
        
        Preferences.totalEarnedMoneyInStage = 0;
        Preferences.isFinished = false;

        InitState();   
    }
    
    void InitState()
    {
        timeScale = Time.timeScale;
        pauseMenu.LoadSettingsState();
        unlockedItemID = NO_ITEM_UNLOCKED;
        bonusPimpleCountTextInitialScale = bonusPimpleCountText.transform.localScale;
    }

    #region Handler Functions

    public void StartLevelButton()
    {
        OnLevelStart?.Invoke();
        StartLevel();
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("displayStart", 0);
        OnNextLevel?.Invoke();
       
    }

    public void RestartLevelButton()
    {
       
    }

    public void OnPauseButtonPressed()
    {
        screensManager.ShowScreen("pause");
        OnGamePaused?.Invoke();
        
    }

    public void OnUseToolsButtonPressed()
    {
        screensManager.HideScreen("inGame");
        screensManager.ShowScreen("useTools");
    }

    public void OnBuyToolsButtonPressed(bool returnToStart = true)
    {
        buyToolsReturnToStart = returnToStart;
        screensManager.ShowScreenOnly("buyTools");
    }

    public void OnReturnFromBuyTools()
    {
        if (buyToolsReturnToStart)
        {
            screensManager.ShowScreenOnly("start");
        }
        else
        {
            screensManager.ShowScreenOnly("useTools");
        }

       
    }

    public void OnReturnBackInGamePressed()
    {
        screensManager.HideAllScreens();
        screensManager.ShowScreen("inGame");
    }

    void OnInventoryUse(int itemID)
    {
        screensManager.HideScreen("useTools");
        screensManager.ShowScreen("inGame");
    }

    public void OnResumeButtonPressed()
    {
       
    }

    public void OnInGameRestartPressed()
    {
        if (defaultPauseOperations)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        OnInGameRestart?.Invoke();
    }

    #endregion

    public void StartLevel()
    {
        
        Preferences.isFinished = false;

        Time.timeScale = 1;

        startCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        
        
    }
  

    public void UpdateProgess(float progress)
    {
        inGameProgressFillImage.fillAmount = progress;
        //levelBarDisplay.DisplayProgress(progress);
    }



    void InitStates()
    {
        ingameCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

    public void OpenWarningSign()
    {
        if (!showingWarning)
        {
            //warningSign.SetActive(true);
            //warningDO.DOPlay();
            damageNumberManager.SpawnMessage(isPositive: false);
            showingWarning = true;
            
        }
    }

    public void SetBonusFillerOldValue(float oldVal)
    {

        bonusFillerUI.SetOldValue(oldVal);
    }

    public void SetBonusFillerNewValue(float newVal, float maxValue)
    {
        bonusFillerUI.DisplayValueWithFiller(newVal,maxValue);
    }

    public void CloseWarningSign()
    {
        showingWarning = false;
        //warningDO.DOPause();
        //warningSign.SetActive(false);
    }

    public void PlayMoneyAdd(int money, bool isBig = false)
    {
        //  moneyAnim.EnableAnimation(money / CENT_DOLLAR_FACTOR);
        damageNumberManager.SpawnMoneyNumber((float)money / CENT_DOLLAR_FACTOR, isBig);
    }

    public void SetNBonusPimples(int bonusPimpleCount)
    {
     
        bonusInGameCanvas.SetBonusPimplesHealed(bonusPimpleCount);
    }

    public void ShowInGameSplat()
    {
        inGameSplat.gameObject.SetActive(true);
        Invoke(nameof(HideInGameSplat), inGameSplatTime);
        damageNumberManager.SpawnMessage(isPositive: false,"oh no!");
    }

    public void ShowPimpleSuccess()
    {
        damageNumberManager.SpawnMessage(isPositive: true, "clear!");
    }

    


    public void ShowGirlSuccess(int success, int fail)
    {
        if(fail > 0)
        {
            damageNumberManager.SpawnMessage(isPositive: true, "success : " + (success) + "/" + (success+fail));

        }
        else
        {
            damageNumberManager.SpawnMessage(isPositive: true, "PERFECT!");
        }

    }

    public void HideInGameSplat()
    {
        inGameSplat.gameObject.SetActive(false);
    }

    public void ShowLength(float length, float maxLength)
    {
        inGameLength.gameObject.SetActive(true);
        inGameLength.text = length.ToString("0.0") + "/" + maxLength.ToString("0.0") + " cm";
       // inGameLength.color = Color.Lerp(Color.white, Color.green, length / maxLength);

        inGameProgressFillImage.fillAmount = length / maxLength;
    }
    
    public void SetCustomerIndex(int id)
    {

        for(int i=0; i<patientTexts.Length; i++)
        {
            patientTexts[i].text = "Patient " + id;
        }

        bonusInGameCanvas.SetCustomerNumber(id);
    }

    public void SetClinicUpgradeInfo(int clinicLevel, int upgradeCost, bool canUpgrade)
    {
        clinicUpgradeMenu.SetClinicLevelText(clinicLevel);
        clinicUpgradeMenu.SetUpgradeCost(upgradeCost);
        clinicUpgradeMenu.SetUpgradeButtonEnabled(canUpgrade);
    }

    public void ShowBonusLevelCustomerCanvas(int earnedMoney)
    {
        screensManager.ShowScreenOnly("bonusCustomer");
        bonusCustomerEndCanvas.SetEarnedMoneyText(earnedMoney);

    }


    public void PlayMoneyAnimation(Action animationEndEvent)
    {
        animatedMoneyIndicator.PlayAnimation(animationEndEvent);
    }

    public void SetBonusTimerPercent(float timerPercent)
    {
        bonusInGameCanvas.SetTimerPercent(timerPercent);
    }



    public void PunchProgressBar()
    {
        inGameProgressFillImage.rectTransform.parent.DOPunchScale(Vector3.one * successfulPunchFactor, successfulPunchDuration,vibrato:0);
    }

    public void HideLength()
    {
        inGameLength.gameObject.SetActive(false);
    }

    public void SetUserInputButtonListener(UserInputButtonListener listener)
    {
        userInputButton.listener = listener;
    }

    public void EnableGameStart(bool bonusStart = false)
    {
        if (!bonusStart)
            screensManager.ShowScreen("start");
        else
            screensManager.ShowScreen("bonusStart");

    }

    public void DisplayNextCustomerCanvas(float successPercent, int customerEarning, float totalAcneNewValue, float totalAcneFullValue, int totalMoney)
    {
        screensManager.ShowScreen("nextcustomer");
        screensManager.HideScreen("ingame");
        nextCustomerCanvas.DisplayCanvas(successPercent, customerEarning / CENT_DOLLAR_FACTOR, totalAcneNewValue, totalAcneFullValue,totalMoney / CENT_DOLLAR_FACTOR);
        incrementUpgradeUIManager.SetUpgradeButtonsVisible(false);
    }

    public void DisplayBonusLevelStart()
    {
        screensManager.ShowScreenOnly("bonusStart");
    }



    public void SetItemUnlocked(int id)
    {
        unlockedItemID = id;
    }

    public void HideCustomerScreens()
    {
        screensManager.HideScreen("nextCustomer");
        screensManager.HideScreen("bonusCustomer");
    }

    public void OnNextCustomerButtonPressed(bool claimBonus = false)
    {
      //  screensManager.ShowScreen("start");
        //screensManager.HideScreen("nextCustomer");
        //screensManager.HideScreen("bonusCustomer");

        incrementUpgradeUIManager.SetUpgradeButtonsVisible();

       // itemUnlockPanel.DisplayItemUnlockPanel(unlockedItemID, //TODO: Buradan devam.

        OnNextCustomer?.Invoke(claimBonus);
    }

    public void OnBonusLevelClaimButtonPressed() {
        OnNextCustomerButtonPressed();
    }

    public void OnItemUnlockMenuClaimed()
    {

    }



    public void OnPauseMenuSaved()
    {
        
    }



}
