using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceImpressionModifier : MonoBehaviour
{

    // Settings

    // Connections
    public Animator animator;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("Smile");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("GetHurt");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("GetAngry");
        }
    }
}
