using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [Header("Game Control")]
    [SerializeField] Button backGround = null; //button outside the optionPanel. Disable the optionPanel.
    [SerializeField] Button exit = null; //button X. Disable the optionPanel.
    [SerializeField] Button quit = null; //button to move to start scene
    [SerializeField] Button restart = null; //button to return to stage 1 and restart the game.


    [Header("Sound Control")]
    [SerializeField] Button sound = null; //bgm Mute
    [SerializeField] Button music = null; //sfx Mute
    [SerializeField] Slider soundSlider = null; //slider to adjust the BGM volume
    [SerializeField] Slider musicSlider = null; //slider to adjust the SFX volume

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
        this.gameObject.SetActive(false);
        GameManager.Instance.ReStart();
    }

    void OnClick_Quit() //move to start scene
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SoundManager.Instance.MusicStart(0);
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
