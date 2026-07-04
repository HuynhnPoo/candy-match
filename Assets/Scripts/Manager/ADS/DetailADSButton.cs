using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeAds
{
    Banner,
    Interstitial,
    RewardedVideo
}
public class DetailADSButton : ButtonBase
{
    public TypeAds TypeAds =TypeAds.Banner;
    public override void OnClick()
    {
        switch (TypeAds) 
        {
            case TypeAds.Banner:
                LevelPlaySample.Instance.LoadBannerAD();
                //LevelPlaySample.Instance.ShowBannerAd();
                break;

                case TypeAds.Interstitial:
                LevelPlaySample.Instance.LoadIntersitialAD();
                LevelPlaySample.Instance.ShowInterstitialAd();
                break;
                case TypeAds.RewardedVideo:
                LevelPlaySample.Instance.LoadRewardedAD();
                LevelPlaySample.Instance.ShowRewardedAd();
                break;
                default:
                Debug.LogWarning("không vó các ddieuf khiện trong đó");
                break;
        }
    }

   
}
