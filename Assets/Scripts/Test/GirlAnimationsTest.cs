using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GirlAnimationsTest : MonoBehaviour
{
    // Settings

    // Connections
    public Animator animator;
    CustomerWalkingToStart girlWalkingToStart;
    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        girlWalkingToStart = GetComponent<CustomerWalkingToStart>();
        girlWalkingToStart.OnPlacementEnd += OnApproachedToBed;
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            animator.SetTrigger("SitDown");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("StandUp");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("TurnAround");
        }
    }
    void OnApproachedToBed()
    {
        animator.SetTrigger("SitDown");
    }
}
