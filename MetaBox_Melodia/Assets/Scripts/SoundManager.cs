
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    private NoteSound myNoteSound;
    public NoteSound MyNoteSound { set { myNoteSound = value; } }


    [Header("Audio Sources")]
    [SerializeField] AudioSource myNoteAudioSource;
    [SerializeField] AudioSource myBGMAudioSource;


    [Header("Audio Volume Control")]
    [SerializeField] AudioMixer myAudioMixer;
    public AudioMixer MyAudioMixer { get { return myAudioMixer; } }

    // replay related 
    [SerializeField] Button myButtonReplay;
    float easyModeCoolTime = 15;





    private void Awake()
    {
        //==============================================
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance.gameObject);
        //==============================================

        //myButtonReplay.interactable = true;

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




    public void PlayNote(PitchName targetPitch)
    {
        AudioClip changeClip;

        // change audio clip as targeted pitch note 
        changeClip = myNoteSound.MyPitchClips.myDictionary[targetPitch];


        myNoteAudioSource.clip = changeClip;

        myNoteAudioSource.Play();
    }



    public void PlayBGM(string target)
    {
        //AudioClip changeClip;

        // change audio clip as targeted pitch note 


        //myBGMAudioSource.clip = changeClip;

        myBGMAudioSource.Play();

        Debug.Log($"나는 {target} 야~! ");
    }


    public void ReplayMusic()
    {
        Debug.Log("음악을 다시 틀어줘~");
    }





}
