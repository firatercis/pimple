using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class IncrementalUpgradeUIManager : MonoBehaviour
{
    const int CENT_DOLLAR_FACTOR = 100;
    //Settings
    public float moneyPunchScaleStrength = 0.5f;
    public float moneyPunchScaleDuration = 1.0f;
    // Connections
    public event Action<int> OnUpgradeClicked;
    public event Action OnNextLevelClicked;
    public event Action OnPreviousLevelClicked;

    public TextMeshProUGUI userMoneyText;
    public TextMeshProUGUI userMoneyTextSecondary;
    public UpgradeButton[] upgradeButtons;
    public LevelSwitchButton nextLevelButton;
    public LevelSwitchButton previousLevelButton;
    public RectTransform moneyEarnAnimationTransform;
    // State Variables dafasdf

    Tween punchScaleTween = null;
    Vector3 initialMoneyIconScale;

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
    }
    void InitState(){
        initialMoneyIconScale = moneyEarnAnimationTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {


            // moneyEarnAnimationTransform.DOPunchAnchorPos(Vector3.one * 100, 2);
            
        }
    }

    public void OnUpgradeButtonClicked(int buttonIndex)
    {
        OnUpgradeClicked?.Invoke(buttonIndex);
    }

    public void OnNextLevelButtonClicked()
    {
        OnNextLevelClicked?.Invoke();
    }

    public void OnPreviousLevelButtonClicked()
    {
        OnPreviousLevelClicked?.Invoke();
    }


    public void SetUserMoney(int money)
    {
        string moneyString = "$ " + (money / CENT_DOLLAR_FACTOR);

        userMoneyText.text = moneyString;
        if (userMoneyTextSecondary != null)
            userMoneyTextSecondary.text = moneyString;
        //moneyEarnTweenAnimation.DOPlay();

        if (punchScaleTween != null)
        {
            punchScaleTween.Kill();
            moneyEarnAnimationTransform.localScale = initialMoneyIconScale;
        }

        punchScaleTween = moneyEarnAnimationTransform.DOPunchScale(Vector3.one * moneyPunchScaleStrength, moneyPunchScaleDuration, vibrato: 0);

    }

    public void SetUpgradableDisplay(int buttonIndex, UpgradableState state )
    {
        upgradeButtons[buttonIndex].SetPrice(state.price);
        upgradeButtons[buttonIndex].SetLevel(state.grade);
        upgradeButtons[buttonIndex].SetMaxed(state.isPurchasable == PurchaseRequestResult.IsMaxed);
        upgradeButtons[buttonIndex].SetInteractable(state.isPurchasable == PurchaseRequestResult.OK);
        // TODO: ismaxed?
    }

    public void SetLevelSwitchDisplay(LevelSwitchState state)
    {
        nextLevelButton?.SetInteractable(state.nextLevelSwitchable == PurchaseRequestResult.OK || state.nextLevelSwitchable == PurchaseRequestResult.DoesNotNeedToPurchase);
        nextLevelButton?.SetMaxed(state.nextLevelSwitchable == PurchaseRequestResult.IsMaxed);
        nextLevelButton?.SetPrice(state.nextLevelPrice);
        if(previousLevelButton != null)
        {
            previousLevelButton.SetMaxed(!state.previousLevelAvailable);
        }
    }

    public void SetUpgradeButtonsVisible(bool isVisible = true)
    {
        for(int i=0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].gameObject.SetActive(isVisible);
        }
        previousLevelButton?.gameObject.SetActive(isVisible); //TODO: kafa karisikligi olur burda
        nextLevelButton?.gameObject.SetActive(isVisible);
    }
}

