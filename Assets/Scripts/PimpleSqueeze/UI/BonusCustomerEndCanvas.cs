using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BonusCustomerEndCanvas : MonoBehaviour
{

    // Settings

    // Connections
    public TextMeshProUGUI earnedMoneyText;
    public event Action OnClaimed;
    public Button claimButton;
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

    public void SetEarnedMoneyText(int earnedMoney)
    {
        earnedMoneyText.text = (earnedMoney / 100).ToString();
    }

    public void OnClaimButtonPressed()
    {
        OnClaimed?.Invoke();
        claimButton.interactable = false;
    }

    private void OnEnable()
    {
        claimButton.interactable = true;
    }

}
