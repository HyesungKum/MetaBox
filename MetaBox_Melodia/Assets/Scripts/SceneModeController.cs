using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneModeController : MonoBehaviour
{
    public static DelegateAudioControl myDelegateAudioControl;


    public enum SceneMode
    {
        StartScene,
        LobbyScene,
        EasyMode,
        NormalMode,
        DifficultMode,
        ExtremeMode,

    }

    static SceneMode mySceneMode;
    static public SceneMode MySceneMode { get { return mySceneMode; } set { mySceneMode = value; } }




    [Header("For Test")]
    [SerializeField] TextMeshProUGUI myTestText;

    [Header("Panel Control")]
    [SerializeField] Canvas myCanvas;
    [SerializeField] GameObject myStartPanel;
    [SerializeField] GameObject myLobbyPanel;
    [SerializeField] GameObject myPanelOption;

    [Header("Audio Volume Control")]
    [SerializeField] Slider myAudioSliderMaster;
    [SerializeField] Slider myAudioSliderBGM;
    [SerializeField] Slider myAudioSliderSFX;


    bool isOptionPanelOpen = false;

    private void Awake()
    {
        switch (mySceneMode)
        {
            case SceneMode.StartScene:
                { isStartPanelOn(true); }
                break;

            case SceneMode.LobbyScene:
                { isStartPanelOn(false); }
                break;
        }

        Debug.Log(MySceneMode);

        myPanelOption.SetActive(false);

    }

    void CurrentVolume()
    {
        float masterVolume;
        float bGMVolume;
        float sFXVolume;

        SoundManager.Inst.MyAudioMixer.GetFloat("Master", out masterVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("BGM", out bGMVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("SFX", out sFXVolume);

        myAudioSliderMaster.value = masterVolume;
        myAudioSliderBGM.value = bGMVolume;
        myAudioSliderSFX.value = sFXVolume;
    }




    public void isStartPanelOn(bool result)
    {
        myStartPanel.SetActive(result);
        myLobbyPanel.SetActive(!result);
    }



    // Option panel ==========================================================
    public void OnClickOption()
    {
        CurrentVolume();

        if (isOptionPanelOpen == false)
        {
            myPanelOption.SetActive(true);

            isOptionPanelOpen = true;
            return;
        }

        myPanelOption.SetActive(false);
        isOptionPanelOpen = false;
    }


    public void MasterAudioControl()
    {
        float volume = myAudioSliderMaster.value;

        myDelegateAudioControl("Master", volume);
    }

    public void BGMAudioControl()
    {
        float volume = myAudioSliderBGM.value;

        myDelegateAudioControl("BGM", volume);
    }

    public void SFXAudioControl()
    {
        float volume = myAudioSliderSFX.value;

        myDelegateAudioControl("SFX", volume);
    }
    // Option panel ==========================================================


    // Start Game ============================================================
    public void OnClickReturnToTown()
    {
        myTestText.text = "마을로 갈꺼야";
    }

    public void OnClickPlay()
    {
        myStartPanel.SetActive(false);
        myLobbyPanel.SetActive(true);
    }

    public void OnClickRank()
    {
        myTestText.text = "랭킹을 보여줘";
    }

    public void OnClickQuit()
    {
        // cloase app 

        myTestText.text = "그만할꺼야";
    }
    // Start ================================================================


    // Lobby ============================================================
    public void OnClickBack()
    {
        myStartPanel.SetActive(true);
        myLobbyPanel.SetActive(false);
    }

    public void OnClickEasy()
    {
        SceneManager.LoadScene("MelodiaEasyMode");

        mySceneMode = SceneMode.EasyMode;
        myCanvas.enabled = false;
    }

    public void OnClickNormal()
    {
        myTestText.text = "보통 난이도";

        mySceneMode = SceneMode.NormalMode;
        //myCanvas.enabled = false;

    }

    public void OnClickDifficult()
    {
        myTestText.text = "난감한 난이도";


        mySceneMode = SceneMode.DifficultMode;
        //myCanvas.enabled = false;

    }

    public void OnClickExtreme()
    {
        myTestText.text = "매우 난감한 난이도";


        mySceneMode = SceneMode.ExtremeMode;
        //myCanvas.enabled = false;

    }
    // Lobby ============================================================

}
