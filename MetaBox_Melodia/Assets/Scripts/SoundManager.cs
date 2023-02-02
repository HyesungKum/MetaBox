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

            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }
    #endregion

    [Header("Note Sound ScriptableObject")]
    [SerializeField]
    private MySoundIndex myNoteSound;

    [SerializeField]
    private MySoundIndex myMusicSound;




    [Header("Audio Sources")]
    [SerializeField] AudioSource myNoteAudioSource;
    [SerializeField] AudioSource myBGMAudioSource;
    [SerializeField] AudioClip tempClip;




    [Header("Audio Volume Control")]
    [SerializeField] AudioMixer myAudioMixer;
    public AudioMixer MyAudioMixer { get { return myAudioMixer; } }


    bool isStopped = false;
    bool isGameStart = false;
    bool isCoroutineRunning = false;

    IEnumerator curCoroutine = null;

    List<int> clipList = new();


    private void Awake()
    {
        //==============================================
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance.gameObject);
        //==============================================

        myBGMAudioSource.clip = tempClip;

        SceneModeController.myDelegateAudioControl = AudioVolumeControl;
        UiManager.myDelegateAudioControl = AudioVolumeControl;
    }



    void AudioVolumeControl(string target, float volume)
    {
        if (volume == -40f)
            myAudioMixer.SetFloat(target, -80f);

        else
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
