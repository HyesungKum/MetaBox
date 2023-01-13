using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Current Active UI")]
    [SerializeField] GameObject curUI;

    //================in game ui===================
    [Header("In Game UI")]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] GameObject inGameUI;

    [SerializeField] Button optionButton;

    //=================option ui===================
    [Header("Option UI")]
    [SerializeField] GameObject optionUI;

    [SerializeField] Button masterSoundButton;
    [SerializeField] Slider masterSlider;

    [SerializeField] Button bgmSoundButton;
    [SerializeField] Slider bgmSlider;

    [SerializeField] Button sfxSoundButton;
    [SerializeField] Slider sfxSlider;

    [SerializeField] Button opRestartButton;
    [SerializeField] Button opResumeButton;
    [SerializeField] Button opExitButton;

    //=================end game ui=================
    [Header("Game Over UI")]
    [SerializeField] GameObject gameOverUI;

    [SerializeField] Button restartButton;
    [SerializeField] Button endExitButton;

    private void Awake()
    {
        //===============in game button listener=======================================
        optionButton.onClick.AddListener(() => EventReciver.CallGamePause());
        optionButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //===============option button listener========================================
        opRestartButton.onClick.AddListener(() => SceneMove(SceneName.Main));
        opRestartButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        masterSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("Master", masterSlider.value));
        masterSoundButton.onClick.AddListener(() => ToggleSlider(masterSlider));
        masterSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("Master", call));

        bgmSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("BGM", bgmSlider.value));
        bgmSoundButton.onClick.AddListener(() => ToggleSlider(bgmSlider));
        bgmSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));

        sfxSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("SFX", sfxSlider.value));
        sfxSoundButton.onClick.AddListener(() => ToggleSlider(sfxSlider));
        sfxSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        opResumeButton.onClick.AddListener(() => EventReciver.CallGameResume());
        opResumeButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        opExitButton.onClick.AddListener(() => SceneMove(SceneName.Start));
        opExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //===============game end button listener======================================
        endExitButton.onClick.AddListener(() => SceneMove(SceneName.Start));
        endExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        restartButton.onClick.AddListener(() => SceneMove(SceneName.Main));
        restartButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //delegate chain
        EventReciver.ScoreModi += UIScoreModi;
        EventReciver.GamePause += UIGamePause;
        EventReciver.GameResume += UIGameResume;
        EventReciver.GameOver += UIGameOver;
    }

    private void Update()
    {
        timer.text = string.Format("{0:D2} : {1:D2} ",(int)(GameManager.Inst.Timer/60f),(int)(GameManager.Inst.Timer % 60f));
    }

    private void OnDisable()
    {
        EventReciver.ScoreModi -= UIScoreModi;
        EventReciver.GamePause -= UIGamePause;
        EventReciver.GameResume -= UIGameResume;
        EventReciver.GameOver -= UIGameOver;
    }

    void UIGamePause()
    {
        ShowUI(optionUI);
    }

    void UIGameResume()
    {
        Debug.Log("ui 되돌리기");
        ShowUI(inGameUI);
    }

    void UIGameOver()
    {
        ShowUI(gameOverUI);
    }

    void SceneMove(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    void ShowUI(GameObject targetUIObj)
    {
        curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }
    void ToggleSlider(Slider target)
    {
        target.interactable = target.interactable == true ? false : true;
    }

    /// <summary>
    /// modify ui score text controll
    /// </summary>
    /// <param name="value">target time value</param>
    void UIScoreModi(int value)
    {
        score.text = GameManager.Inst.Score.ToString();
    }
}
