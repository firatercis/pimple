using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandApproachSignal : MonoBehaviour
{
    //Settings
    public  UnityEvent handApproachedEvent;
    // Connections
    
    // State Variables dafasdf
    
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

    public void OnHandApproached()
    {
        handApproachedEvent.Invoke();
    }

}

