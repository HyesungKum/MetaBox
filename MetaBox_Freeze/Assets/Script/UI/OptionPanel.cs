using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] Button back = null;
    [SerializeField] Button quit = null;
    [SerializeField] Button restart = null;
    [SerializeField] Button exitOption = null;

    [SerializeField] Slider soundSlider = null;
    [SerializeField] Slider musicSlider = null;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(OnClick_ReStart);
        quit.onClick.AddListener(OnClick_Quit);
        restart.onClick.AddListener(OnClick_ReStart);
        exitOption.onClick.AddListener(OnClick_ReStart);
        soundSlider.onValueChanged.AddListener(delegate { OnValueChanged_Sound(); });
        musicSlider.onValueChanged.AddListener(delegate { OnValueChanged_Music(); });
    }
    
    void OnClick_ReStart()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void OnClick_Quit() //종료하고 타이틀로
    {

    }

    void OnValueChanged_Sound() //bgm
    {

    }

    void OnValueChanged_Music() //sfx
    {

    }

}
