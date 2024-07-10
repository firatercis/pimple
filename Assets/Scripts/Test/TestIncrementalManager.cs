using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class TestIncrementalManager: MonoBehaviour
{
    const int DEFAULT_EARNING = 1000;
    const int CHEAT_EARNING = 100000;
    const int CENT_DOLLAR_FACTOR = 100;
    //Settings
    public float moneyEarnPeriod = 5;
    public int moneyPerPeriod = 100;
    public float testCubeRotateRate = 90;
    // Connections
    public IncrementalUpgSystemProperties systemProperties;
    private UserProgressDatabase userProgressDatabase;
    public DamageNumber earningFXPrefab;
    public IncrementalUpgradeUIManager ui;
    public Transform[] testCubes;
    public int[] earningsPerLevel;
    // State Variables 
    float moneyEarnPeriodCounter;
    Transform currentTestCube;
    // Start is called before the first frame update

    private void Awake()
    {
        InitConnections();
    }

    void Start()
    {

        InitState();
    }
    void InitConnections(){
        userProgressDatabase = new UserProgressDatabase();
        ui.OnUpgradeClicked += OnUpgradeClicked;
        ui.OnNextLevelClicked += OnNextLevelClicked;
        ui.OnPreviousLevelClicked += OnPreviousLevelClicked;
        userProgressDatabase.OnUpgradeStateChanged += OnUpgradeStateChanged;
        userProgressDatabase.OnMoneyChanged += OnMoneyStateChanged;
        userProgressDatabase.OnLevelChanged += OnLevelChanged;

    }
    void InitState(){
        userProgressDatabase.Load(systemProperties);
        currentTestCube = testCubes[userProgressDatabase.levelIndex];
        currentTestCube.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEarnMoneyTestButton();
    }

    private void CheckEarnMoneyTestButton()
    {
        if (Input.GetKey(KeyCode.M))
        {
            CheckMoneyEarnPeriod();
            currentTestCube.Rotate(Vector3.right * testCubeRotateRate * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            OnMoneyEarned(CHEAT_EARNING);
        }
    }

    void CheckMoneyEarnPeriod()
    {
        moneyEarnPeriodCounter += Time.deltaTime;
        if(moneyEarnPeriodCounter >= moneyEarnPeriod)
        {
            moneyEarnPeriodCounter = 0;
            int earning = DEFAULT_EARNING * (userProgressDatabase.upgradableStates[2].grade + 1) * systemProperties.levelProperties[userProgressDatabase.levelIndex].incomeMultiplier;
            DamageNumber damageNumber = earningFXPrefab.Spawn(currentTestCube.position, earning / CENT_DOLLAR_FACTOR);

            OnMoneyEarned(earning);
        }
    }

    void SetUpgradeButtonsDisplay()
    {
        for(int i=0; i< userProgressDatabase.upgradableStates.Length; i++)
        {
            ui.SetUpgradableDisplay(i,userProgressDatabase.upgradableStates[i]);
        }
    }

    void OnMoneyEarned(int money)
    {
        userProgressDatabase.OnMoneyGainRequest(money);
    }

    void OnUpgradeClicked(int buttonIndex)
    {
        userProgressDatabase.OnGradePurchaseRequest(buttonIndex);
    }

    void OnNextLevelClicked()
    {
        userProgressDatabase.OnNextLevelRequest();
    }

    void OnPreviousLevelClicked()
    {
        userProgressDatabase.OnPreviousLevelRequest();
    }

    void OnUpgradeStateChanged(int upgradeIndex)
    {
        ui.SetUpgradableDisplay(upgradeIndex,userProgressDatabase.upgradableStates[upgradeIndex]);
        ui.SetLevelSwitchDisplay(userProgressDatabase.levelSwitchState);
    }

    void OnMoneyStateChanged(int money)
    {
        ui.SetUserMoney(money);
    }

    void OnLevelChanged(int level, bool byUnlock)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

