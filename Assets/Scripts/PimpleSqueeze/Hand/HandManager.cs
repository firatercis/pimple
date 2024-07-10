using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandManager : MonoBehaviour
{
    public GameObject tissue;

    public float endValue = .75f;
    public float sweepValue = 1f;
    
    public TwoBoneIKConstraint rightRig;
    public TwoBoneIKConstraint topRig;

    public Animator leftHand;
    public Animator rightHand;
    public Animator rightHandSweep;

    public List<GameObject> acnesOnGlove = new List<GameObject>();

    public void OpenTissue()
    {
        rightHandSweep.SetBool("Open", true);
        tissue.SetActive(true);
        rightRig.weight = 0;

        DOTween.To( ()=>0, endValue => topRig.weight = endValue, endValue, .5f);
        DOTween.To(() => endValue, sweepValue => topRig.weight = sweepValue, sweepValue, .5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void CloseTissue()
    {
        rightHandSweep.SetBool("Open", false);
        tissue.SetActive(false);
        rightRig.weight = 1;
    }

    public void OpenAcnes()
    {
        if (Preferences.currentAcneIndex <= acnesOnGlove.Count && Preferences.currentAcneIndex > 0)
        {
            for (int i = 0; i < Preferences.currentAcneIndex; i++)
            {
                acnesOnGlove[i].SetActive(true) ;
            }
        }
    }
}
