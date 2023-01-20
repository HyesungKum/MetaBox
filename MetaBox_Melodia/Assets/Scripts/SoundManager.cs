using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public enum PitchName
{
    LoDo,
    LoRe,
    LoMi,
    LoFa,
    LoSol,
    LoRa,
    LoSi,

    HiDo,
    HiRe,
    HiMi,
    HiFa,
    HiSol,
    HiRa,
}


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
    private NoteSoundIndex myNoteSound;
    public NoteSoundIndex MyNoteSound { set { myNoteSound = value; } }





    [Header("Audio Sources")]
    [SerializeField] AudioSource myNoteAudioSource;
    [SerializeField] AudioSource myBGMAudioSource;
    [SerializeField] AudioClip tempClip;




    [Header("Audio Volume Control")]
    [SerializeField] AudioMixer myAudioMixer;
    public AudioMixer MyAudioMixer { get { return myAudioMixer; } }


    [SerializeField] bool isStopped;
    public bool IsStopped { get { return isStopped; } set { isStopped = value; } }

    Coroutine curCoroutine;
    public Coroutine CurCoroutine { get { return curCoroutine; } set { curCoroutine = value; } }

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


    public void PlayNote(int targetPitch, float pitch)
    {
        AudioClip changeClip;

        // change audio clip as targeted pitch note 
        changeClip = myNoteSound.MyPitchClips.myDictionary[targetPitch];

        myNoteAudioSource.pitch = pitch;

        myNoteAudioSource.clip = changeClip;

        myNoteAudioSource.Play();
    }


    public void PlayBGM(string target)
    {
        //AudioClip changeClip;

        // change audio clip as targeted pitch note 

        //myBGMAudioSource.clip = changeClip;

        myBGMAudioSource.Play();

        Debug.Log($"³ª´Â {target} ¾ß~! ");
    }


    // play music before start game 
    public void FirstPlay(List<int> TargetNotes)
    {
        isStopped = false;
        clipList = TargetNotes;

        PlayStageMusic();
    }


    public void PlayStageMusic()
    {
        CurCoroutine = StartCoroutine("playMusic");

    }


    IEnumerator playMusic()
    {
        foreach (int targetNote in clipList)
        {
            if (isStopped)
                break;


            PlayNote(targetNote, 1);

            Debug.Log("³ë·¡ Æ²¾î! " + targetNote);
            Debug.Log("¸ØÃã? " + isStopped);

            yield return new WaitUntil(() => myNoteAudioSource.isPlaying == false);
        }


        Debug.Log("¸ØÃç" + isStopped);

        if (!IsStopped)
        {
            GameManager.myDelegateGameStatus(GameStatus.StartGame);
        }

        // for test =============================================================

        //PlayNote(clipList[0], 1);

        //Debug.Log("³ë·¡ Æ²¾î! " + clipList[0]);

        //yield return new WaitUntil(() => myNoteAudioSource.isPlaying == false);


        // for test =============================================================
    }



    public void StopMusic()
    {
        Debug.Log("¸ØÃç!");

        if (CurCoroutine != null)
            StopCoroutine(CurCoroutine);

    }

}
