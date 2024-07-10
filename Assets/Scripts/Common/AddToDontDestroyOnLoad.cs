using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddToDontDestroyOnLoad : MonoBehaviour
{
    //Settings

    // Connections
    public GameObject[] targetGameObjects;
    // State Variables dafasdf

    // Start is called before the first frame update

    private void Awake()
    {
        for(int i=0; i< targetGameObjects.Length; i++)
        {
            DontDestroyOnLoad(targetGameObjects[i]);
        }

    }

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
}

