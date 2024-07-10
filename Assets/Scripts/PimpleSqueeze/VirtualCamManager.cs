using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamManager : MonoBehaviour
{
    //Settings

    // Connections
    CinemachineVirtualCamera topVirtualCam;
    // State Variables dafasdf
    CinemachineVirtualCamera currentVirtualCam;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        currentVirtualCam = topVirtualCam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}

