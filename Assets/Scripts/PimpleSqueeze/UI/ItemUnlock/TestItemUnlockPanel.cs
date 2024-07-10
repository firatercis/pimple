using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemUnlockPanel : MonoBehaviour
{

    // Settings
    public int startUnlockableId;
    public float startPercent;
    public float incrementValue;
    // Connections
    public ItemUnlockPanel itemUnlockPanel;
    // State variables
    int currentUnlockableId;
    float currentPercent = 0;
    float oldPercent;
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
        currentUnlockableId = startUnlockableId;
        currentPercent = startPercent;
        oldPercent = currentPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            currentPercent += incrementValue;
            itemUnlockPanel.DisplayItemUnlockPanel(currentUnlockableId, currentPercent, oldPercent);
            oldPercent = currentPercent;
            if(currentPercent >= 1.0f)
            {
                currentPercent = 0;
                oldPercent = 0;
                currentUnlockableId++;
            }
        }    
    }
}
