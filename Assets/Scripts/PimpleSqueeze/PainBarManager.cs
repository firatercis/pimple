using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainBarManager : MonoBehaviour
{
    public int counter = 0;
    public Image hurtingBar;

    void Start()
    {
        hurtingBar.fillAmount = 0;
    }

    public void IncreasePainLevel( )
    {
        counter++;
        hurtingBar.DOFillAmount(counter/Preferences.totalAcneNumber, .1f);
    }
}
