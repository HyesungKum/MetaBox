using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [Header("Game Control")]
    [SerializeField] Button exit = null; //button X. Disable the optionPanel.

    [Header("Sound Control")]
    [SerializeField] Slider BGMSlider = null; //slider to adjust the BGM volume
    [SerializeField] Slider SFXSlider = null; //slider to adjust the SFX volume

    void Awake()
    {
        exit.onClick.AddListener(OnClick_Close);

        BGMSlider.onValueChanged.AddListener(delegate { OnValueChanged_Sound(); });
        SFXSlider.onValueChanged.AddListener(delegate { OnValueChanged_Music(); });
    }

    void OnEnable()
    {
        BGMSlider.value = SoundManager.Instance.GetVolume("BGM");
        SFXSlider.value = SoundManager.Instance.GetVolume("SFX");
    }

    void OnClick_Close()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        this.gameObject.SetActive(false);
    }

    void OnValueChanged_Sound() //bgm
    {
        SoundManager.Instance.AudioVolumeControl("BGM", BGMSlider.value);
    }

    void OnValueChanged_Music() //sfx
    {
        SoundManager.Instance.AudioVolumeControl("SFX", SFXSlider.value);
    }
}
