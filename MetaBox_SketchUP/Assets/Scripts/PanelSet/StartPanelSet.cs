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
        //gameStartBut.onClick.AddListener(() => OnClickStartBut());
        //gameStartBut.onClick.AddListener(() => OptionPanelSet.Inst.SFXSet());
        gameStartBut.onClick.AddListener(delegate { OnClickStartBut(); AudioManager.Inst.SFXSet();});
        optionBut.onClick.AddListener(delegate { OnClickOption(); AudioManager.Inst.SFXSet();});
        tutorial.onClick.AddListener(delegate { OnClickTutorial(); AudioManager.Inst.SFXSet();});
        gotoTownBut.onClick.AddListener(delegate { MoveTown(mainPackName); AudioManager.Inst.SFXSet();});
        exitBut.onClick.AddListener(delegate{ AppQuit(); AudioManager.Inst.SFXSet(); });
    }

    public void OnClickStartBut()
    {
        PanelSettingMgr.Inst.LevelPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickOption()
    {
        PanelSettingMgr.Inst.OptionPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickTutorial()
    {
        PanelSettingMgr.Inst.TutorialPanelSet(true);
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
