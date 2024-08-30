using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Ads
{
    public interface IAdManager
    {
        public void LoadAd(string adId);
        public void ShowAd();
        public void DestroyAd();
        //  public void LogResponseInfo();
        // TODO: REgister some event callbacks

    }

}

