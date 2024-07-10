using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemUnlockPanel : MonoBehaviour
{

    // Settings
    public float percentFillDuration = 1.0f;
    public string[] titles;
    public string[] descriptions;
    public float itemBannerPunchScale = 0.2f;
    // Connections
    public Image[] fillBackgroundImages;
    public Image[] fillForegroundImages;
    public TextMeshProUGUI captionText;
    public GameObject itemBannerGO;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI percentCounterText;
    public RotateGlow rotateGlow;
    public GameObject claimButtonGO;
    public GameObject continueButtonGO;
    public TextMeshProUGUI claimText;
    // State variables
    GameObject currentNextButtonGO;

    float currentPercent;
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

    public void DisplayItemUnlockPanel(int unlockableID, float newPercent, float oldPercent = 0)
    {
      
        if(newPercent >= 1.0f)
        {
            DisplayUnlockedMode(unlockableID);
            newPercent = 1.0f;
        }
        else
        {
            HideUnlockedMode();
        }

        gameObject.SetActive(true); // TODO: Canvas switcher
        fillForegroundImages[unlockableID].fillAmount = oldPercent;
        SetPercentDisplay(unlockableID, newPercent);
        TweenPercentText(oldPercent, newPercent);

        itemBannerGO.SetActive(true);
        captionText.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
        captionText.text = titles[unlockableID];
        descriptionText.text = descriptions[unlockableID];


    }

    float GetPercentDisplay()
    {
        return currentPercent;
    }

    void SetPercentDisplay(int id,float percent)
    {
        SetImageActive(id);
        fillForegroundImages[id].DOFillAmount(percent, percentFillDuration);
    }

    void TweenPercentText(float oldValue, float newValue)
    {
        continueButtonGO.SetActive(false);
        claimButtonGO.SetActive(false);
        currentPercent = oldValue;
        DOTween.To(
            () => currentPercent,
            x => SetPercentText(x),
            newValue,
            percentFillDuration
        ).OnComplete(()=> currentNextButtonGO.SetActive(true));
    }

    void SetPercentText(float percent)
    {
        currentPercent = percent;
        percentCounterText.text = percent.ToString("00.0%") ;
    }

    void SetImageActive(int id)
    {
        // Hide all images
        for (int i = 0; i < fillBackgroundImages.Length; i++)
        {
            fillBackgroundImages[i].gameObject.SetActive(false);
            fillForegroundImages[i].gameObject.SetActive(false);
        }
        // Set the correct image active
        fillBackgroundImages[id].gameObject.SetActive(true);
        fillForegroundImages[id].gameObject.SetActive(true);
    }

    void DisplayUnlockedMode(int id)
    {
        rotateGlow.gameObject.SetActive(true);

        itemBannerGO.transform.DOPunchScale(itemBannerPunchScale * Vector3.one, 0.5f, vibrato:0);
        currentNextButtonGO = claimButtonGO;
    }
    void HideUnlockedMode()
    {
        rotateGlow.gameObject.SetActive(false);

        currentNextButtonGO = continueButtonGO;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
