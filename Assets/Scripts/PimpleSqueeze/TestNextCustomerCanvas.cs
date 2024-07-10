using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestNextCustomerCanvas : MonoBehaviour
{
    //Settings

    // Connections
    public NextCustomerCanvasManager nextCustomerCanvasManager;
    // State Variables 
    float acneLength = 0;
    float maxAcneLength = 100;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        //InitState();
    }
    void InitConnections(){
        nextCustomerCanvasManager.OnNextCustomerRequest += OnNextCustomerButtonPressed;
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            acneLength += 10;
            nextCustomerCanvasManager.gameObject.SetActive(true);
            nextCustomerCanvasManager.DisplayCanvas(40.0f, 745, acneLength, maxAcneLength,100);

        }
    }

    void OnNextCustomerButtonPressed()
    {
        Debug.Log("Next customer request");
    }

}

