using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;



public interface ToolsMenuListener
{
    void OnDisplay();
}

public class ToolsMenu : MonoBehaviour
{

    // Settings
    public ToolPanelMode menuMode;
    // Connections
    public ToolPanel[] toolPanels;
    public event Action<int> OnItemConfirm;
    public Button confirmButton;
    public GameObject infoBG;
    public TextMeshProUGUI infoText;
    public ToolsMenuListener listener;
    // State variables
    int currentSelectedItem = -1;
    bool[] confirmableStates;
    InventoryItem[] items;
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        for (int i = 0; i < toolPanels.Length; i++)
        {
            toolPanels[i].mode = menuMode;
        }
    }

    void InitState(){

        DisplayInfo(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInfo(string text)
    {
        if(infoText != null)
        {
            infoBG.SetActive(true);
            //infoText.gameObject.SetActive(true);
            infoText.text = text;
        }
        else
        {
            infoBG.SetActive(false);
           // infoText.gameObject.SetActive(false);
        }
    }

    public void DisplayInventory(InventoryItem[] items)
    {
        this.items = items;
    
        confirmableStates = new bool[items.Length];
       
        for (int i = 0; i < items.Length; i++)
        {
            toolPanels[i].Display(items[i]);
            confirmableStates[i] = menuMode == ToolPanelMode.BuyMode ? items[i].isBuyable : items[i].quantity > 0;

        }

        ConfigConfirmButton();


    }

    void ConfigConfirmButton()
    {
        if (currentSelectedItem == -1) 
            confirmButton.interactable = false;
        else 
            confirmButton.interactable = confirmableStates[currentSelectedItem];
    }


    public void OnItemSelected(int selectedIndex)
    {
        currentSelectedItem = selectedIndex;
        for (int i = 0; i < toolPanels.Length; i++)
        {
            if (i == selectedIndex)
                toolPanels[i].Highlight();
            else
                toolPanels[i].UnHighlight();
        }


        DisplayInfo(items[selectedIndex].description);
        ConfigConfirmButton();

    }
    
    public void OnConfirmButtonPressed()
    {
        if(currentSelectedItem >= 0)
        {
            OnItemConfirm?.Invoke(currentSelectedItem);
        }
    }

    private void OnEnable()
    {
        listener.OnDisplay();
    }

}
