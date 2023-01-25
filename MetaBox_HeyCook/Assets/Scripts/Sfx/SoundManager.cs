using UnityEngine;
using UnityEngine.Audio;
using Kum;

public class SoundManager : MonoSingleTon<SoundManager>
{
    //=========================================clip data========================================
    [Header("Reference Data")]
    [SerializeField] private SoundData soundData;

    //========================================audio mixer=======================================
    [Header("Master Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    //=========================================audio source=====================================
    [Header("Audio Out")]
    [SerializeField] private AudioSource BGMAudio;
    
    [SerializeField] private AudioSource[] SFXAudio = new AudioSource[3];

    //======================================inner variables=====================================
    private int audioCursor = 0;

    //========================BGM Controll================================
    public void SetBGM(string sourceName)
    {
        soundData.clips.TryGetValue(sourceName, out AudioClip clip);
        BGMAudio.pitch = 1f;
        BGMAudio.clip = clip;
    }
    public void SetBGMLoop() => BGMAudio.loop = true;
    public void SetBGMUnLoop() => BGMAudio.loop = false;
    public void SetBGMSpeed(float speed) => BGMAudio.pitch = speed;
    public void PlayBGM() => BGMAudio.Play();
    public void StopBGM() => BGMAudio.Stop();

    //=======================SFX Controll=================================
    public void CallSfx(string sourceName)
    {
        soundData.clips.TryGetValue(sourceName, out AudioClip clip);

        SFXAudio[audioCursor].clip = clip; 
        SFXAudio[audioCursor].Play();

        audioCursor = (audioCursor + 1) % SFXAudio.Length;
    }

    //=======================Volume Controll==============================
    public void VolumeControll(string target ,float volume)
    {
        audioMixer.SetFloat(target, volume);
    }
    public float GetVolume(string target)
    {
        audioMixer.GetFloat(target, out float value);
        return value;
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