using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
public class UseToolsMenu : MonoBehaviour
{

    // Settings

    // Connections
    public TextMeshProUGUI[] itemCountTexts;
    public Button[] itemUseButtons;
    public GameObject[] itemParentGOs;
    public event Action<int> OnInventoryUse;
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

    public void DisplayInventory(InventoryItem[] items)
    {
        for(int i=0; i<items.Length; i++)
        {
            int quantity = items[i].quantity;
            if (items[i].quantity <= 0)
            {
                itemUseButtons[i].interactable = false;
            }
            else
            {
                itemUseButtons[i].interactable = true;
            }
            itemCountTexts[i].text = quantity.ToString();
            
        }
       
    }

    public void OnInventoryUseInput(int itemID)
    {
        

        OnInventoryUse?.Invoke(itemID);
    }
}
