using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public delegate void DelegateAudioControl(string target, float volume);

public class SoundManager : MonoBehaviour
{
    #region Singleton

    private static SoundManager instance = null;
    public static SoundManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    instance = new GameObject("SoundManager", typeof(SoundManager)).GetComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("Note Sound ScriptableObject")]
    [SerializeField] private MySoundIndex myNoteSound;
    [SerializeField] private MySoundIndex myMusicSound;

    [Header("Audio Sources")]
    [SerializeField] AudioSource myNoteAudioSource;
    [SerializeField] AudioSource myBGMAudioSource;

    [Header("Audio Volume Control")]
    [SerializeField] AudioMixer myAudioMixer;

    public bool MasterMute { get; private set; } = false;
    public bool BGMMute { get; private set; } = false;
    public bool SFXMute { get; private set; } = false;

    bool isStopped = false;
    bool isGameStart = false;
    bool isCoroutineRunning = false;

    IEnumerator curCoroutine = null;

    List<int> clipList = new();


    void Awake()
    {
        //==============================================
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);
        //==============================================

        Option.myDelegateAudioControl = AudioVolumeControl;
        Option.myDelegateAudioMute = AudioMute;
    }

    public float GetVolume(string target)
    {
        myAudioMixer.GetFloat(target, out float volume);
        return volume;
    }

    void AudioVolumeControl(string target, float volume)
    {
        if (volume == -40f) myAudioMixer.SetFloat(target, -80f);
        else myAudioMixer.SetFloat(target, volume);
    }

    void AudioMute(string target, float value)
    {
        myAudioMixer.GetFloat(target, out float volume);
        volume = volume == -80 ? value : -80; 
        if (target.Equals("Master")) MasterMute = volume == -80 ? true : false;
        else if (target.Equals("BGM")) BGMMute = volume == -80 ? true : false;
        else SFXMute = volume == -80 ? true : false;

        myAudioMixer.SetFloat(target, volume);
    }

    

    // play note sound 
    public void PlayNote(int targetPitch, float pitch)
    {

        AudioClip changeClip;

        // change audio clip as targeted pitch note 
        changeClip = myNoteSound.MyClipList.myDictionary[targetPitch];

        myNoteAudioSource.pitch = pitch;

        myNoteAudioSource.clip = changeClip;

        myNoteAudioSource.Play();
    }


    // play music 
    public void SetStageMusic(int targetMusic, float pitch)
    {
        AudioClip changeClip;

        // change audio clip 
        changeClip = myMusicSound.MyClipList.myDictionary[targetMusic];

        myBGMAudioSource.pitch = pitch;

        myBGMAudioSource.clip = changeClip;

    }


    public void PlayStageMusic()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.MusicPlaying);

        curCoroutine = playMyMusic();

        StartCoroutine(curCoroutine);
    }


    IEnumerator playMyMusic()
    {
        isCoroutineRunning = true;

        myBGMAudioSource.Play();


        yield return new WaitUntil(() => myBGMAudioSource.isPlaying == false);



        isCoroutineRunning = false;
        GameManager.Inst.UpdateCurProcess(GameStatus.MusicStop);
    }



    public void StopMusic()
    {
        if (isCoroutineRunning)
        {
            myBGMAudioSource.Stop();
            StopCoroutine(curCoroutine);
        }
    }


}
