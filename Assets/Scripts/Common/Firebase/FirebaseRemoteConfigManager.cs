
using SoftwareKingdom.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Crashlytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using SoftwareKingdom.Common;

namespace SoftwareKingdom.RemoteConfig.FirebaseConnectors
{
    public class FirebaseRemoteConfigManager : MonoBehaviour, IRemoteConfig
    {


        public Firebase.FirebaseApp app = null;

        void Start() {
            InitializeFirebaseAndStartGame();
        }


        // Begins the firebase initialization process and afterwards, opens the main menu.
        private void InitializeFirebaseAndStartGame() {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(
               previousTask =>
               {
                   var dependencyStatus = previousTask.Result;
                   if (dependencyStatus == Firebase.DependencyStatus.Available)
                   {
                       // Create and hold a reference to your FirebaseApp,
                       app = Firebase.FirebaseApp.DefaultInstance;
                       // Set the recommended Crashlytics uncaught exception behavior.
                       Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                       SetRemoteConfigDefaults();
                   }
                   else
                   {
                       UnityEngine.Debug.LogError(
                 $"Could not resolve all Firebase dependencies: {dependencyStatus}\n" +
                 "Firebase Unity SDK is not safe to use here");
                   }
               });
        }

        private void SetRemoteConfigDefaults() {
            var defaults = new Dictionary<string, object>();
            //defaults.Add(
            //   Hamster.MapObjects.AccelerationTile.AccelerationTileForceKey,
            //   Hamster.MapObjects.AccelerationTile.AccelerationTileForceDefault);
            //defaults.Add(
            //   Hamster.States.MainMenu.SubtitleOverrideKey,
            //   Hamster.States.MainMenu.SubtitleOverrideDefault);
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(
               previousTask =>
               {

                   // default remote config degerleri ayarlanmasi.
               }
            );
        }



        public object GetValue(string parameterName) {

            //int result = FirebaseRemoteConfig.GetValue("MaxCountShowIntersitialAds").LongValue)
            return null;
        }

        public object LoadParameter(string parameterName) {
            return null;
        }

      
        // Update is called once per frame
        void Update() {

        }

        public void InitGameParameters(GameParameters parameters) {
            throw new System.NotImplementedException();
        }

        void ActivateValuesOnConfigUpdate(object sender, ConfigUpdateEventArgs args) {
            if (args.Error != RemoteConfigError.None)
            {
                Debug.Log($"Error occurred while listening: {args.Error}");
                return;
            }

            Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));
            // Activate all fetched values and then logs.
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.ActivateAsync().ContinueWithOnMainThread(
               task => {
                   Debug.Log($"Keys from {nameof(ActivateValuesOnConfigUpdate)} activated.");
               });
        }
    }
}

