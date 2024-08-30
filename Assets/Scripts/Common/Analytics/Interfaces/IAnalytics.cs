using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoftwareKingdom.Analytics
{
    public interface IAnalytics
    {
        void OnLevelStarted(int levelIndex);
        void OnLevelCompleted(int levelIndex);
        void OnLevelFailed(int levelIndex);

        void OnSessionStarted();
        void OnSessionEnded();
        void OnCustomEvent(Dictionary<string, object> parameters);
    }

}

