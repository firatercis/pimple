using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.Networking.UnityWebRequest;


public class UpgradableState
{
    public string upgradableName;
    public PurchaseRequestResult isPurchasable;
    public int grade;
    public int price;
}

public class LevelSwitchState
{
    public PurchaseRequestResult nextLevelSwitchable;
    public bool previousLevelAvailable;
    public int nextLevelPrice;
}

public enum PurchaseRequestResult
{
    OK,
    CannotAfford,
    IsMaxed,
    DoesNotNeedToPurchase,
}

public class UserProgressDatabase
{
    // Constants
    const int DEFAULT_UPG_GRADE = 1;
    const int PRICELESS = -1;

    public int levelIndex;
    public int lastLevelUnlocked;

    // Connections
    public event Action<int> OnUpgradeStateChanged;
    public event Action OnLevelSwitchStateChanged;
    public event Action<int> OnMoneyChanged;
    public event Action<int,bool> OnLevelChanged;
    // State variables
    IncrementalUpgSystemProperties systemProperties;
    LevelProperties currentLevelProperties;
    public int totalMoney;
    public UpgradableState[] upgradableStates;
    public LevelSwitchState levelSwitchState;

    string totalMoneyPrefsKey;


    private void Start()
    {
        if (systemProperties != null)
        {
            Load(systemProperties);
        }
    }

    #region Public Functions

    public void Load(IncrementalUpgSystemProperties systemProperties)
    {
        this.systemProperties = systemProperties;
        levelIndex = PlayerPrefs.GetInt("levelIndex", 0);
        lastLevelUnlocked = PlayerPrefs.GetInt("lastLevelUnlocked");

        currentLevelProperties = systemProperties.levelProperties[levelIndex];

        totalMoneyPrefsKey = "totalMoney";
        if (!systemProperties.sameMoneyAcrossLevels)
        {
            totalMoneyPrefsKey = "totalMoneyL" + levelIndex;
        }


        //totalMoneyPrefsKey = systemProperties.sameMoneyAcrossLevels 
        //                            ? "totalMoney" 
        //                            : "totalMoneyL" + levelIndex;
        

        totalMoney = PlayerPrefs.GetInt(totalMoneyPrefsKey, currentLevelProperties.initialMoney);
        int nUpgradables = currentLevelProperties.upgradableProperties.Length;
        upgradableStates = new UpgradableState[nUpgradables];

        for (int i = 0; i < nUpgradables; i++)
        {
            UpgradableProperties currentUpgProperties = currentLevelProperties.upgradableProperties[i];
            upgradableStates[i] = LoadUpgradableState(currentUpgProperties, levelIndex);
        }

        levelSwitchState = new LevelSwitchState();
        UpdateStates();
        // TODO: Save Level change
        SendEvents();
    }

    public void OnMoneyGainRequest(int gainedMoney)
    {
        totalMoney += gainedMoney;
        UpdateStates();
        SaveMoneyChange(totalMoney,levelIndex);
        SendEvents();
    }

    public void OnMoneySpendRequest(int spentMoney)
    {
        totalMoney -= spentMoney;
        UpdateStates();
        SaveMoneyChange(totalMoney, levelIndex);
        SendEvents();
    }

    public bool OnGradePurchaseRequest(int upgradeIndex)
    {
        bool result = true;

        int grade = upgradableStates[upgradeIndex].grade + 1;

        UpgradableProperties currentUpgProperty = currentLevelProperties.upgradableProperties[upgradeIndex];
        if (upgradableStates[upgradeIndex].isPurchasable == PurchaseRequestResult.OK) // If can buy
        {
            totalMoney -= upgradableStates[upgradeIndex].price;
            upgradableStates[upgradeIndex].grade++;
            PurchaseRequestResult newPurchaseState = CanUpgrade(currentUpgProperty, upgradableStates[upgradeIndex].grade + 1);
            UpdateStates();
            SaveGradeChange(currentUpgProperty.upgradableName, grade, levelIndex);
            SaveMoneyChange(totalMoney, levelIndex);
            SendEvents();
        }
        else
        {
            result = false;
        }
        return result;
    }

    public bool OnNextLevelRequest()
    {
        return OnLevelSwitchRequest(levelIndex + 1);
    }

    public bool OnPreviousLevelRequest()
    {
        return OnLevelSwitchRequest(levelIndex - 1);
    }

