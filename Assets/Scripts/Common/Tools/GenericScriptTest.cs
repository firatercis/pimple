using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericScriptTest : MonoBehaviour
{
    //Settings

    // Connections
    public UnityEvent tests;
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
    public void LaunchTest()
    {
        //for (int i = 0; i < tests.Length; i++)
        //{
        //    tests[i].Invoke();
        //}
        tests.Invoke();
    }
}

