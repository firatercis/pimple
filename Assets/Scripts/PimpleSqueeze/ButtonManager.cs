using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SoftwareKingdom;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healingSpeedButtonPriceText;
    [SerializeField] private TextMeshProUGUI squeezePowerPriceText;
    [SerializeField] private TextMeshProUGUI newClinicMoneyText;
    [SerializeField] private TextMeshProUGUI earningsPriceText;
    [SerializeField] private TextMeshProUGUI totalMoneyText;

    [SerializeField] private TextMeshProUGUI healingSpeedText;
    [SerializeField] private TextMeshProUGUI squeezePowerLevelText;
    [SerializeField] private TextMeshProUGUI earningsLevelText;
    
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] darkBG;
 
    [Header("Prices")]
    public int healingSpeedPrice;
    public int squeezePowerPrice;
    public int newClinicPrice;
    public int earningsPrice;
    public int totalMoney;

    [Header("Price Multiplier")]
    public float healingSpeedPriceMultiplier;
    public float squeezePowerPriceMultiplier;
    public float earningsPriceMultiplier;

    [Header("Powers Start Values")]
    public float healingSpeed;
    public float squeezePower;
    public int earning;

    [Header("Power Value Multiplier")]
    public float healingSpeedMultiplier;
    public float squeezePowerMultiplier;
    public float earningMultiplier;
    
    //Upgrade level variables;
    private int healingSpeedLevel;
    private int squeezePowerLevel;
    private int earningsLevel;

    [Header("Button DOTween")]
    public Transform squeezeDO;
    public Transform healDO;
    public Transform earningDO;

    private void Awake()
    {
        healingSpeedPrice = PlayerPrefs.GetInt("HealingSpeedPrice", healingSpeedPrice);
        squeezePowerPrice = PlayerPrefs.GetInt("SqueezePowerPrice", squeezePowerPrice);
        earningsPrice = PlayerPrefs.GetInt("EarningsPrice", earningsPrice);
        totalMoney = PlayerPrefs.GetInt("TotalMoney", totalMoney);

        healingSpeedLevel = PlayerPrefs.GetInt("HealingSpeedLevel", 0);
        squeezePowerLevel = PlayerPrefs.GetInt("SqueezePowerLevel", 0);
        earningsLevel = PlayerPrefs.GetInt("EarningsLevel", 0);

        healingSpeed = PlayerPrefs.GetFloat("HealingSpeed", healingSpeed);
        squeezePower = PlayerPrefs.GetFloat("SqueezePower", squeezePower);
        earning = PlayerPrefs.GetInt("Earnings", earning);
        
        
        PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney", totalMoney));
        PlayerPrefs.SetFloat("HealingSpeed", PlayerPrefs.GetFloat("HealingSpeed", healingSpeed));
        PlayerPrefs.SetFloat("SqueezePower", PlayerPrefs.GetFloat("SqueezePower", squeezePower));
        PlayerPrefs.SetInt("Earnings", PlayerPrefs.GetInt("Earnings", earning));

        healingSpeedButtonPriceText.text = healingSpeedPrice.ToString();
        squeezePowerPriceText.text = squeezePowerPrice.ToString();
        earningsPriceText.text = earningsPrice.ToString();
        newClinicMoneyText.text = newClinicPrice.ToString();

        healingSpeedText.text = "LEVEL" + healingSpeedLevel;
        squeezePowerLevelText.text = "LEVEL" + squeezePowerLevel;
        earningsLevelText.text = "LEVEL" + earningsLevel;
    }

    void Start()
    {
        checkMoney();
    }

    public void SqueezePowerButtom()
    {
        if (totalMoney >= squeezePowerPrice)
        {
            KingdomAnalytics.CustomEvent("SqueezePower", 1);
            
            totalMoney -= squeezePowerPrice;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
            totalMoneyText.text = "$" + totalMoney;

            squeezePowerPrice =(int) (squeezePowerPrice * squeezePowerPriceMultiplier);
            squeezePower *= squeezePowerMultiplier;
            PlayerPrefs.SetFloat("SqueezePower", squeezePower);
            PlayerPrefs.SetInt("SqueezePowerPrice", squeezePowerPrice);

            var pos = squeezeDO.position;
            
            squeezePowerLevel += 1;
            PlayerPrefs.SetInt("SqueezePowerLevel", squeezePowerLevel);
            
            squeezePowerLevelText.text = "LEVEL " + squeezePowerLevel;

            squeezePowerPriceText.text = squeezePowerPrice.ToString();
        }
        checkMoney();
    }

    public void HealingSpeedButton()
    {
        if (totalMoney >= healingSpeedPrice)
        {
            KingdomAnalytics.CustomEvent("HealingSpeed", 1);

            totalMoney -= healingSpeedPrice;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
            totalMoneyText.text = "$" + totalMoney;

            healingSpeedPrice = (int)(healingSpeedPrice * healingSpeedPriceMultiplier);
            healingSpeed *= healingSpeedMultiplier;
            PlayerPrefs.SetFloat("HealingSpeed", healingSpeed);
            PlayerPrefs.SetInt("HealingSpeedPrice", healingSpeedPrice);

            healingSpeedLevel += 1;
            PlayerPrefs.SetInt("HealingSpeedLevel", healingSpeedLevel);

            healingSpeedText.text = "LEVEL " + healingSpeedLevel;

            healingSpeedButtonPriceText.text = healingSpeedPrice.ToString();
        }
        checkMoney();
    }

    public void EarningsButton()
    {
        if (totalMoney >= earningsPrice)
        {
            KingdomAnalytics.CustomEvent("Earnings", 1);

            totalMoney -= earningsPrice;
            PlayerPrefs.SetFloat("TotalMoney", totalMoney);
            totalMoneyText.text = "$" + totalMoney;

            earningsPrice = (int)(earningsPrice * earningsPriceMultiplier);
            earning = (int)(earning * earningMultiplier);
            PlayerPrefs.SetInt("Earnings", earning);
            PlayerPrefs.SetInt("EarningsPrice", earningsPrice);

            earningsLevel += 1;
            PlayerPrefs.SetInt("EarningsLevel", earningsLevel);
            
            earningsLevelText.text = "LEVEL " + earningsLevel;

            earningsPriceText.text = earningsPrice.ToString();
        }
        checkMoney();
    }

    public void NewClinicButton()
    {
        if (totalMoney > newClinicPrice)
        {
            PlayerPrefs.SetInt("ClinicPrice", newClinicPrice * 2);
            PlayerPrefs.SetInt("ClinicIndex", PlayerPrefs.GetInt("ClinicIndex", 0)+1);
        }
    }

    public void checkMoney()
    {
        totalMoney = PlayerPrefs.GetInt("TotalMoney");
        
        if (totalMoney < healingSpeedPrice)
        {
            darkBG[0].SetActive(true);
        }
        else
        {
            darkBG[0].SetActive(false);
        }
        
        if (totalMoney < squeezePowerPrice)
        {
            darkBG[1].SetActive(true);
        }
        else
        {
            darkBG[1].SetActive(false);
        }

        if (totalMoney < earningsPrice)
        {
            darkBG[2].SetActive(true);
        }
        else
        {
            darkBG[2].SetActive(false);
        }
        
        if (totalMoney < newClinicPrice)
        {
            darkBG[3].SetActive(true);
        }
        else
        {
            darkBG[3].SetActive(false);
        }
    }
}
