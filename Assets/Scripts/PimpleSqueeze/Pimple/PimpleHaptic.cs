using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Static;

public class PimpleHaptic : MonoBehaviour
{
    //Settings
    public float hapticTimePeriod = 2.0f;
    // Connections

    // State Variables 
    private float hapticTimeCounter;
    
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

    public void OnSqueeze(bool inDanger = false)
    {
        if (!GameSettingsBase.vibrationOn) return;
        hapticTimeCounter += Time.deltaTime;
        if (hapticTimeCounter >= hapticTimePeriod)
        {
            hapticTimeCounter = 0;
            if (inDanger)
                Taptic.Heavy();
            else
                Taptic.Medium();
        }
    }

    public void OnKilled()
    {

        if (!GameSettingsBase.vibrationOn) return;
        Taptic.Heavy();
    }

    public void OnHealed()
    {
        if (!GameSettingsBase.vibrationOn) return;
        Taptic.Success();
    }

    void OnRelease()
    {

    } 
}

