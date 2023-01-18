using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelSet : MonoBehaviour
{
    [SerializeField] Button backStartPanelBut = null;

    [Header("[Audio Slider]")]
    [SerializeField] Slider bgmSlider = null;
    [SerializeField] Slider sfxSlider = null;

    [Header("Audio Source")]
    [SerializeField] AudioSource bgmSource = null;
    [SerializeField] AudioSource sfxSource = null;

    // === variable ===
    bool isInGameOptionPanel = false;
    bool isOptionPanelOpen = false;

    void Awake()
    {
        bgmSlider.value = 1;
        sfxSlider.value = 1;

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        backStartPanelBut.onClick.AddListener(() => OnClickBakcButton());
    }

    void SetBGMVolume(float value)
    {
        sfxSource.Stop();
        bgmSource.Play();

        value = bgmSlider.value;
        bgmSource.volume = value;
    }

    void SetSFXVolume(float value)
    {
        bgmSource.Stop();
        sfxSource.Play();

        value = sfxSlider.value;
        sfxSource.volume = value;
    }

    void OnClickBakcButton()
    {
        this.gameObject.SetActive(false);
        if (isInGameOptionPanel == false)
        {
            PanelSettingMgr.Inst.StartPanelSet(true);
            isInGameOptionPanel = true;
        }
        else if(isInGameOptionPanel == true)
        {
            InGamePanelSet.Inst.OnClickOptionBut();
            isInGameOptionPanel = false;
        }
    }

}
