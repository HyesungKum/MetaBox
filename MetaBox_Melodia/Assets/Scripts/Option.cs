using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public static DelegateAudioControl myDelegateAudioControl;
    public static DelegateAudioControl myDelegateAudioMute;

    [Header("Option Panel Control")]
    [SerializeField] Button backGround = null;
    [SerializeField] Button exit = null;
    [SerializeField] Button quit = null;
    [SerializeField] Button restart = null;

    [Header("Audio Volume Control")]
    [SerializeField] Button muteMaster = null;
    [SerializeField] Button muteBGM = null;
    [SerializeField] Button muteSFX = null;
    [SerializeField] Slider myAudioSliderMaster;
    [SerializeField] Slider myAudioSliderBGM;
    [SerializeField] Slider myAudioSliderSFX;

    void Awake()
    {
        backGround.onClick.AddListener(OnClick_Close);
        exit.onClick.AddListener(OnClick_Close);
        if (quit.gameObject.activeSelf) quit.onClick.AddListener(OnClick_Quit);
        if (restart.gameObject.activeSelf) restart.onClick.AddListener(OnClick_ReStart);

        muteMaster.onClick.AddListener(() => myDelegateAudioMute("Master", myAudioSliderMaster.value));
        muteBGM.onClick.AddListener(() => myDelegateAudioMute("BGM", myAudioSliderBGM.value));
        muteSFX.onClick.AddListener(() => myDelegateAudioMute("SFX", myAudioSliderSFX.value));
        myAudioSliderMaster.onValueChanged.AddListener(delegate { OnValueChanged_Master(); });
        myAudioSliderBGM.onValueChanged.AddListener(delegate { OnValueChanged_BGM(); });
        myAudioSliderSFX.onValueChanged.AddListener(delegate { OnValueChanged_SFX(); });
    }

    void OnEnable()
    {
        if (SoundManager.Inst.MasterMute == false) myAudioSliderMaster.value = SoundManager.Inst.GetVolume("Master");
        if (SoundManager.Inst.BGMMute == false) myAudioSliderBGM.value = SoundManager.Inst.GetVolume("BGM");
        if (SoundManager.Inst.SFXMute == false) myAudioSliderSFX.value = SoundManager.Inst.GetVolume("SFX");
    }

    void OnClick_Close()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
        //Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
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

    void OnValueChanged_Master()
    {
        myDelegateAudioControl("Master", myAudioSliderMaster.value);
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
