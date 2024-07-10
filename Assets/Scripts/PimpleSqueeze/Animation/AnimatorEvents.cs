using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvents : MonoBehaviour
{
    //Settings

    // Connections
    public UnityEvent OnSitDownEndEvent;
    // State Variables 
    
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

    public void OnSitDownEnd()
    {
        OnSitDownEndEvent.Invoke();
    }

}

