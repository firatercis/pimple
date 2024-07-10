using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IncrementalUpgData", menuName = "IncrementalUpgData")]
public class IncrementalUpgSystemProperties : ScriptableObject
{
    public bool sameMoneyAcrossLevels;
    public bool sameUpgradeStatesAcrossLevels;
    public LevelProperties[] levelProperties;

    public TextAsset textAsset;

}

[System.Serializable]
public class LevelProperties
{
    public int unlockCost;
    public int initialMoney;
    public int incomeMultiplier;
    public GameObject backgroundPrefab;
    public UpgradableProperties[] upgradableProperties;
}


public enum UpgradableType
{
    Finite,
    AdditiveInfinite,
    MultiplicativeInfinite,
}

[System.Serializable]
public class UpgradableProperties
{
    public string upgradableName;
    public int[] costs;
    public float[] values;
    public UpgradableType ugradableType;
    public float proceduralCostCoefficient;
    public float proceduralValuePerGrade; // TODO: ProceduralArray
}



