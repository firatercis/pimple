using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultCaptionManager : MonoBehaviour
{
    //Settings

    // Connections
    public GameObject[] possibleCaptions;
    public float[] captionValues;
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

    public void DisplayCaption(float value)
    {
        int index = 0;
        for(int i=1; i< captionValues.Length; i++)
        {
            if (value < captionValues[i]) break;
            index++;
        }

        for(int i=0; i< captionValues.Length; i++)
        {
            possibleCaptions[i].gameObject.SetActive(i == index);
        }
    }
}

