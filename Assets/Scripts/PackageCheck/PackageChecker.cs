using UnityEngine;

public class PackageChecker
{
    #if UNITY_ANDROID && !UNITY_EDITOR
    public static bool IsAppInstalled(string bundleID)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID);

        return launchIntent == null ? false : true;
    }
    #endif
}