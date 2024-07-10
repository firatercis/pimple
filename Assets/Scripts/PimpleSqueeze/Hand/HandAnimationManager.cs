using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandAnimationManager : MonoBehaviour
{
    //Settings

    // Connections
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    // State Variables
    bool playSqueezeTrigger = false;
    bool stopSqueezeTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
      

    }

    public void GoPosition()
    {
        leftHandAnimator.SetTrigger("TakePosition");
        rightHandAnimator.SetTrigger("TakePosition");
        leftHandAnimator.ResetTrigger("StartSqueeze");
        rightHandAnimator.ResetTrigger("StartSqueeze");

    }

    public void PlayHandsUp()
    {
        StopSqueeze();
        leftHandAnimator.SetTrigger("HandsUp");
        rightHandAnimator.SetTrigger("HandsUp");
    }

    public void PlaySqueeze()
    {
        leftHandAnimator.ResetTrigger("StopSqueeze");
        rightHandAnimator.ResetTrigger("StopSqueeze");

        leftHandAnimator.SetTrigger("StartSqueeze");
        rightHandAnimator.SetTrigger("StartSqueeze");
    }

    

    public void StopSqueeze()
    {
        leftHandAnimator.ResetTrigger("StartSqueeze");
        rightHandAnimator.ResetTrigger("StartSqueeze");
        leftHandAnimator.SetTrigger("StopSqueeze");
        rightHandAnimator.SetTrigger("StopSqueeze");
    }

    public void PlayClean()
    {
       // leftHandAnimator.SetBool("Squeeze", false);
      //  rightHandAnimator.SetTrigger("Clean");
    }

    public void SetVisible(bool visible)
    {
        leftHandAnimator.gameObject.SetActive(visible);
        rightHandAnimator.gameObject.SetActive(visible);
        if (visible == false)
        {
            // Reset the animators
            leftHandAnimator.SetTrigger("ResetPosition");
            rightHandAnimator.SetTrigger("ResetPosition");
        }

    }

   
}




