using Newtonsoft.Json;
using System.IO;
using ToolKum.AppTransition;
using UnityEngine;
using UnityEngine.UI;

public struct UserData
{
    public string ID;
    public int charIndex;
    public bool troughTown;
}

public class StartUI : MonoBehaviour
{
    public static UserData curUserData = new();
    public static int MyLevel { get; private set; } = 0;

    [Header("Panel Control")]
    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject tutorialPanel = null;
    [SerializeField] GameObject exitPanel = null;
    [SerializeField] Fade fade = null; //for scene transitions

    [Header("Start Panel Button")]
    [SerializeField] Button start = null;
    [SerializeField] Button option = null;
    [SerializeField] Button tutorial = null;
    [SerializeField] Button exit = null;

    [Header("Level Panel Button")]
    [SerializeField] Button back = null; //activate start panel
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    [Header("Tutorial Panel Button")]
    [SerializeField] Button back_Start = null;

    [Header("Exit Panel Button")]
    [SerializeField] Button quit = null;
    [SerializeField] Button back_Game = null;

    [Header("[Application Setting]")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";
    [SerializeField] private string localSavePath = "/MetaBox/SaveData/HCSaveData.json";
#if UNITY_EDITOR
    [SerializeField] private string defaultPath = "/MetaBox/SaveData/";
#else
    private string defaultPath = "/storage/emulated/0/MetaBox/SaveData/TownSaveData.json";
#endif

    void Awake()
    {
        SetPanel(true);
        optionPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        exitPanel.SetActive(false);

        start.onClick.AddListener(() => SetPanel(false));
        option.onClick.AddListener(() => optionPanel.SetActive(true));
        tutorial.onClick.AddListener(() => tutorialPanel.SetActive(true)); //ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main")
        exit.onClick.AddListener(() => startPanel.SetActive(false));
        exit.onClick.AddListener(() => exitPanel.SetActive(true));

        back.onClick.AddListener(() => SetPanel(true));
        easy.onClick.AddListener(OnClick_Easy);
        normal.onClick.AddListener(OnClick_Normal);
        hard.onClick.AddListener(OnClick_Hard);
        extreme.onClick.AddListener(OnClick_Extreme);

        back_Start.onClick.AddListener(() => tutorialPanel.SetActive(false));

        quit.onClick.AddListener(() => AppQuit());
        back_Game.onClick.AddListener(() => startPanel.SetActive(true));
        back_Game.onClick.AddListener(() => exitPanel.SetActive(false));

    }

    private void Start()
    {
        SoundManager.Instance.AddButtonListener();
        SoundManager.Instance.MusicStart(0);
        FileCheck();
    }

    //=============================================Check Local File===============================================
    private void FileCheck()
    {
        if (File.Exists(localSavePath))
        {
            curUserData = ReadSaveData(localSavePath);
        }
        else //존재하지 않을때 
        {
            curUserData.ID = "New";
            curUserData.charIndex = 0;
            curUserData.troughTown = false;
        }
    }
    private UserData ReadSaveData(string path)
    {
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

        return readData;
    }

    void SetPanel(bool panel)
    {
        startPanel.SetActive(panel);
        levelPanel.SetActive(!panel);
    }

    void OnClick_Easy()
    {
        MyLevel = 1;
        fade.FadeOut(1);
    }
    void OnClick_Normal()
    {
        MyLevel = 2;
        fade.FadeOut(1);
    }
    void OnClick_Hard()
    {
        MyLevel = 3;
        fade.FadeOut(1);
    }
    void OnClick_Extreme()
    {
        MyLevel = 4;
        fade.FadeOut(1);
    }

    void AppQuit()
    {
        if (curUserData.troughTown) AppTrans.MoveScene(mainPackName);
        else Application.Quit();
    }
}
