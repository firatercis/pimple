using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleToolResponse : MonoBehaviour
{

    // Settings

    // Connections
    public event Action OnScalpel;
    public event Action OnExtractor;
    public event Action OnNeedle;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scalpel"))
        {
            OnScalpel?.Invoke();
        }else if (other.CompareTag("Needle"))
        {
            OnNeedle?.Invoke();
        }else if (other.CompareTag("Extractor"))
        {
            OnExtractor?.Invoke();
        }
    }

    

}
