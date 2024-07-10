using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    // Settings
    public bool oneShot;
    public float oneShotTime;
    public float powerMultiplier = 2;
    // Connections
    Animator animator;
    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        animator = GetComponent<Animator>();
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUse(Transform target)
    {

        transform.position = target.position;
        animator.SetTrigger("Start");
    }

    public void OnContinuousUse(Transform target = null)
    {
        animator.SetTrigger("Play");
    }

    public void OnPauseUse()
    {
        if(!oneShot)
        animator.SetTrigger("Pause");
    }

    public void OnStopUse()
    {
        animator.SetTrigger("Stop");
    }

}
