using SupersonicWisdomSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WisdomSDKLoader : MonoBehaviour
{

    // Settings

    // Connections

    // State variables

    void Awake(){

        

        if (Debug.isDebugBuild)
        {
            // 13ea538d-d28f-cccc-bbbb-aaaaaaaaaaaa
           

            Debug.Log("This is a debug build!");
            GameAnalyticsSDK.GameAnalytics.SetCustomId("13ea538d-d28f-cccc-bbbb-aaaaaaaaaaaa");
        }

        // Subscribe
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady); 
        // Then initialize
        SupersonicWisdom.Api.Initialize();
    }


    void OnSupersonicWisdomReady()
    {
        SceneManager.LoadScene("MainSceneRev");
    }

}
