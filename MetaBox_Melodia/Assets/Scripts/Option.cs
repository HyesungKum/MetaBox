using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public static DelegateAudioControl myDelegateAudioControl;


    [Header("Audio Volume Control")]
    [SerializeField] Slider myAudioSliderMaster;
    [SerializeField] Slider myAudioSliderBGM;
    [SerializeField] Slider myAudioSliderSFX;

    void OnEnable()
    {
        CurrentVolume();
    }

    public void OnClickOption()
    {
        this.gameObject.SetActive(false);
    }

    // keep volume values as scene changes
    void CurrentVolume()
    {
        SoundManager.Inst.MyAudioMixer.GetFloat("Master", out float masterVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("BGM", out float bGMVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("SFX", out float sFXVolume);

        myAudioSliderMaster.value = masterVolume;
        myAudioSliderBGM.value = bGMVolume;
        myAudioSliderSFX.value = sFXVolume;
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
}
