using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandsApproachTest : MonoBehaviour
{
    //Settings

    // Connections
    public PimpleWayPoints[] pimples;
    public HandManagerRev handCtrl;

    // State Variables dafasdf
    int currentPimpleIndex;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        currentPimpleIndex = 0;
        handCtrl.GoToPimple(pimples[currentPimpleIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentPimpleIndex++;
            currentPimpleIndex %= pimples.Length;
            handCtrl.GoToPimple(pimples[currentPimpleIndex]);
        }
    }
}

