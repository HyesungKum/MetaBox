using Newtonsoft.Json;
using System.IO;
using ToolKum.AppTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct UserData
{
    public string ID;
    public int charIndex;
    public bool troughTown;
}

public enum Panel
{
    Start,
    Music,
    Level,
    Option
}

public enum SceneMode
{
    littlestar,
    rabbit,
    butterfly,
    stone,
    end
}

public class StartUI : MonoBehaviour
{
    public static UserData curUserData = new();
    public static SceneMode MySceneMode { get; private set; } = SceneMode.littlestar;
    public static int Level { get; private set; } = 1;

    public ScriptableObj scriptableImg = null;

    [Header("Panel Control")]
    [SerializeField] GameObject myStartPanel;
    [SerializeField] GameObject myMusicPanel;
    [SerializeField] GameObject myLevelPanel;
    [SerializeField] GameObject myOptionPanel;
    [SerializeField] GameObject myTutorialPanel;
    [SerializeField] GameObject myExitPanel;

    [Header("Start Panel")]
    [SerializeField] Button start;
    [SerializeField] Button option;
    [SerializeField] Button tutorial;
    [SerializeField] Button exit;

    [Header("Music Panel")]
    [SerializeField] Image musicImg;
    [SerializeField] Button selectMusic;
    [SerializeField] Button prev;
    [SerializeField] Button next;
    [SerializeField] Button backStart;

    [Header("Level Panel")]
    [SerializeField] Button esay;
    [SerializeField] Button normal;
    [SerializeField] Button hard;
    [SerializeField] Button extreme;
    [SerializeField] Button backMusic;

    [Header("Tutorial Panel")]
    [SerializeField] Button endTutorial;

    [Header("Exit Panel")]
    [SerializeField] Button yes;
    [SerializeField] Button no;

    [Header("[Application Setting]")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";
    [SerializeField] private string localSavePath = "/MetaBox/SaveData/HCSaveData.json";
#if UNITY_EDITOR
    [SerializeField] private string defaultPath = "/MetaBox/SaveData/";
#else
    private string defaultPath = "/storage/emulated/0/MetaBox/SaveData/TownSaveData.json";
#endif

    private void Awake()
    {
        SetPanel(Panel.Start);

        start.onClick.AddListener(() => SetPanel(Panel.Music));
        option.onClick.AddListener(() => myOptionPanel.SetActive(true));
        tutorial.onClick.AddListener(() => myTutorialPanel.SetActive(true)); //ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main")
        exit.onClick.AddListener(() => myExitPanel.SetActive(true));

        musicImg.sprite = scriptableImg.MusicImg[(int)MySceneMode];
        selectMusic.onClick.AddListener(() => SetPanel(Panel.Level));
        prev.onClick.AddListener(OnClickPrev);
        next.onClick.AddListener(OnClickNext);
        backStart.onClick.AddListener(() => SetPanel(Panel.Start));

        esay.onClick.AddListener(OnClickEasy);
        normal.onClick.AddListener(OnClickNormal);
        hard.onClick.AddListener(OnClickHard);
        extreme.onClick.AddListener(OnClickExtreme);
        backMusic.onClick.AddListener(() => SetPanel(Panel.Music));

        endTutorial.onClick.AddListener(() => myTutorialPanel.SetActive(false));

        yes.onClick.AddListener(() => AppQuit());
        no.onClick.AddListener(() => myExitPanel.SetActive(false));

    }

    private void Start()
    {
        SoundManager.Inst.AddButtonListener();
        SoundManager.Inst.BGMPlay(1);
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

    void SetPanel(Panel setPanel)
    {
        switch (setPanel)
        {
            case Panel.Start:
                myStartPanel.SetActive(true);
                myMusicPanel.SetActive(false);
                myLevelPanel.SetActive(false);
                myOptionPanel.SetActive(false);
                break;

            case Panel.Music:
                myStartPanel.SetActive(false);
                myMusicPanel.SetActive(true);
                myLevelPanel.SetActive(false);
                myOptionPanel.SetActive(false);
                break;
            case Panel.Level:
                myStartPanel.SetActive(false);
                myMusicPanel.SetActive(false);
                myLevelPanel.SetActive(true);
                myOptionPanel.SetActive(false);
                break;
        }
    }

    void OnClickPrev()
    {
        if (MySceneMode == SceneMode.littlestar) MySceneMode = SceneMode.end -1;
        else MySceneMode--;
        
        musicImg.sprite = scriptableImg.MusicImg[(int)MySceneMode];
    }

    void OnClickNext()
    {
        MySceneMode++;
        if (MySceneMode == SceneMode.end) MySceneMode = 0;

        musicImg.sprite = scriptableImg.MusicImg[(int)MySceneMode];
    }

    #region Level
    void OnClickEasy()
    {
        Level = 1;
        SceneManager.LoadScene("Loading");
    }

    void OnClickNormal()
    {
        Level = 2;
        SceneManager.LoadScene("Loading");
    }

    void OnClickHard()
    {
        Level = 3;
        SceneManager.LoadScene("Loading");
    }

    void OnClickExtreme()
    {
        Level = 4;
        SceneManager.LoadScene("Loading");
    }
    #endregion

    void AppQuit()
    {
        if (curUserData.troughTown) AppTrans.MoveScene(mainPackName);
        else Application.Quit();
    }
}
