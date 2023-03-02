using UnityEngine;
using UnityEngine.UI;

public class StartOption : MonoBehaviour
{
    [Header("Audio Slider")]
    [SerializeField] Slider startBgmSlider = null;
    [SerializeField] Slider startButSfxSlider = null;

    void Awake()
    {
        startBgmSlider.value = SoundManager.Inst.BGMValue;
        startButSfxSlider.value = SoundManager.Inst.SFXValue;

        startBgmSlider.onValueChanged.AddListener(OnChangedBGMControl);
        startButSfxSlider.onValueChanged.AddListener(OnChangedSFXControl);
    }

    void OnChangedBGMControl(float sound) => SoundManager.Inst.BGMControl(startBgmSlider);

    void OnChangedSFXControl(float sound) => SoundManager.Inst.SFXControl(startButSfxSlider);
}