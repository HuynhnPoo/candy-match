using UnityEngine;
using Unity.Services.LevelPlay;

public class AdsManager : MonoBehaviour
{
    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;

    bool isAdsEnable = false;
    void Start()
    {
        LevelPlay.Init("2467bd4cd");
        LevelPlay.ValidateIntegration();

        
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SkdInitializationFailedEvent;

        interstitialAd?.LoadAd();
        rewardedAd?.LoadAd();

        Debug.Log("khoi tạo thanh cong");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
           // ShowBanner();
           ShowRewardedVideo();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowInterstitial();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowBanner();
        }
    }
    void SetupAds()
    {
        LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        rewardedAd = new LevelPlayRewardedAd("6nje9pd3b82wc2fi");
        rewardedAd.OnAdLoaded += (adInfo) => Debug.Log("video có thưởng load xong");
        rewardedAd.OnAdRewarded += (adInfo, reward) =>
        {
            Debug.Log($"nhận thương thành công tên :{reward.Name}, giá trị :{reward.Amount}");
        };

        //
        interstitialAd = new LevelPlayInterstitialAd("pb8nxqgf7y542l1j");
        interstitialAd.OnAdLoaded += (adInfo) => Debug.Log("interstitial đã load xong ");
        interstitialAd.OnAdClosed += (adInfo) => Debug.Log("đóng quảng cáo");

        //
        bannerAd = new LevelPlayBannerAd("fnk61wotqhzhe06b");
        bannerAd.OnAdLoaded += (adInfo) => Debug.Log("banner đã load xong");
    }
    public void SdkInitializationCompletedEvent(LevelPlayConfiguration conifg)
    {

        Debug.Log($"khởi tao thành công {conifg}");
        SetupAds();
        isAdsEnable = true;
    }

    public void SkdInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogWarning($"khởi tao không thành công {error}");
    }

    public void ShowRewardedVideo()
    {
        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            rewardedAd?.ShowAd();
        }
        else { rewardedAd?.LoadAd(); }
    }

    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.IsAdReady())
        {
            interstitialAd?.ShowAd();
        }
        else { interstitialAd?.LoadAd(); }

    }

    public void ShowBanner()
    {
        bannerAd.LoadAd();
    }
    public void HideBanner()
    {
        bannerAd.HideAd();
    }

    void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
    {
        Debug.Log("hien thi tát cả các data "+impressionData.AllData);
    }


}

