//using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class UnlockableItem
{
    // Attributes
    public string itemName;
    public int experienceNeeded;
    // State variables
    public bool isUnlockedAlready;
    public int currentExperience = 0;
    
    public float GetPercent() {
        return (float)currentExperience / (float)experienceNeeded;
    }

}



public class ItemUnlockManager : MonoBehaviour
{
    // Settings
    public UnlockableItem[] unlockableItems;

    public int experiencePerCustomer = 10;

    // Connections
    Dictionary<string, UnlockableItem> itemsDictionary = null;
    public ItemUnlockPanel itemUnlockPanel;
    // State variables

    int currentItemIndex;
    int currentUnlockExperience;


    int oldItemIndex;
    float oldUnlockPercent;
    bool allItemsUnlocked = false;

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        InitDictionary();
        LoadState();
    }

    void InitState(){

    }

    void InitDictionary()
    {
        itemsDictionary = new Dictionary<string, UnlockableItem>();
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            itemsDictionary.Add(unlockableItems[i].itemName.ToLower(), unlockableItems[i]);
        }
    }

    void LoadState()
    {
        currentUnlockExperience = PlayerPrefs.GetInt(nameof(currentUnlockExperience), 0);
        CalculateUnlockedItems(currentUnlockExperience);
        if(currentItemIndex > 0 && currentItemIndex < unlockableItems.Length)
            oldUnlockPercent = unlockableItems[currentItemIndex].GetPercent();
        oldItemIndex = currentItemIndex;
    }

    public void CalculateUnlockedItems(int currentExperience)
    {
        currentItemIndex = 0;
        int experienceCost = 0;
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            experienceCost += unlockableItems[i].experienceNeeded;

            if(currentUnlockExperience >= experienceCost)
                unlockableItems[i].isUnlockedAlready = true;
                
            if (currentUnlockExperience > experienceCost) // TODO: Greator
            {
                currentItemIndex++;
            }
            else
            {
                unlockableItems[i].currentExperience = unlockableItems[i].experienceNeeded - (experienceCost - currentUnlockExperience);
                break;
            }
        }   
    }
    public bool AllItemsUnlocked()
    {
        return currentItemIndex >= unlockableItems.Length;
    }

    public void EarnExperience(int experience)
    {
        currentUnlockExperience += experience;
        PlayerPrefs.SetInt(nameof(currentUnlockExperience), currentUnlockExperience);
        CalculateUnlockedItems(currentUnlockExperience);
    }

    public bool IsUnlocked(string key)
    {

     
        UnlockableItem item = itemsDictionary.GetValueOrDefault(key.ToLower(), null);
        if(item != null)
        {
            return item.isUnlockedAlready;
        }
        else
        {
            Debug.LogError("unlockable item key " + key + " does not exist");
            return false;
        }
    }


    public void UpdateUI()
    {

        if (AllItemsUnlocked()) return;

        float newUnlockPercent = unlockableItems[currentItemIndex].GetPercent();

        if (oldItemIndex != currentItemIndex) oldUnlockPercent = 0;

        itemUnlockPanel.DisplayItemUnlockPanel(currentItemIndex, newUnlockPercent, oldUnlockPercent);
        if(newUnlockPercent >= 1.0f)
        {
         //   GameAnalytics.NewDesignEvent("unlocked:" + unlockableItems[currentItemIndex].itemName, 0);
        }
        oldUnlockPercent = newUnlockPercent;
        oldItemIndex = currentItemIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
