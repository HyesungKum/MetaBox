using Kum;
using UnityEngine;
using UnityEngine.Audio;

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
    private int? lockCursor = null;

    //========================BGM Controll================================
    public void SetBGM(string sourceName)
    {
        if (soundData == null) return;
        if (!soundData.clips.TryGetValue(sourceName, out AudioClip clip)) return;

        BGMAudio.pitch = 1f;
        BGMAudio.clip = clip;
    }
    public void SetBGMLoop()
    {
        if(BGMAudio != null) BGMAudio.loop = true;
    }
    public void SetBGMUnLoop()
    {
        if (BGMAudio != null) BGMAudio.loop = false;
    }
    public void SetBGMSpeed(float speed)
    {
        if (BGMAudio != null) BGMAudio.pitch = speed;
    }
    public void PlayBGM()
    {
        if (BGMAudio != null) BGMAudio.Play();
    }
    public void StopBGM()
    {
        if (BGMAudio != null) BGMAudio.Stop();
    }

    //=======================SFX Controll=================================
    /// <summary>
    /// Call predefined Sfx
    /// </summary>
    /// <param name="sourceName">predefined Sfx Name</param>
    public void CallSfx(string sourceName)
    {
        if (soundData == null) return;
        if (!soundData.clips.TryGetValue(sourceName, out AudioClip clip)) return;
        
        //dodge lock cursor
        if(audioCursor == lockCursor) audioCursor = (audioCursor + 1) % SFXAudio.Length;

        SFXAudio[audioCursor].clip = clip; 
        SFXAudio[audioCursor].Play();

        audioCursor = (audioCursor + 1) % SFXAudio.Length;
    }
    /// <summary>
    /// call sfx using clip
    /// </summary>
    /// <param name="clip">target play sfx clip</param>
    public void CallSfx(AudioClip clip)
    {
        if (clip == null) return;

        SFXAudio[audioCursor].clip = clip;
        SFXAudio[audioCursor].Play();

        audioCursor = (audioCursor + 1) % SFXAudio.Length;
    }
    public void LoopSfx(string sourceName)
    {
        if (soundData == null) return;
        if (lockCursor != null) return;
        if (!soundData.clips.TryGetValue(sourceName, out AudioClip clip)) return;

        SFXAudio[audioCursor].clip = clip;
        SFXAudio[audioCursor].loop = true;
        SFXAudio[audioCursor].Play();

        lockCursor = audioCursor;

        audioCursor = (audioCursor + 1) % SFXAudio.Length;
    }
    public void StopLoopSfx()
    {
        if (lockCursor == null) return;
        if (soundData == null) return;

        SFXAudio[(int)lockCursor].loop = false;
        SFXAudio[(int)lockCursor].Stop();

        lockCursor = null;
    }

    //=======================Volume Controll==============================
    public void VolumeControll(string target ,float volume)
    {
        if (volume == -40) audioMixer.SetFloat(target, -80);
        else audioMixer.SetFloat(target, volume);
    }
    public float GetVolume(string target)
    {
        audioMixer.GetFloat(target, out float value);
        if (value == -80) return -40;
        else return value;
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