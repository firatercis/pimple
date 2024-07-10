using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BonusFillerUI : MonoBehaviour
{
    // Constants
    const float MAX_FILL_VAL = 1.0f;

    // Settings
    public float fillTime = 1.0f;
    // Connections
    public Image fillerImage;
    public TextMeshProUGUI fillerImageText;
    // State variables
    float oldVal;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetOldValue(float value)
    {

        oldVal = Mathf.Clamp(value, 0, MAX_FILL_VAL); 
    }

    public void DisplayValueWithFiller(float newValue, float maxValue)
    {
        float fillPercent = newValue / maxValue;
        fillPercent = Mathf.Clamp(fillPercent, 0, MAX_FILL_VAL);
        float oldFillPercent = oldVal / maxValue;
        oldFillPercent = Mathf.Clamp(oldFillPercent, 0, MAX_FILL_VAL);  
        fillerImage.fillAmount = oldFillPercent;
        DOTween.To(
            () => fillerImage.fillAmount,
            x => fillerImage.fillAmount = x,
            fillPercent,
            fillTime);
    }

}
