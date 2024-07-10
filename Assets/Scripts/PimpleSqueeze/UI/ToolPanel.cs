using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum ToolPanelMode
{
    BuyMode,
    UseMode
}
public class ToolPanel : MonoBehaviour
{
    // Settings
    public ToolPanelMode mode;
    // Connections
    public TextMeshProUGUI countText;
    public TextMeshProUGUI costText;
    public GameObject disableMask;
    public GameObject maxTextGO;
    public Button selectButton;
    public GameObject highlight;
    public GameObject lockGO;
    // State variables
    public bool isBuyable; // TODO: Tersine veri akisi, felaket!
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

    public void Display(InventoryItem item)
    {
        countText.text = "" + item.quantity.ToString() ;
        if(costText != null && mode ==  ToolPanelMode.BuyMode)
        {
            costText.text = "$" + item.cost / 100;
        }
        
        SetAccessState(item);
    }

    public void SetAccessState(InventoryItem item)
    {
        if (item.isLocked)
        {
            UnHighlight();
            lockGO.SetActive(true);
            disableMask.SetActive(true);
            return;
        }
        else
        {
            lockGO.SetActive(false);
        }

        bool accessible = mode == ToolPanelMode.BuyMode ? item.isBuyable : item.quantity > 0;

        if (!accessible)
        {
            disableMask.SetActive(true);
            selectButton.interactable = false;
        }
        else
        {
            disableMask.SetActive(false);
            selectButton.interactable = true;
        }

        if(mode == ToolPanelMode.BuyMode)
        {
            if (item.quantity >= item.maxQuantity)
            {
                maxTextGO.SetActive(true);
            }
            else
            {
                maxTextGO?.SetActive(false);
            }
        }

        
    }

    public void Highlight()
    {
        highlight.SetActive(true);
    }

    public void UnHighlight()
    {
        highlight.SetActive(false);
    }

}
