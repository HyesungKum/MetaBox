using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class SoundManager : MonoSingleTon<SoundManager>
{
    //=========================================clip data=========================================
    [Header("Reference Data")]
    [SerializeField] private SoundData soundData;

    //========================================audio mixer========================================
    [Header("Master Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    //=========================================audio source======================================
    [Header("Audio Out")]
    [SerializeField] private AudioSource BGMAudio;
    
    [SerializeField] private AudioSource SFX1;
    [SerializeField] private AudioSource SFX2;
    [SerializeField] private AudioSource SFX3;

    //========================BGM Controll===============================
    public void SetBGM(string sourceName)
    {
        soundData.clips.TryGetValue(sourceName, out AudioClip clip);
        BGMAudio.clip = clip;
    }
    public void PlayBGM() => BGMAudio.Play();
    public void StopBGM() => BGMAudio.Stop();

    //=======================SFX Controll================================
    public void CallSound(string sourceName)
    {
        soundData.clips.TryGetValue(sourceName, out AudioClip clip);

        if (!SFX1.isPlaying)
        {
            SFX1.clip = clip;
            SFX1.Play();
        }
        else if (!SFX2.isPlaying)
        {
            SFX1.clip = clip;
            SFX1.Play();
        }
        else if (!SFX3.isPlaying)
        {
            SFX1.clip = clip;
            SFX1.Play();
        }
        else
        {
            SFX1.clip = clip;
            SFX1.Play();
        }
    }

    //=======================Volume Controll==============================
    public void VolumeControll(string target ,float volume)
    {

        audioMixer.SetFloat(target, volume);
    }

    /// <summary>
    /// toggle sound (on - off)
    /// </summary>
    /// <param name="target">target audio mixer parameter</param>
    /// <param name="volume">setting value -40 ~ 0 </param>
    public void ToggleControll(string target, float value)
    {
        audioMixer.GetFloat(target, out float volume);

        volume = volume == -80? value : -80;

        audioMixer.SetFloat(target, volume);   
    }
}