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
    [SerializeField] Button exitBut = null;
    [SerializeField] Button tutorial = null;

    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    void Awake()
    {
        gameStartBut.onClick.AddListener(delegate { OnClickStartBut(); SoundManager.Inst.ButtonSFXPlay(); }); 
        optionBut.onClick.AddListener(delegate { OnClickOption(); SoundManager.Inst.ButtonSFXPlay(); }); 
        tutorial.onClick.AddListener(delegate { OnClickTutorial(); SoundManager.Inst.ButtonSFXPlay(); });
        gotoTownBut.onClick.AddListener(delegate { MoveTown(mainPackName); SoundManager.Inst.ButtonSFXPlay(); }); 
        exitBut.onClick.AddListener(delegate{ OnClickQuitBut(); SoundManager.Inst.ButtonSFXPlay(); });
    }

    public void OnClickStartBut()
    {
        StartSceneManager.Inst.LevelPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickOption()
    {
        StartSceneManager.Inst.OptionPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickTutorial()
    {
        StartSceneManager.Inst.TutorialPanelSet(true);
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

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.QuitPanelSet(true);
        this.gameObject.SetActive(false);
    }


}
