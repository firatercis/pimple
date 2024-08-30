using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Common.InterfaceBus;
using Firebase.Analytics;




namespace SoftwareKingdom.Analytics.FirebaseConnectors
{
    public class FirebaseAnalyticsController : MonoBehaviour, IAnalytics
    {

        public Firebase.FirebaseApp app = null;

       

        void Awake() {
            InterfaceBus<IAnalytics>.Register(this);
        }

        public void OnCustomEvent(Dictionary<string, object> parameters) {

        }

        public void OnLevelCompleted(int levelIndex) {

            Firebase.Analytics.FirebaseAnalytics
      .LogEvent(
        Firebase.Analytics.FirebaseAnalytics.EventLevelEnd,
        Firebase.Analytics.FirebaseAnalytics.ParameterScore,
        levelIndex
  );


        }

        public void OnLevelFailed(int levelIndex) {

        }

        public void OnLevelStarted(int levelIndex) {

        }

        public void OnSessionEnded() {

        }

        public void OnSessionStarted() {

        }



        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}

