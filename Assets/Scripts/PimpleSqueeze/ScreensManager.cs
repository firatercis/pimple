
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{

    // Settings

    // Connections
    public string[] screenNames;
    public GameObject[] screens;
    public GameObject[] popUpMenus;
    Dictionary<string, GameObject> screenNameDictionary;
    // State variables
    string lastScreen;
    List<string> openScreens;
    string storedState = "";
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        screenNameDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < screenNames.Length; i++)
        {
            screenNameDictionary.Add(screenNames[i].ToLower(), screens[i]);
        }

    }

    void InitState(){
        openScreens = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowScreen(string screenName)
    {
        screenName = screenName.ToLower();
        GameObject screen = screenNameDictionary[screenName];
        if(screen != null)
        {
            screen.SetActive(true);
        }
    }

    public void HideScreen(string screenName)
    {

        screenName = screenName.ToLower();
        GameObject screen = screenNameDictionary[screenName];
        if (screen != null)
        {
            lastScreen = screenName;
            screen.SetActive(false);
        }

    }

    public void HideAllScreens()
    {

        for (int i=0; i< screens.Length; i++)
        {
            if (screens[i].gameObject.activeSelf)
            {
                screens[i].SetActive(false);
                lastScreen = screenNames[i];
            }
        }

    }

    public void ShowScreenOnly(string screenName)
    {
        HideAllScreens();
        ShowScreen(screenName);
    }

    public void ShowScreenMultiple(string screenNamesOneLine)
    {

        string[] screenNames = screenNamesOneLine.Split(",");

        HideAllScreens();

        for(int i=0; i< screenNames.Length; i++)
        {
            ShowScreen(screenNames[i]);
        }


    }


}
