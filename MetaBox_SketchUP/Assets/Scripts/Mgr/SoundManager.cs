using System.Collections.Generic;
using System.Linq;
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

    [Header("[Audio Sources]")]
    [SerializeField] AudioSource BGMAudioSurce = null;
    [SerializeField] public AudioSource SFXSource = null;
    [SerializeField] public AudioClip butSourceClip = null;
    [SerializeField] public AudioClip clearSFXClip = null;
    [SerializeField] public AudioClip failSFXClip = null;
    //[SerializeField] public AudioClip playNodeClearClip = null;
    //[SerializeField] public AudioClip colorPanelSFXClip = null;

    [Header("[Audio Volume Control]")]
    [SerializeField] AudioMixer myAudioMixer = null;

    public AnimalAudioData animalAudioData = null;
    private AudioClip animalAudioPlayClip = null;

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

        if (animalAudioData == null)
            animalAudioData = Resources.Load<AnimalAudioData>("Data/AnimalAudioData");

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

    void Start()
    {
        //AnimalAudioPlay(1);
    }

    public void BGMControl(Slider BGMSlider)
    {
        BGMValue = BGMSlider.value;

        if (BGMValue == -40f)
            MyAudioMixer.SetFloat("BGM", -80);
        else
            MyAudioMixer.SetFloat("BGM", BGMValue);
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

    public void ButtonSFXPlay()
    {
        SFXSource.clip = butSourceClip;
        SFXSource.Play();
    }

    public void ClearSFXPlay()
    {
        SFXSource.clip = clearSFXClip;
        SFXSource.Play();
    }

    public void FailSFXPlay()
    {
        SFXSource.clip = failSFXClip;
        SFXSource.Play();
    }

    public void ToggleAduioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void AnimalAudioPlay(int index)
    {
        //animalAudioDictionary.ContainsKey(index);
        animalAudioDictionary.TryGetValue(index, out animalAudioPlayClip);

        Debug.Log("## check " + animalAudioPlayClip);
        SFXSource.clip = animalAudioPlayClip;
        Debug.Log("SFXSource.clip : " + SFXSource.clip);
        SFXSource.Play();

        //    animalAudioPlayClip = animalAudioData.PolarbearAudioClip;
        //    Debug.Log("name Check : " + animalAudioPlayClip.name);
        //    //animalAudioPlayClip.name = animalWord;
        //    animalAudioList.Contains(animalAudioPlayClip);
        //    Debug.Log("## 찾았는지? : " + animalAudioList.Contains(animalAudioPlayClip));
        //    Debug.Log("## 오디오 찾기 : " + animalAudioPlayClip.name);

        //    SFXSource.clip = animalAudioPlayClip;
    }
}