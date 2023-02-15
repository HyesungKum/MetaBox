using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager instatce;
    public static SoundManager Inst
    {
        get
        {
            if (instatce == null)
            {
                instatce = FindObjectOfType<SoundManager>();
                if (instatce == null)
                {
                    instatce = new GameObject(nameof(SoundManager), typeof(SoundManager)).GetComponent<SoundManager>();
                }
            }
            return instatce;
        }
    }
    #endregion

    [Header("[Audio BGM]")]
    [SerializeField] AudioSource BGMAudioSurce = null;
    [SerializeField] public AudioClip titleAudioClip = null;
    [SerializeField] public AudioClip inGameAudioClip = null;

    [Header("[Audio SFX]")]
    [SerializeField] public AudioSource SFXSource = null;
    [SerializeField] public AudioClip butSourceClip = null;

    [Header("[Audio Volume Control]")]
    [SerializeField] AudioMixer myAudioMixer = null;

    [Header("[Audio Data]")]
    public AnimalAudioData animalAudioData = null;
    public SFXData sfxData = null;
    private AudioClip animalAudioPlayClip = null;

    [Header("[Touch Effect]")]
    [SerializeField] GameObject butClickEffect = null;
    GameObject InstButEffect = null;

    [SerializeField] private int levelIndex;
    public int LevelIndex { get { return levelIndex; } set { levelIndex = value; } }

    private float bGMValue;
    private float sfxValue;

    public AudioMixer MyAudioMixer { get { return myAudioMixer; } }
    public float BGMValue { get { return bGMValue; } set { bGMValue = value; } }
    public float SFXValue { get { return sfxValue; } set { sfxValue = value; } }

    Dictionary<int, AudioClip> animalAudioDictionary;

    void Awake()
    {
        if (instatce == null)
            instatce = this;
        else if (instatce != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instatce.gameObject);

        BGMAudioSurce.playOnAwake = true;
        BGMAudioSurce.loop = true;

        TitleBGMPlay();

        if (animalAudioData == null)
            animalAudioData = Resources.Load<AnimalAudioData>("Data/AnimalAudioData");

        if (sfxData == null)
            sfxData = Resources.Load<SFXData>("Data/SFXData");

        animalAudioDictionary = new Dictionary<int, AudioClip>();

        #region Dictionary Add
        animalAudioDictionary.Add(1, animalAudioData.PolarbearAudioClip);
        animalAudioDictionary.Add(2, animalAudioData.ReindeerAudioClip);
        animalAudioDictionary.Add(3, animalAudioData.PenguinAudioClip);

        animalAudioDictionary.Add(4, animalAudioData.OrcaAudioClip);
        animalAudioDictionary.Add(5, animalAudioData.WalrusAudioClip);
        animalAudioDictionary.Add(6, animalAudioData.DolphinAudioClip);

        animalAudioDictionary.Add(7, animalAudioData.GiraffeAudioClip);
        animalAudioDictionary.Add(8, animalAudioData.ElephantAudioClip);
        animalAudioDictionary.Add(9, animalAudioData.CheetahAudioClip);

        animalAudioDictionary.Add(10, animalAudioData.TigerAudioClip);
        animalAudioDictionary.Add(11, animalAudioData.DeerAudioClip);
        animalAudioDictionary.Add(12, animalAudioData.RabbitAudioClip);
        #endregion
    }

    public void BGMControl(Slider BGMSlider)
    {
        BGMValue = BGMSlider.value;

        if (BGMValue == -40f)
            MyAudioMixer.SetFloat("BGM", -80);
        else
            MyAudioMixer.SetFloat("BGM", BGMValue);

        Debug.Log("BGMValue : " + BGMValue);
    }

    public void SFXControl(Slider ButSfxSlider)
    {
        SFXSource.clip = butSourceClip;
        SFXSource.Play();

        SFXValue = ButSfxSlider.value;
        if (SFXValue == -40f)
            MyAudioMixer.SetFloat("SFX", -80);
        else
            MyAudioMixer.SetFloat("SFX", SFXValue);
    }

    public void ButtonEffect(Vector3 instTrsnsform)
    {
        InstButEffect = ObjectPoolCP.PoolCp.Inst.BringObjectCp(butClickEffect);
        InstButEffect.transform.position = instTrsnsform;
    }

    public void BGMPlayStop()
    {
        BGMAudioSurce.Stop();
    }

    public void BGMValueDown()
    {
        if (BGMValue == -40f)
            MyAudioMixer.SetFloat("BGM", -80);
        else
            MyAudioMixer.SetFloat("BGM", -35);
    }

    public void TitleBGMPlay()
    {
        BGMAudioSurce.clip = titleAudioClip;
        MyAudioMixer.SetFloat("BGM", 0);
        BGMAudioSurce.Play();
    }

    public void InGameBGMPlay()
    {
        BGMAudioSurce.clip = inGameAudioClip;
        MyAudioMixer.SetFloat("BGM", 0);
        BGMAudioSurce.Play();
    }

    public void ButtonSFXPlay()
    {
        MyAudioMixer.SetFloat("BGM", -10);
        SFXSource.clip = butSourceClip;
        SFXSource.Play();
    }

    public void AlarmClockSFXPlay()
    {
        SFXSource.clip = sfxData.AlarmClock;
        SFXSource.Play();
    }

    public void StageClearSFXPlay()
    {
        SFXSource.clip = sfxData.StageClear;
        SFXSource.Play();
    }

    public void GameClearSFXPlay()
    {
        SFXSource.clip = sfxData.GameClear;
        SFXSource.Play();
    }

    public void GameLoseSFXPlay()
    {
        SFXSource.clip = sfxData.GameLose;
        SFXSource.Play();
    }

    public void ChangeLineAndColorSFXPlay()
    {
        SFXSource.clip = sfxData.ChangeLineAndColor;
        SFXSource.Play();
    }

    public void ConnectLineSFXPlay()
    {
        SFXSource.clip = sfxData.ConnectLine;
        SFXSource.Play();
    }

    public void ToggleAduioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void AnimalAudioPlay(int index)
    {
        animalAudioDictionary.TryGetValue(index, out animalAudioPlayClip);
        SFXSource.clip = animalAudioPlayClip;
        MyAudioMixer.SetFloat("SFX", 0);
        SFXSource.Play();
    }


    public void SelectPanelNoNameSFXPlay()
    {
        SFXSource.clip = sfxData.noName;
        SFXSource.Play();
    }
}