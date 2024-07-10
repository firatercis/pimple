using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour
{

    // Settings
    public GameObject[] onItems;
    public GameObject[] offItems;
    // Connections
    public UnityEvent<bool> OnValueChanged;
    // State variables
    public bool value;
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
        SetValue(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(bool value)
    {
        this.value = value;
        for (int i = 0; i < onItems.Length; i++)
        {
            onItems[i].SetActive(value);
            offItems[i].SetActive(!value); 
        }
    }

    public void ToggleValue()
    {
        SetValue(!value);
        OnValueChanged.Invoke(value);
    }


    void ToggleOff()
    {

    }
}
