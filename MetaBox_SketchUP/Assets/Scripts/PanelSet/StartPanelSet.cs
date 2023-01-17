using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelSet : MonoBehaviour
{
    [Header("[Start Panel]")]
    [SerializeField] Button gameStartBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button gotoTownBut = null;
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";
    [SerializeField] Button ExitBut = null;

    [SerializeField] GameObject nextPanel = null;
    [SerializeField] GameObject optionPanel = null;

    void Awake()
    {
        gameStartBut.onClick.AddListener(() => OnClickStartBut());
        gotoTownBut.onClick.AddListener(() => MoveTown(mainPackName));
        optionBut.onClick.AddListener(() => OnClickOption());
        ExitBut.onClick.AddListener(() => AppQuit());
    }

    public void OnClickStartBut()
    {
        nextPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void OnClickOption()
    {
        optionPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void MoveTown(string pakageName)
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
