using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelSet : MonoBehaviour
{
    [SerializeField] Button backStartPanelBut = null;

    [Header("[Audio Slider]")]
    [SerializeField] Slider bgmSlider = null;
    [SerializeField] Slider sfxSlider = null;


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
        AudioManager.Inst.sfxSource.Stop();
        AudioManager.Inst.bgmSource.Play();

        value = bgmSlider.value;
        AudioManager.Inst.bgmSource.volume = value;
    }

    void SetSFXVolume(float value)
    {
        AudioManager.Inst.bgmSource.Stop();
        AudioManager.Inst.sfxSource.Play();

        value = sfxSlider.value;
        AudioManager.Inst.sfxSource.volume = value;
    }

    void OnClickBakcButton()
    {
        this.gameObject.SetActive(false);
        PanelSettingMgr.Inst.StartPanelSet(true);
        isInGameOptionPanel = true;
    }
}
