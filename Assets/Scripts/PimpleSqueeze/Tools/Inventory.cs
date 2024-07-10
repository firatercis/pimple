using GameAnalyticsSDK;
using Obi;
using SoftwareKingdom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;



public class Inventory : MonoBehaviour, ToolsMenuListener
{
    const int NONE_UNLOCKED = -1;
    // Settings
    public int itemAddPerPurchase = 1;
    // Connections
    IncrementalUpgradeManager incrementalUpgradeManager;
    UserProgressDatabase progressDatabase;
    public InventoryItem[] items;
    public ToolsMenu useToolsMenu;
    public ToolsMenu buyToolsMenu;
    public ItemUnlockManager itemUnlockManager;
    public event Action<int> OnItemUsed;

    // State variables
    int lastUnlockedItem = NONE_UNLOCKED;
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void LoadItems()
    {
        for(int i=0; i<items.Length; i++)
        {
            items[i].quantity = PlayerPrefs.GetInt(items[i].prefsKey,0);
        }
    }

    public int GetQuantity(int itemID)
    {
        return items[itemID].quantity;
    }

    public bool UseItem(int itemID)
    {
        bool result = true;
        if(items[itemID].quantity > 0)
        {
            items[itemID].quantity--;
            SaveItem(itemID);
            UpdateUI();
            OnItemUsed?.Invoke(itemID);
            GameAnalytics.NewDesignEvent("used:" + items[itemID].itemName, 0);
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void UpdateUI()
    {

        UpdateStates();

        useToolsMenu.DisplayInventory(items);
        buyToolsMenu.DisplayInventory(items);
    }

    public void UpdateStates()
    {
        for(int i=0; i < items.Length; i++)
        {
            items[i].SetBuyable(progressDatabase.totalMoney);
            bool itemIsUnlocked = itemUnlockManager.IsUnlocked(items[i].itemName);

            if (itemIsUnlocked)
            {
                items[i].SetUnlocked();
            }
        }
    }

    public void AddItem(int itemID, int quantity)
    {
        items[itemID].quantity += quantity;
        SaveItem(itemID);
        UpdateUI();

    }

    public void BuyItem(int itemID)
    {
        progressDatabase.OnMoneySpendRequest(items[itemID].cost);
        AddItem(itemID, itemAddPerPurchase);
        GameAnalytics.NewDesignEvent("bought:" + items[itemID].itemName, 0);
    }

    void SaveItem(int id)
    {
        PlayerPrefs.SetInt(items[id].prefsKey, items[id].quantity);
        
    }

    void InitConnections(){
        useToolsMenu.OnItemConfirm += OnInventoryUse;
        buyToolsMenu.OnItemConfirm += BuyItem;
        useToolsMenu.listener = this;
        buyToolsMenu.listener = this;
        
        incrementalUpgradeManager = GetComponent<IncrementalUpgradeManager>();
        progressDatabase = incrementalUpgradeManager.GetDatabase();
        itemUnlockManager = GetComponent<ItemUnlockManager>();

    }

    void InitState(){
        LoadItems();

        UpdateStates();

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInventoryUse(int itemID)
    {
        
        UseItem(itemID);
    }

    public void SetLastUnlockedItem(int index)
    {
        lastUnlockedItem = index;
        UpdateStates();
    }

    public void OnDisplay()
    {
        UpdateUI();
    }
}