    public bool OnLevelSwitchRequest(int levelToBeSwitchedTo)
    {
        bool result = true;

        PurchaseRequestResult purchaseRequestResult = CanSwitchToLevel(levelToBeSwitchedTo);

        if (purchaseRequestResult == PurchaseRequestResult.OK)
        {
            totalMoney -= systemProperties.levelProperties[levelToBeSwitchedTo].unlockCost;
            OnMoneyChanged?.Invoke(totalMoney);
            // Level 
            SwitchToLevel(levelToBeSwitchedTo,byUnlock:true);
           
        }
        else if(purchaseRequestResult == PurchaseRequestResult.DoesNotNeedToPurchase)
        {
            SwitchToLevel(levelToBeSwitchedTo, byUnlock:false);
        }
        else
        {
            result = false;
        }

        return result;
    }
   
    public float GetValue(int upgradableIndex)
    {
        int gradeIndex = upgradableStates[upgradableIndex].grade;
        float value = (float)(gradeIndex + 1); // If values array is not defined, return grade
        UpgradableProperties props = currentLevelProperties.upgradableProperties[upgradableIndex];
        float[] values = props.values;

        if (values.Length > 0)
        {
            if(gradeIndex < values.Length)
            {
                value = values[gradeIndex];
            }
            else
            {
                value = GetProceduralValue(props, gradeIndex);
            }

        }
        return value;
    }

    public int GetGrade(int upgradableIndex)
    {
       return upgradableStates[upgradableIndex].grade;
    }

    public string GetUpgradableName(int upgradableIndex) {
        return currentLevelProperties.upgradableProperties[upgradableIndex].upgradableName;
    }

    public float GetLevelIncomeCofficient()
    {
        return currentLevelProperties.incomeMultiplier;
    }

    #endregion
    private void SwitchToLevel(int levelToBeSwitchedTo, bool byUnlock=false)
    {
        levelIndex = levelToBeSwitchedTo;
        SaveLevelChange(levelIndex);
        Load(systemProperties); // Load new level properties
        OnLevelChanged?.Invoke(levelToBeSwitchedTo, byUnlock);
        ClearEventConnections(); // TODO: Siralama kontrol et.
    }

    #region Purchase Checks
    private PurchaseRequestResult CanUpgrade(UpgradableProperties props, int gradeIndex)
    {
        PurchaseRequestResult result = PurchaseRequestResult.OK;
        if (gradeIndex >= props.costs.Length)
        {
            if(props.ugradableType == UpgradableType.Finite)
            {
                result = PurchaseRequestResult.IsMaxed;
            }
            else
            {
                int cost = GetProceduralPrice(props, gradeIndex);
                if(totalMoney  < cost)
                {
                    result = PurchaseRequestResult.CannotAfford;
                }
            }
        }
        else if (totalMoney < props.costs[gradeIndex])
        {
            result = PurchaseRequestResult.CannotAfford;
        }
        return result;
    }

    private PurchaseRequestResult CanSwitchToLevel(int levelToBeSwitched)
    {
        PurchaseRequestResult result = PurchaseRequestResult.OK;

        if (levelToBeSwitched >= 0 &&  levelToBeSwitched <= lastLevelUnlocked)
        {
            result = PurchaseRequestResult.DoesNotNeedToPurchase;
        }
        else if (levelToBeSwitched >= systemProperties.levelProperties.Length || levelToBeSwitched < 0)
        {
            result = PurchaseRequestResult.IsMaxed;
        }
        else if (totalMoney < systemProperties.levelProperties[levelToBeSwitched].unlockCost)
        {
            result = PurchaseRequestResult.CannotAfford;
        }


        return result;
    }

    #endregion

    #region UpdateState functions
    private void UpdateStates()
    {
        for (int i = 0; i < currentLevelProperties.upgradableProperties.Length; i++)
        {
            UpgradableProperties currentUpgProperty = currentLevelProperties.upgradableProperties[i];
            PurchaseRequestResult newPurchaseState = CanUpgrade(currentUpgProperty, upgradableStates[i].grade + 1);
            RefreshUpgradableVariables(currentUpgProperty, upgradableStates[i], newPurchaseState);
        }
        // Check level switch states
        RefreshLevelSwitchStates();


    }

