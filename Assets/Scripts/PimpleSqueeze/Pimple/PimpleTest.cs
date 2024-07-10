using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleTest : MonoBehaviour
{

    // Settings

    // Connections
    public AcneManagerRev pimpleToBeTested;
    public Animator pimpleAnimator;
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
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Heal
            pimpleAnimator.SetTrigger("Sweep");
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            pimpleAnimator.SetTrigger("Revive");
        }
    }
}
