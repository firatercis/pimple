using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardedAdManager 
{
    public void LoadAd(string adId);
    public void ShowAd();
    public void DestroyAd();
  //  public void LogResponseInfo();
    // TODO: REgister some event callbacks

}
