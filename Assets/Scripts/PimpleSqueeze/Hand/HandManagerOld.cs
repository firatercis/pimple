using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManagerOld : MonoBehaviour
{
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;

    [SerializeField] private DOTweenAnimation leftHandDO;
    [SerializeField] private DOTweenAnimation rightHandDO;

    public void GoPosition()
    {
        leftHandAnimator.SetBool("Position", true);
        rightHandAnimator.SetBool("Position", true);

        DOVirtual.DelayedCall(1f, ()  => {
            leftHandAnimator.SetBool("Position", false);
            rightHandAnimator.SetBool("Position", false);
        });
    }

    public void PlayHandsUp()
    {
        StopSqueeze();
        leftHandAnimator.SetBool("HandsUp", true);
        rightHandAnimator.SetBool("HandsUp", true);
    }

    public void PlaySqueeze()
    {
        leftHandAnimator.SetBool("Squeeze", true);
        rightHandAnimator.SetBool("Squeeze", true);
    }

    public void StopSqueeze()
    {
        leftHandAnimator.SetBool("Squeeze", false);
        rightHandAnimator.SetBool("Squeeze", false);
    }

    public void PlayHandShake()
    {
        leftHandDO.DOPlay();
        rightHandDO.DOPlay();
    }

    public void StopHandShake()
    {
        leftHandDO.DOPause();
        rightHandDO.DOPause();
    }
}
