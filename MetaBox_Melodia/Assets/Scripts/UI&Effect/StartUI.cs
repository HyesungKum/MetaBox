using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Panel
{
    Start,
    Music,
    Level,
    Option
}

public enum SceneMode
{
    LittleStar,
    Rabbit,
    Butterfly,
    Stone,
}

public class StartUI : MonoBehaviour
{
    public Panel MyPanel { get; set; } = Panel.Start;
    public static SceneMode MySceneMode { get; private set; }
    public static int Level { get; private set; }


    [Header("Panel Control")]
    [SerializeField] GameObject myStartPanel;
    [SerializeField] GameObject myMusicPanel;
    [SerializeField] GameObject myLevelPanel;
    [SerializeField] GameObject myOptionPanel;

    [Header("Start Panel Button")]
    [SerializeField] Button start;
    [SerializeField] Button option;
    [SerializeField] Button town;
    [SerializeField] Button exit;

    [Header("Music Panel Button")]
    [SerializeField] Button music;
    [SerializeField] Button prev;
    [SerializeField] Button next;
    [SerializeField] Button backStart;

    [Header("Level Panel Button")]
    [SerializeField] Button esay;
    [SerializeField] Button normal;
    [SerializeField] Button hard;
    [SerializeField] Button extreme;
    [SerializeField] Button backMusic;

    private void Awake()
    {
        SetPanel(MyPanel);

        start.onClick.AddListener(() => SetPanel(Panel.Music));
        option.onClick.AddListener(() => SetPanel(Panel.Option));
        town.onClick.AddListener(() => ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main"));
        exit.onClick.AddListener(() => Application.Quit());

        esay.onClick.AddListener(OnClickEasy);
        normal.onClick.AddListener(OnClickNormal);
        hard.onClick.AddListener(OnClickHard);
        extreme.onClick.AddListener(OnClickExtreme);
        backMusic.onClick.AddListener(() => SetPanel(Panel.Music));
    }

    
    void SetPanel(Panel setPanel)
    {
        MyPanel = setPanel;
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
            case Panel.Option:
                myOptionPanel.SetActive(true);
                break;
        }
    }

    void OnClickEasy()
    {
        Level = 1;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickNormal()
    {
        Level = 2;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickHard()
    {
        Level = 3;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickExtreme()
    {
        Level = 4;
        SceneManager.LoadScene("MelodiaEasyMode");
    }
}
