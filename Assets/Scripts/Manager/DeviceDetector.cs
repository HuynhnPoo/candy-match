using UnityEngine;
using System.Runtime.InteropServices;

public static class DeviceDetector
{
    public static bool IsMobilePlatformInWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobileDevice();
#else
        return Application.isMobilePlatform;
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool IsMobileDevice();
#endif
}
