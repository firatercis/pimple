using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS

using Unity.Advertisement.IosSupport;

#endif

public class IosSupportStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
        //InitIOSSupport();
#endif
    }

#if UNITY_IOS

    public void InitIOSSupport()
    {
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {

            ATTrackingStatusBinding.RequestAuthorizationTracking();

        }
    }

#endif
    // Update is called once per frame
    void Update()
    {

    }
}
