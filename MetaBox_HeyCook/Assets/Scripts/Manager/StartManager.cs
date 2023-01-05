using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ToolKum.AppTransition;

public class StartManager : MonoBehaviour
{
    //================app transition============================
    [Header("Application Setting")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    //=================UI==========================
    [Header("Title")]
    [SerializeField] GameObject title;

    [Header("First Buttons")]
    [Tooltip("Start, option, exit group")]
    [SerializeField] GameObject firstButtonGroup;

    [SerializeField] Button startButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button exitButton;

    [Header("Second Buttons")]
    [Tooltip("difficulty button group")]
    [SerializeField] GameObject secondButtonGroup;

    [SerializeField] Button easyButton;
    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button extremeButton;

    private void Awake()
    {
        //=================screen setting==================
        //Screen.SetResolution(1920, 1080, true);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        //=================apply button listener===================
        startButton .onClick.AddListener(() => ShowDifficulty());
        optionButton.onClick.AddListener(() => ShowOption());
        exitButton  .onClick.AddListener(() => AppTrans.MoveScene(mainPackName)); ;

        easyButton   .onClick.AddListener(() => SceneMove(1));
        normalButton .onClick.AddListener(() => SceneMove(2));
        hardButton   .onClick.AddListener(() => SceneMove(3));
        extremeButton.onClick.AddListener(() => SceneMove(4));
    }

    void SceneMove(int level)
    {
        SceneManager.LoadScene(SceneName.Main);
    }

    void ShowOption()
    {
        
    }

    void ShowDifficulty()
    {
        title.SetActive(false);
        firstButtonGroup.SetActive(false);
        secondButtonGroup.SetActive(true);
    }
}
