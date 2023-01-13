using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ToolKum.AppTransition;
using System;

public class StartManager : MonoBehaviour
{
    //================app transition============================
    [Header("Application Setting")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    //=================UI==========================
    [Header("Current Active UI")]
    [SerializeField] GameObject curUI;

    [Header("Main UI Group")]
    [Tooltip("Start, option, exit group")]
    [SerializeField] GameObject mainUIGroup;

    [SerializeField] Button startButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button villageButton;
    [SerializeField] Button exitButton;

    [Header("Second UI Group")]
    [Tooltip("difficulty button group")]
    [SerializeField] GameObject difficultyUIGroup;

    [SerializeField] Button easyButton;
    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button extremeButton;

    [SerializeField] Button difficultyExitButton;

    [Header("Option UI Group")]
    [Tooltip("difficulty button group")]
    [SerializeField] GameObject optionUIGroup;

    [SerializeField] Button masterSoundButton;
    [SerializeField] Slider masterSlider;

    [SerializeField] Button bgmSoundButton;
    [SerializeField] Slider bgmSlider;

    [SerializeField] Button sfxSoundButton;
    [SerializeField] Slider sfxSlider;

    [SerializeField] Button optionExitButton;

    private void Awake()
    {
        //=================screen setting==================
        Application.targetFrameRate = 60;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        //=================apply first button listener===================
        startButton  .onClick.AddListener(() => ShowUI(difficultyUIGroup));
        startButton  .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        optionButton .onClick.AddListener(() => ShowUI(optionUIGroup));
        optionButton .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        villageButton.onClick.AddListener(() => AppTrans.MoveScene(mainPackName));
        villageButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        exitButton   .onClick.AddListener(() => Application.Quit());
        exitButton   .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //==============apply Difficulty Button listener=========================
        easyButton   .onClick.AddListener(() => SceneMove(1));
        easyButton   .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        normalButton .onClick.AddListener(() => SceneMove(2));
        normalButton .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        hardButton   .onClick.AddListener(() => SceneMove(3));
        hardButton   .onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        extremeButton.onClick.AddListener(() => SceneMove(4));
        extremeButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        difficultyExitButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        difficultyExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //============apply option button listener==========================
        masterSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("Master", masterSlider.value));
        masterSoundButton.onClick.AddListener(() => ToggleSlider(masterSlider));
        masterSlider     .onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("Master", call));
                         
        bgmSoundButton   .onClick.AddListener(() => SoundManager.Inst.ToggleControll("BGM", bgmSlider.value));
        bgmSoundButton   .onClick.AddListener(() => ToggleSlider(bgmSlider));
        bgmSlider        .onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));
                         
        sfxSoundButton   .onClick.AddListener(() => SoundManager.Inst.ToggleControll("SFX", sfxSlider.value));
        sfxSoundButton   .onClick.AddListener(() => ToggleSlider(sfxSlider));
        sfxSlider        .onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        optionExitButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        optionExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));
    }

    private void Start()
    {
        SoundManager.Inst.SetBGM("StartBGM");
        SoundManager.Inst.PlayBGM();
    }

    void SceneMove(int level)
    {
        SceneManager.LoadScene(SceneName.Main);
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
}
