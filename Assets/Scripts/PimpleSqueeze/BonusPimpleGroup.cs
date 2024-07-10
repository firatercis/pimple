using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BonusPimpleGroup : MonoBehaviour
{

    // Settings

    // Connections
    public CinemachineVirtualCamera pimplesTopCam;
    public AcneManagerRev[] pimples;
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

    public void HealAll()
    {
        for(int i=0; i < pimples.Length; i++)
        {
            pimples[i].Heal(ropeExists:false, hide:true);
            
        }
    }
}
