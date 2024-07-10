//using Facebook.Unity;
using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
public class SKAdBindingInit : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_IOS
        if (!FB.IsInitialized)
        {
            FB.Init(FbInitCallback, (isUnityShown) => { });
        }
        else
        {
            FB.ActivateApp();
        }
#endif
    }
    private void FbInitCallback()
    {
#if UNITY_IOS
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
        FB.Mobile.SetAdvertiserTrackingEnabled(true);
        FB.Mobile.SetAutoLogAppEventsEnabled(true);
        Invoke(nameof(RegisterAppForNetworkAttribution), 1);
#endif
    }
    private void RegisterAppForNetworkAttribution()
    {
#if UNITY_IOS
        SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
#endif
    }
}