    public void RefreshLevelSwitchStates()
    {
        PurchaseRequestResult levelSwitchAvailable = CanSwitchToLevel(levelIndex + 1);
        levelSwitchState.nextLevelSwitchable = levelSwitchAvailable;

        if (levelSwitchAvailable == PurchaseRequestResult.IsMaxed || levelSwitchAvailable == PurchaseRequestResult.DoesNotNeedToPurchase)
        {
            levelSwitchState.nextLevelPrice = PRICELESS;
        }
        else
        {
            levelSwitchState.nextLevelPrice = systemProperties.levelProperties[levelIndex + 1].unlockCost;
        }

        levelSwitchState.previousLevelAvailable =
            CanSwitchToLevel(levelIndex - 1) == PurchaseRequestResult.DoesNotNeedToPurchase;
    }
    private void RefreshUpgradableVariables(UpgradableProperties props, UpgradableState upgradableState, PurchaseRequestResult canBeUpgraded)
    {

        upgradableState.isPurchasable = canBeUpgraded;
        
        // Set the price of the state
        if(upgradableState.grade + 1 >= props.costs.Length  && canBeUpgraded !=PurchaseRequestResult.IsMaxed) // TODO: Buraya daha iyi bir kod
        {

            upgradableState.price = GetProceduralPrice(props, upgradableState.grade + 1);

        } else if (canBeUpgraded == PurchaseRequestResult.OK) // TODO: Bu kod daha da düzenlenebilir.
        {
            upgradableState.price = props.costs[upgradableState.grade + 1];
        }
        else if (canBeUpgraded == PurchaseRequestResult.IsMaxed)
        {
            upgradableState.price = PRICELESS;
        }
        else if (canBeUpgraded == PurchaseRequestResult.CannotAfford)
        {
            upgradableState.price = props.costs[upgradableState.grade + 1];

        }
    }

    private int GetProceduralPrice(UpgradableProperties props, int gradeIndex)
    {
        int lastPrice = props.costs[props.costs.Length - 1];
        int gradeDifference = gradeIndex - props.costs.Length + 1;
        int price = lastPrice;
        if (props.ugradableType == UpgradableType.AdditiveInfinite)
        {
            price += (int) ((float) gradeDifference * props.proceduralCostCoefficient);  
        }else if(props.ugradableType == UpgradableType.MultiplicativeInfinite)
        {
            price *= (int) Mathf.Pow(props.proceduralCostCoefficient, gradeDifference);
        }

        return price;
    }

    private float GetProceduralValue(UpgradableProperties props, int gradeIndex)
    {
        float lastValue = props.values[props.values.Length - 1];
        int gradeDifference = gradeIndex - props.values.Length + 1;
        float value = lastValue;
        if (props.ugradableType == UpgradableType.AdditiveInfinite)
        {
            value += (int)((float)gradeDifference * props.proceduralValuePerGrade);
        }
   

        return value;
    }



    #endregion


    private void ClearEventConnections()
    {
        OnUpgradeStateChanged = null;
        OnLevelSwitchStateChanged = null;
        OnMoneyChanged = null;
        OnLevelChanged = null;

    }

    private void SendEvents(bool upgradablesChanged = false)
    {
        OnMoneyChanged?.Invoke(totalMoney);

        for (int i = 0; i < upgradableStates.Length; i++)
        {
            OnUpgradeStateChanged?.Invoke(i);
        }

        
        OnLevelSwitchStateChanged?.Invoke();
    }

    #region Database save load helpers
    private UpgradableState LoadUpgradableState(UpgradableProperties props, int levelIndex)
    {
        UpgradableState upgradableState = new UpgradableState();
        upgradableState.upgradableName = props.upgradableName;

        string upgradableGradeKey = systemProperties.sameUpgradeStatesAcrossLevels
            ? upgradableState.upgradableName
            : upgradableState.upgradableName + "L" + levelIndex;

        upgradableState.grade = PlayerPrefs.GetInt(upgradableGradeKey);
        PurchaseRequestResult canBeUpgraded = CanUpgrade(props, upgradableState.grade + 1);
        RefreshUpgradableVariables(props, upgradableState, canBeUpgraded);

        return upgradableState;
    }

    private void SaveGradeChange(string upgradeName, int grade, int levelIndex)
    {

        string upgradableGradeKey = systemProperties.sameUpgradeStatesAcrossLevels
          ? upgradeName
          : upgradeName + "L" + levelIndex;

        PlayerPrefs.SetInt(upgradableGradeKey, grade);

    }

    private void SaveMoneyChange(int money, int levelIndex)
    {
        PlayerPrefs.SetInt(totalMoneyPrefsKey, money);
    }

    private void SaveLevelChange(int levelIndex)
    {
        PlayerPrefs.SetInt("levelIndex", levelIndex);

        if (levelIndex > lastLevelUnlocked)
        {
            lastLevelUnlocked = levelIndex;
            PlayerPrefs.SetInt("lastLevelUnlocked", lastLevelUnlocked);
        }

    }



    #endregion


    

}
