using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class ClinicUpgradeMenu : MonoBehaviour
{

    // Settings

    // Connections
    public TextMeshProUGUI clinicLevelText;
    public TextMeshProUGUI upgradeCostText;
    public Button upgradeButton;
    public event Action OnUpgradeRequest;
    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpgradeCost(int cost)
    {
        upgradeCostText.text = (cost / 100).ToString();
    }

    public void SetClinicLevelText(int level)
    {
        clinicLevelText.text = "Clinic Level " + (level+1); 
    }

    public void SetUpgradeButtonEnabled(bool isEnabled)
    {
        upgradeButton.interactable = isEnabled;
    }

    public void OnUpgradeButtonPressed()
    {

    }

    public void OnBackButtonPressed()
    {

    }

}
