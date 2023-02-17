using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SFX
{
    Button = 1,
    Flower,
    ReplayTouch,
    TimeLimit,
    StageClear,
    GameSuccess,
    GameFail
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
            return instance;
        }
    }
    #endregion

    [Header("Note Sound ScriptableObject")]
    [SerializeField] private MySoundIndex myNoteSound;
    [SerializeField] private MySoundIndex myMusicSound;
    [SerializeField] private MySoundIndex myBGMSound;
    [SerializeField] private MySoundIndex mySFXSound;


    [Header("Audio Sources")]
    [SerializeField] AudioSource mySFX;
    [SerializeField] AudioSource myBGM;

    [Header("Audio Volume Control")]
    [SerializeField] AudioMixer myAudioMixer;

    [Header("Button")]
    [SerializeField] GameObject touchEff;

    public bool MasterMute { get; private set; } = false;
    public bool BGMMute { get; private set; } = false;
    public bool SFXMute { get; private set; } = false;

    bool isStopped = false;
    bool isGameStart = false;
    bool isCoroutineRunning = false;

    Coroutine runningCoroutine = null;
    WaitUntil musicPlaying = null;
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
        AddButtonListener();
        BGMPlay(1);
        musicPlaying = new WaitUntil(() => myBGM.isPlaying.Equals(false));
    }




    #region Button
    public void AddButtonListener() //각 씬매니저에서 호출
    {
        GameObject[] rootObj = GetSceneRootObject();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject go = (GameObject)rootObj[i] as GameObject;
            Component[] buttons = go.transform.GetComponentsInChildren(typeof(Button), true);
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => mySFX.PlayOneShot(mySFXSound.MyClipList.myDictionary[(int)SFX.Button]));
                button.onClick.AddListener(delegate { OnClickButton(button.transform.position); });
            }
        }
    }
    GameObject[] GetSceneRootObject()
    {
        Scene curscene = SceneManager.GetActiveScene();
        return curscene.GetRootGameObjects();
    }

    void OnClickButton(Vector3 pos)
    {
        PoolCp.Inst.BringObjectCp(touchEff).transform.position = pos;
    }
    #endregion

    public void LoadMusicData(SceneMode targetMusic)
    {
        myMusicSound = Resources.Load<MySoundIndex>($"ScriptableObject/{targetMusic}");
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
        mySFX.PlayOneShot(myNoteSound.MyClipList.myDictionary[targetPitch]);
    }

    public void SFXPlay(SFX sfx)
    {
        mySFX.PlayOneShot(mySFXSound.MyClipList.myDictionary[(int)sfx]);
    }



    public void PlayStageMusic()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.MusicPlaying);

        StartCoroutine(nameof(playMyMusic));
    }

    IEnumerator playMyMusic()
    {
        isCoroutineRunning = true;

        RePlay();

        yield return musicPlaying;

        isCoroutineRunning = false;
        GameManager.Inst.UpdateCurProcess(GameStatus.MusicStop);
        if(myMusicSound.MyClipList.myDictionary.Keys.Count.Equals(GameManager.Inst.CurStage)) yield break;

        BGMPlay(2);
    }

    public void RePlay()
    {
        myBGM.clip = myMusicSound.MyClipList.myDictionary[GameManager.Inst.CurStage];
        myBGM.volume = 1;
        myBGM.Play();
    }

    public void BGMPlay(int scene) //1 - title, 2 - ingame
    {
        if (myBGM.isPlaying) myBGM.Stop();

        myBGM.clip = myBGMSound.MyClipList.myDictionary[scene];
        myBGM.volume = 0.3f;
        myBGM.Play();
    }


    public void StopMusic()
    {
        if (isCoroutineRunning) StopCoroutine(nameof(playMyMusic));
        if (myBGM.isPlaying) myBGM.Stop();
    }


}
