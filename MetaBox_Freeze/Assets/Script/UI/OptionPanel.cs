using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Start is called before the first frame update
    void Start()
    {
        backGround.onClick.AddListener(OnClick_Close);
        quit.onClick.AddListener(OnClick_Quit);
        if(restart != quit) restart.onClick.AddListener(OnClick_ReStart);
        exit.onClick.AddListener(OnClick_Close);

        sound.onClick.AddListener(delegate { OnClick_MuteBGM(); });
        music.onClick.AddListener(delegate { OnClick_MuteSFX(); });
        soundSlider.onValueChanged.AddListener(delegate { OnValueChanged_Sound(); });
        musicSlider.onValueChanged.AddListener(delegate { OnValueChanged_Music(); });
    }

    private void OnEnable()
    {
        if(SoundManager.Instance.BGMMute == false) soundSlider.value = SoundManager.Instance.GetVolume("BGM");
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

    void OnClick_Quit() //종료하고 타이틀로
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene(0);
    }

    void OnClick_MuteBGM()
    {
        SoundManager.Instance.AudioMute("BGM", soundSlider.value);
    }
    void OnClick_MuteSFX()
    {
        SoundManager.Instance.AudioMute("SFX", musicSlider.value);
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
