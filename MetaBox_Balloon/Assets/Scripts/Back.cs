using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";
    [SerializeField] Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() => MoveScene(mainPackName));

        //=================screen setting==================

        Screen.SetResolution(1920, 1080, true);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

    }

    void MoveScene(string pakageName)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = null;

        try
        {
            intent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", pakageName);
        }
        catch (Exception ex)
        {
            Debug.LogError("exception" + ex.Message);
        }

        currentActivity.Call("startActivity", intent);

        Application.Quit();
    }
}
