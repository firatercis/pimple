using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelUnlockManager : MonoBehaviour
{

    // Settings
    public int bonusXPIncreasePerLevel = 10;
    public int baseXPRequiredBase = 30;
    // Connections

    // State variables
    int nBonusLevelsPassed;
    public int requiredBonusXPForNextLevel; // TODO: public state variable
    public int currentBonusXP;

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
        // Load saved state
        nBonusLevelsPassed = PlayerPrefs.GetInt(nameof(nBonusLevelsPassed), 0);
        currentBonusXP = PlayerPrefs.GetInt(nameof(currentBonusXP), 0);
        requiredBonusXPForNextLevel = baseXPRequiredBase + nBonusLevelsPassed * bonusXPIncreasePerLevel; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool EarnBonusXP(int bonusXP)
    {
        bool bonusLevelUnlocked = false;
        currentBonusXP += bonusXP;
        if(currentBonusXP >= requiredBonusXPForNextLevel)
        {
            currentBonusXP = requiredBonusXPForNextLevel;
            bonusLevelUnlocked = true;
        }

        return bonusLevelUnlocked;
    }

    public float GetProgress()
    {
        return (float)currentBonusXP / (float)requiredBonusXPForNextLevel;
    }

    public void OnBonusLevelOpened()
    {
        currentBonusXP = 0;
        nBonusLevelsPassed++;
        requiredBonusXPForNextLevel = baseXPRequiredBase + nBonusLevelsPassed * bonusXPIncreasePerLevel;
    }
}
