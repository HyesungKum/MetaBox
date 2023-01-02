using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    private void Awake()
    {
        backButton.onClick.AddListener( () => moveScene(mainPackName));
        backButton.onClick.AddListener( () => AppQuit());
    }
    void moveScene(string pakageName)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", pakageName);

        jo.Call("startActivity", intent);

    }

    private void AppQuit()
    {
        Application.Quit();
    }
}
