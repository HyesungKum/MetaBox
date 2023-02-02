using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] Button backGround = null;
    [SerializeField] Button exit = null;
    [SerializeField] Button quit = null;
    [SerializeField] Button restart = null;
    
    [SerializeField] Button sound = null; //bgm
    [SerializeField] Button music = null; //sfx
    [SerializeField] Slider soundSlider = null;
    [SerializeField] Slider musicSlider = null;

    void Awake()
    {
        backGround.onClick.AddListener(OnClick_Close);
        exit.onClick.AddListener(OnClick_Close);
        if (quit.gameObject.activeSelf) quit.onClick.AddListener(OnClick_Quit);
        if (restart.gameObject.activeSelf) restart.onClick.AddListener(OnClick_ReStart);

        sound.onClick.AddListener(() => SoundManager.Instance.AudioMute("BGM", soundSlider.value));
        music.onClick.AddListener(() => SoundManager.Instance.AudioMute("SFX", musicSlider.value));
        soundSlider.onValueChanged.AddListener(delegate { OnValueChanged_Sound(); });
        musicSlider.onValueChanged.AddListener(delegate { OnValueChanged_Music(); });
    }

    void OnEnable()
    {
        if (SoundManager.Instance.BGMMute == false) soundSlider.value = SoundManager.Instance.GetVolume("BGM");
        if (SoundManager.Instance.SFXMute == false) musicSlider.value = SoundManager.Instance.GetVolume("SFX");
    }

    void OnClick_Close()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        this.gameObject.SetActive(false);
    }

    void OnClick_ReStart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        GameManager.Instance.ReStart();
        this.gameObject.SetActive(false);
    }

    void OnClick_Quit() //½ºÅ¸Æ®¾À
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("Start");
    }

    void OnValueChanged_Sound() //bgm
    {
        SoundManager.Instance.AudioVolumeControl("BGM", soundSlider.value);
    }

    void OnValueChanged_Music() //sfx
    {
        SoundManager.Instance.AudioVolumeControl("SFX", musicSlider.value);
    }
}
