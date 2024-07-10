using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameAnalyticsSDK;

public class ClinicUpgradeManager : MonoBehaviour
{
    const int GLOW_MAT_INDEX = 1;

    // Settings
    public int[] clinicUpgradeCosts;
    public Color[] gloveColors;
    public int[] gloveMatIndices;
    // Connections
    public GameObject[] environmentPrefabs;
    public ClinicUpgradeMenu clinicUpgradeMenu;
    public Transform environmentPlacement;
    public CinemachineVirtualCamera upgradeRoomVirtualCamera;
    IncrementalUpgradeManager incrementalUpgradeManager;
    UserProgressDatabase userProgressDatabase;
    public Renderer[] handRenderers;

    // State variables

    GameObject currentEnvironmentParent;

    public int clinicLevel;

    void Awake(){
        InitConnections();
    }

    void Start()
    {
       
    }

    void InitConnections(){
        incrementalUpgradeManager = GetComponent<IncrementalUpgradeManager>();
        userProgressDatabase = incrementalUpgradeManager.GetDatabase();
    }

    public void LoadState(){
        clinicLevel = PlayerPrefs.GetInt(nameof(clinicLevel), 0);
        LoadEnvironment(clinicLevel);
    }

    public void LoadEnvironment(int environmentIndex)
    {
        if (environmentIndex >= environmentPrefabs.Length) environmentIndex = environmentPrefabs.Length - 1;
       
        if (currentEnvironmentParent != null)
            Destroy(currentEnvironmentParent);
        currentEnvironmentParent = Instantiate(environmentPrefabs[environmentIndex], environmentPlacement.position, environmentPlacement.rotation);
        ChangeGloveColor(environmentIndex);
    }

    void ChangeGloveColor(int environmentIndex)
    {
        for(int i=0; i<handRenderers.Length; i++)
        {
            Material[] handMaterials = handRenderers[i].materials;

            for(int j=0; j<gloveMatIndices.Length; j++)
            {
                handMaterials[gloveMatIndices[j]].color = gloveColors[environmentIndex];
            }
        }
    }

    public int GetUpgradeCost()
    {
        return clinicUpgradeCosts[clinicLevel];
    }

    public void SwitchToClinicUpgradeMode()
    {
        upgradeRoomVirtualCamera.gameObject.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        LoadState();
        bool canUpgradeClinic = userProgressDatabase.totalMoney >= GetUpgradeCost();

        clinicUpgradeMenu.SetClinicLevelText(clinicLevel);
        clinicUpgradeMenu.SetUpgradeCost(GetUpgradeCost());
        clinicUpgradeMenu.SetUpgradeButtonEnabled(canUpgradeClinic);
    }

    public void BackToGameplayMode()
    {
        upgradeRoomVirtualCamera.gameObject.SetActive(false);
    }

    public bool TryUpgrade()
    {
        bool couldUpgrade = false;
        int upgradeCost = clinicUpgradeCosts[clinicLevel];
        if(userProgressDatabase.totalMoney >= upgradeCost)
        {
            couldUpgrade = true;
            userProgressDatabase.OnMoneySpendRequest(upgradeCost);
            clinicLevel++;
            PlayerPrefs.SetInt(nameof(clinicLevel), clinicLevel);
            LoadEnvironment(clinicLevel);
            GameAnalytics.NewDesignEvent("clinicUpgrade", clinicLevel); // TODO: GameAnalytics direct access
        }
        return couldUpgrade;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
