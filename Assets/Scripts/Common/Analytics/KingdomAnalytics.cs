

//#define ELEPHANT
#define GAME_ANALYTICS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//#if ELEPHANT
//using ElephantSDK;
//#endif
#if GAME_ANALYTICS
using GameAnalyticsSDK;
#endif




namespace SoftwareKingdom
{

    public class AnalyticsEntry
    {
        public string key;
        public float value;

        public AnalyticsEntry(string key, float value)
        {
            this.key = key;
            this.value = value;
        }   
    }

    public class KingdomAnalytics
    {
        public static bool initialized;
        public static void Init()
        {
            
#if GAME_ANALYTICS
            if (!initialized)
            {
                Debug.Log("Initialized Game Analytics");
                initialized = true;
                GameAnalytics.Initialize();
            }
#endif
        }

        public static void LevelCompleted(int levelIndex, params string[] parameters)
        {
            Debug.Log("Sending level completed event: " + levelIndex);
            PrintParams(parameters);
//#if ELEPHANT
//            Params analyticParams = GetParams(parameters);
//            Elephant.LevelCompleted(levelIndex + 1, analyticParams);
//#endif

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,"Level Completed", levelIndex);
#endif

        }

        public static void LevelFailed(int levelIndex, params string[] parameters)
        {

            Debug.Log("Sending level failed event: " + levelIndex);
            PrintParams(parameters);
//#if ELEPHANT
//            Params analyticParams = GetParams(parameters);
//            Elephant.LevelFailed(levelIndex  + 1, analyticParams);     
//#endif

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level Failed", levelIndex);
#endif

        }

        public static void LevelStarted(int levelIndex, params string[] parameters)
        {
            Debug.Log("Sending level started event: " + levelIndex);
            PrintParams(parameters);
//#if ELEPHANT
//            Params analyticParams = GetParams(parameters);
//            Elephant.LevelStarted(levelIndex + 1, analyticParams);
//#endif

#if GAME_ANALYTICS
            Init();
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level Started", levelIndex);
#endif
        }

        public static void CustomEvent(string eventType, int levelIndex, params string[] parameters)
        {
            Debug.Log("Sending level custom event: " + levelIndex);
            PrintParams(parameters);
#if GAME_ANALYTICS
            GameAnalytics.NewDesignEvent(eventType, levelIndex);
#endif

            //#if ELEPHANT
            //            Params analyticParams = GetParams(parameters);
            //            Elephant.Event(eventType,levelIndex + 1, analyticParams);
            //#endif
        }

        public static void CustomEvent(string eventType, params AnalyticsEntry[] entries)
        {

            Dictionary<string,object> keyValuePairs = new Dictionary<string,object>();

            for(int i=0; i < entries.Length; i++)
            {
                keyValuePairs.Add(entries[i].key, entries[i].value);
            }
#if GAME_ANALYTICS
            //GameAnalytics.NewDesignEvent(eventType,keyValuePairs);
#endif

        }

        //#if ELEPHANT
        //        private static Params GetParams(string[] parameters)
        //        {
        //            if (parameters.Length == 0) return null;
        //            Params analyticParams = Params.New();
        //            Debug.Log("Parameters:");
        //            for (int i = 0; i < parameters.Length / 2; i++)
        //            {
        //                string key = parameters[2 * i];
        //                double value = System.Convert.ToDouble(parameters[2 * i + 1]);
        //                analyticParams.Set(key, value);
        //            }
        //            return analyticParams;
        //        }
        //#endif
        private static void PrintParams(string[] parameters)
       {
           if (parameters.Length == 0) return ;
         
           Debug.Log("Parameters:");
           for (int i = 0; i < parameters.Length / 2; i++)
           {
               string key = parameters[2 * i];
               double value = System.Convert.ToDouble(parameters[2 * i + 1]);
               Debug.Log(key + "=" + value);
           }
   
       }

        public static float GetRemoteConfigFloat(string key, float defaultValue=0)
        {
            float value = 0;
//#if ELEPHANT
//            value = RemoteConfig.GetInstance().GetFloat(key, defaultValue);
//#endif
            return value;
        }


    }
}

