using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnlockableValues", order = 1)]
public class UnlockableValues : ScriptableObject
{
    const int NO_ITEM_UNLOCKED = -1;
    // Settings
    public string[] itemKeys;
    public int[] itemValues;
    // Connections

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

    public int GetLastUnlockedItemID(int value)
    {
        int result = NO_ITEM_UNLOCKED;
        for(int i = 0; i < itemValues.Length; i++)
        {
            if (itemValues[i] > value)
                break;
            result = i;
        }
        return result;
    }

}
