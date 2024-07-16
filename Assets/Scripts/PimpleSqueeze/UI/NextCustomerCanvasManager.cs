using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextCustomerCanvasManager : MonoBehaviour
{

    //const string TEST_RW_AD_ID = "ca-app-pub-3940256099942544/5354046379";
    const string TEST_RW_AD_ID = "ca-app-pub-6031861456314162/1304790186";
    //Settings

    // Connections
    public TextMeshProUGUI successPercentText;
    public TextMeshProUGUI customerEarningText;
    public TextMeshProUGUI totalMoneyText;
    public BonusFillerUI bonusFiller;
    public ResultCaptionManager resultCaptionManager;
    public Button claimButton;
    public event Action OnNextCustomerRequest;

    public GameObject rewardedAdManagerGO;
    private IRewardedAdManager rewardedAdManager;
    // State Variables dafasdf

    public float oldFillAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
        rewardedAdManager = rewardedAdManagerGO.GetComponent<IRewardedAdManager>();
    }
    void InitState(){
        oldFillAmount = 0;
        rewardedAdManager.LoadAd(TEST_RW_AD_ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCanvas(float successPercent,int customerEarning,float totalAcneNewValue, float totalAcneFullValue,int totalMoney)
    {

        resultCaptionManager?.DisplayCaption(successPercent);

        successPercentText.text = successPercent.ToString("00") + "%";
        customerEarningText.text = "$" + customerEarning.ToString();
        totalMoneyText.text = "$" + totalMoney.ToString();
        claimButton.gameObject.SetActive(true);
        //float targetFillAmount = totalAcneNewValue / totalAcneFullValue;

        //DOTween.To(
        //    () => acneLengthFillerImage.fillAmount,
        //    x => acneLengthFillerImage.fillAmount = x,
        //    targetFillAmount,
        //    fillingImageTime
        //    );

        // acneLengthFillerImage.fillAmount = totalAcneNewValue / totalAcneFullValue;
        //acneLengthText.text = totalAcneNewValue.ToString();

    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
    }

    public void OnNextCustomerButtonPressed()
    {
     
        oldFillAmount = 0;
        claimButton.gameObject.SetActive(false);

        // TODO: For testing of rewarded mobile ads:
        
        rewardedAdManager.ShowAd();
        OnNextCustomerRequest?.Invoke();
    }

    public void SetBonusFillerOldValue(float oldVal)
    {

        bonusFiller.SetOldValue(oldVal);
    }



}

