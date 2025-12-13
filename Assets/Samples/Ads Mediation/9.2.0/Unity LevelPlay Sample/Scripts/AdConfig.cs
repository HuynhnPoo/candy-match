public static class AdConfig
{
    public static string AppKey => GetAppKey();
    public static string BannerAdUnitId => GetBannerAdUnitId();
    public static string InterstitalAdUnitId => GetInterstitialAdUnitId();
    public static string RewardedVideoAdUnitId => GetRewardedVideoAdUnitId();

    static string GetAppKey()
    {
        #if UNITY_ANDROID
           // return "85460dcd";
            return "2467bd4cd";
        #elif UNITY_IPHONE
            return "8545d445";
        #else
            return "unexpected_platform";
        #endif
    }

    static string GetBannerAdUnitId()
    {
        #if UNITY_ANDROID
           // return "thnfvcsog13bhn08";
            return "fnk61wotqhzhe06b";
      
#elif UNITY_IPHONE
            return "iep3rxsyp9na3rw8";
#else
            return "unexpected_platform";
#endif
    }
    static string GetInterstitialAdUnitId()
    {
        #if UNITY_ANDROID
          //  return "aeyqi3vqlv6o8sh9";
            return "pb8nxqgf7y542l1j";
      
#elif UNITY_IPHONE
            return "wmgt0712uuux8ju4";
#else
            return "unexpected_platform";
#endif
    }

    static string GetRewardedVideoAdUnitId()
    {
        #if UNITY_ANDROID
          //  return "76yy3nay3ceui2a3";
            return "6nje9pd3b82wc2fi";
#elif UNITY_IPHONE
            return "qwouvdrkuwivay5q";
#else
            return "unexpected_platform";
#endif
    }
}
