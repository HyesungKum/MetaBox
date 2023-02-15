using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public static DelegateAudioControl myDelegateAudioControl;

    [Header("Option Panel Control")]
    [SerializeField] Button exit = null;
    [SerializeField] Button home = null;

    [Header("Audio Volume Control")]
    [SerializeField] Slider myAudioSliderBGM;
    [SerializeField] Slider myAudioSliderSFX;

    void Awake()
    {
        exit.onClick.AddListener(OnClick_Close);
        if (home != null) home.onClick.AddListener(OnClick_Quit);

        myAudioSliderBGM.onValueChanged.AddListener(delegate { OnValueChanged_BGM(); });
        myAudioSliderSFX.onValueChanged.AddListener(delegate { OnValueChanged_SFX(); });
    }

    void OnEnable()
    {
        myAudioSliderBGM.value = SoundManager.Inst.GetVolume("BGM");
        myAudioSliderSFX.value = SoundManager.Inst.GetVolume("SFX");
    }

    void OnClick_Close()
    {
        if (home != null) GameManager.Inst.UpdateCurProcess(GameStatus.GamePlaying);
        this.gameObject.SetActive(false);
    }

    void OnClick_ReStart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        this.gameObject.SetActive(false);
        //GameManager.Instance.ReStart();
    }

    void OnClick_Quit() //½ºÅ¸Æ®¾À
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("Start");
    }

    void OnValueChanged_BGM()
    {
        myDelegateAudioControl("BGM", myAudioSliderBGM.value);
    }

    void OnValueChanged_SFX()
    {
        myDelegateAudioControl("SFX", myAudioSliderSFX.value);
    }
    
}
