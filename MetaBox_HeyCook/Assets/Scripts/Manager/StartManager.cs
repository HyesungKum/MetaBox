using System.Collections;
using TMPro;
using ToolKum.AppTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Env
{
    Connected = 0,
    Disconnected = 1
}

public class StartManager : MonoBehaviour
{
    //================app transition============================
    [Header("[Application Setting]")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    //=================UI==========================
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    [Header("[Main UI Group]")]
    [Tooltip("Start, option, exit group")]
    [SerializeField] GameObject mainUIGroup;

    [SerializeField] Button startButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button exitButton;

    [Header("[Difficulty UI Group]")]
    [Tooltip("Difficulty button group")]
    [SerializeField] GameObject difficultyUIGroup;

    [SerializeField] Button easyButton;
    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button extremeButton;

    [SerializeField] Button difficultyExitButton;

    [Header("[Option UI Group]")]
    [Tooltip("difficulty button group")]
    [SerializeField] GameObject optionUIGroup;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button optionExitButton;
    [SerializeField] Button optionInitBuctton;

    [Header("[Exit Check UI Group]")]
    [SerializeField] GameObject ExitCheckUIGroup;
    [SerializeField] Button ExitCheckExitButton;
    [SerializeField] Button ExitCheckVillageButton;
    [SerializeField] Button ExitCheckReturnButton;

    [Header("[Production]")]
    [SerializeField] GameObject production;
    [SerializeField] GameObject viewHall;

    [Header("[Online Enviorment]")]
    [SerializeField] TextMeshProUGUI PlayerIDText;
    [SerializeField] Image connectImage;
    [Tooltip("Index 0 Image is Connected icon Image \n" +
             "Index 1 Image is Disconnected icon Image")]
    [SerializeField] Sprite[] EnvSprites;

    private void Awake()
    {
        //=================screen setting==================
        Application.targetFrameRate = 60;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        //=================apply first button listener===================
        startButton.onClick.AddListener(() => ShowUI(difficultyUIGroup));
        startButton  .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        optionButton.onClick.AddListener(() => ShowUI(optionUIGroup));
        optionButton .onClick.AddListener(() => SoundCheck());
        optionButton .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        exitButton   .onClick.AddListener(() => ShowUI(ExitCheckUIGroup));
        exitButton   .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //==============apply Difficulty Button listener=========================
        easyButton   .onClick.AddListener(() => SceneMove(1));
        easyButton   .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        normalButton .onClick.AddListener(() => SceneMove(2));
        normalButton .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        hardButton   .onClick.AddListener(() => SceneMove(3));
        hardButton   .onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        extremeButton.onClick.AddListener(() => SceneMove(4));
        extremeButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        difficultyExitButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        difficultyExitButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //============apply option button listener==========================
        bgmSlider        .onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));
                         
        sfxSlider        .onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        optionExitButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        optionExitButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        optionInitBuctton.onClick.AddListener(()=> PlayerPrefs.DeleteAll());
        optionInitBuctton.onClick.AddListener(()=> SoundManager.Inst.CallSfx("ButtonClick"));

        //apply exit check listener
        ExitCheckExitButton.onClick.AddListener(() => Application.Quit());
        ExitCheckExitButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));
        
        ExitCheckVillageButton.onClick.AddListener(() => AppTrans.MoveScene(mainPackName));
        ExitCheckVillageButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        ExitCheckReturnButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        ExitCheckReturnButton.onClick.AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));
    }

    private void Start()
    {
        //get internet connection
        if (SaveLoadManger.Inst.OnlineMode) connectImage.sprite = EnvSprites[(int)Env.Connected];
        else connectImage.sprite = EnvSprites[(int)Env.Disconnected];

        //get guest id
        PlayerIDText.text = SaveLoadManger.Inst.GetID();

        //set startscene sound
        SoundManager.Inst.SetBGM("StartBGM");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();

        //view hall production
        Production();
    }

    void SceneMove(int level)
    {
        LevelTransfer.Inst.Level = level;
        StartCoroutine(nameof(ViewHallShrink));
    }
    void ShowUI(GameObject targetUIObj)
    {
        curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    //==========================================Production Controll==========================================
    void Production()
    {
        StartCoroutine(nameof(ViewHallExtension));
    }
    IEnumerator ViewHallExtension()
    {
        production.SetActive(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x <= 45)
        {
            timer += Time.deltaTime * 0.067f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 46f, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.one * 45f;
        production.SetActive(false);
    }
    IEnumerator ViewHallShrink()
    {
        production.SetActive(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x >= 0.8f)
        {
            timer += Time.deltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.zero, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.zero;

        SceneManager.LoadScene(SceneName.Loading);
    }

    //==========================================Sound Slider Controll========================================
    void SoundCheck()
    {
        bgmSlider.value = SoundManager.Inst.GetVolume("BGM");
        sfxSlider.value = SoundManager.Inst.GetVolume("SFX");
    }
    void ToggleSlider(Slider target)
    {
        target.interactable = target.interactable == true ? false : true;
    }
}
