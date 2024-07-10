using DamageNumbersPro;
using SoftwareKingdom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class IncrementalUpgradeManager : MonoBehaviour
{
    const int DEFAULT_EARNING = 1000;
    const int CHEAT_EARNING = 100000;
    const int CENT_DOLLAR_FACTOR = 100;
    //Settings
    
    // Connections
    public IncrementalUpgSystemProperties systemProperties;
    private UserProgressDatabase userProgressDatabase;
    public IncrementalUpgradeUIManager ui;
    public event Action<int> OnUpgradeStateChangedSignal;
    public event Action OnUpgradeRequest;

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
        if (userProgressDatabase == null)
        {
            CreateDatabase();
        }
        ui.OnUpgradeClicked += OnUpgradeClicked;
        ui.OnNextLevelClicked += OnNextLevelClicked;
        ui.OnPreviousLevelClicked += OnPreviousLevelClicked;
    }

    void CreateDatabase()
    {   
        userProgressDatabase = new UserProgressDatabase();
        userProgressDatabase.OnUpgradeStateChanged += OnUpgradeStateChanged;
        userProgressDatabase.OnMoneyChanged += OnMoneyStateChanged;
        userProgressDatabase.OnLevelChanged += OnLevelChanged;     
    }


    void InitState(){
        userProgressDatabase.Load(systemProperties);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetUpgradeButtonsDisplay()
    {
        for(int i=0; i< userProgressDatabase.upgradableStates.Length; i++)
        {
            ui.SetUpgradableDisplay(i,userProgressDatabase.upgradableStates[i]);
        }
    }

    public void OnMoneyEarned(int money)
    {
        userProgressDatabase.OnMoneyGainRequest(money);
    }

    void OnUpgradeClicked(int buttonIndex)
    {
        userProgressDatabase.OnGradePurchaseRequest(buttonIndex);
        OnUpgradeRequest?.Invoke();

        // Send analytics
        string upgName = userProgressDatabase.GetUpgradableName(buttonIndex);
        KingdomAnalytics.CustomEvent(upgName,
            new AnalyticsEntry("level", userProgressDatabase.levelIndex),
            new AnalyticsEntry("grade", userProgressDatabase.GetGrade(buttonIndex)));

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
        OnUpgradeStateChangedSignal?.Invoke(upgradeIndex);
    }

    void OnMoneyStateChanged(int money)
    {
        ui.SetUserMoney(money);
    }

    void OnLevelChanged(int level, bool byUnlock)
    {
        if(byUnlock)
            KingdomAnalytics.LevelCompleted(userProgressDatabase.levelIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public UserProgressDatabase GetDatabase()
    {
        if(userProgressDatabase == null)
        {
            CreateDatabase();
        }
        return userProgressDatabase;
    }
}

