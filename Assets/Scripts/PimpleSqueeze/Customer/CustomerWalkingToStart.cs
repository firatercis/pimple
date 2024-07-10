using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UIElements;

public class CustomerWalkingToStart : MonoBehaviour
{
    const int LAY_DOWN_N_JUMPS = 1;
    // Settings
    public float walkingTime = 1.0f;
    public float turnTime = 0.5f;
    public float layDownTime = 1.0f;
    public float bedPlacementShift = 2.0f;
    // Connections
    private Animator animator;
    public event Action OnPlacementEnd;
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
    public void PlaceToBed(Animator animator,Transform bedPoint, Vector3 positionToLay)
    {
        this.animator = animator;
        transform.DOMove(bedPoint.position, walkingTime)
            .OnComplete(() =>TurnBackToBed(bedPoint.rotation.eulerAngles,positionToLay));
    }
    void TurnBackToBed(Vector3 bedPointRotation, Vector3 positionToLay)
    {
        transform.DORotate(bedPointRotation, turnTime).OnComplete(()=> LayDownToBed(positionToLay));
    }
    void LayDownToBed(Vector3 positionToLay)
    {
        //transform.DOJump(positionToLay, bedPlacementShift, LAY_DOWN_N_JUMPS, layDownTime);
        animator.SetTrigger("SitDown");
    }
}


