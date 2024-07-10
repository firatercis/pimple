using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PimpleAnimation : MonoBehaviour
{
    private Animator animator;
    public GameObject blackDotGO;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayBlackHeadAnim()
    {
        //  animator.SetBool("BlackHeadRemove", true);
        animator.SetBool("Scalpelled", true);
    }

    public void PlayResistAnim()
    {
        
        animator.SetBool("Resist",true);
    }

   

    public void PlayNeedleAnim()
    {
        animator.SetBool("Needle", true);
    }

    public void PlaySqueezeAnim()
    {
        animator.SetBool("Squeeze", true);
 
    }

    public void PlayScalpel(bool blackDotVisible = false)
    {
        blackDotGO.SetActive(blackDotVisible);
        animator.SetTrigger("Scalpel");
        animator.SetBool("Resist", false);
    }

    public void PlayNeedle()
    {
        animator.SetTrigger("Needle");
    }

    public void StopSqueezeAnim()
    {
        animator.SetBool("Resist", false);
        animator.SetBool("Squeeze", false);
    }

    public void SweepAnim()
    {
        animator.SetTrigger("Sweep");
    }

    public void ReviveAnim()
    {
        animator.SetTrigger("Revive");
    }
    public void PlayBlackDotFinished()
    {
        animator.SetTrigger("BlackDotFinished");
    }
}
