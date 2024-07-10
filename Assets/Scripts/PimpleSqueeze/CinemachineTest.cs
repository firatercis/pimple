using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CinemachineTest : MonoBehaviour
{
    //Settings

    // Connections
    public GameObject[] virtualCamGOs;
    // State Variables
    int currentCamIndex;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        currentCamIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentCamIndex++;
            currentCamIndex = currentCamIndex % virtualCamGOs.Length;
            SetVirtualCamActive(currentCamIndex);
        }
    }

    void SetVirtualCamActive(int currentCamIndex)
    {
        for(int i=0; i< virtualCamGOs.Length; i++)
        {
            if (i == currentCamIndex) virtualCamGOs[i].SetActive(true);
            else virtualCamGOs[i].SetActive(false);
        }
    }
}

