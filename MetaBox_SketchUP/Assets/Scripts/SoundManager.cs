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

    [Header("[Audio Volume Control]")]
    [SerializeField] AudioMixer myAudioMixer = null;
    public AudioMixer MyAudioMixer { get { return myAudioMixer; } }


    private float bGMValue;
    public float BGMValue
    { get { return bGMValue; } set { bGMValue = value; } }

    private float sfxValue;
    public float SFXValue
    { get { return sfxValue; } set { sfxValue = value; } }

    void Awake()
    {
        if (instatce == null)
            instatce = this;
        else if (instatce != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instatce.gameObject);

        BGMAudioSurce.playOnAwake = true;
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

    public void ToggleAduioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
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
}
