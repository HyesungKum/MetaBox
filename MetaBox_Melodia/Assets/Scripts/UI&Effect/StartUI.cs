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
    littlestar,
    rabbit,
    butterfly,
    stone,
    end
}

public class StartUI : MonoBehaviour
{
    public static SceneMode MySceneMode { get; private set; } = SceneMode.littlestar;
    public static int Level { get; private set; }

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

    [Header("Exit Panel")]
    [SerializeField] Button endTutorial;

    [Header("Exit Panel")]
    [SerializeField] Button yes;
    [SerializeField] Button no;

    

    private void Awake()
    {
        SetPanel(Panel.Start);

        start.onClick.AddListener(() => SetPanel(Panel.Music));
        option.onClick.AddListener(() => myOptionPanel.SetActive(true));
        tutorial.onClick.AddListener(() => myTutorialPanel.SetActive(true)); //ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main")
        exit.onClick.AddListener(() => myExitPanel.SetActive(true));

        musicImg.sprite = scriptableImg.MusicImg[(int)MySceneMode];
        selectMusic.onClick.AddListener(OnClickSelectMusic);
        prev.onClick.AddListener(OnClickPrev);
        next.onClick.AddListener(OnClickNext);
        backStart.onClick.AddListener(() => SetPanel(Panel.Start));

        esay.onClick.AddListener(OnClickEasy);
        normal.onClick.AddListener(OnClickNormal);
        hard.onClick.AddListener(OnClickHard);
        extreme.onClick.AddListener(OnClickExtreme);
        backMusic.onClick.AddListener(() => SetPanel(Panel.Music));

        endTutorial.onClick.AddListener(() => myTutorialPanel.SetActive(false));

        yes.onClick.AddListener(() => Application.Quit());
        no.onClick.AddListener(() => myExitPanel.SetActive(false));
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

    void OnClickSelectMusic()
    {
        SetPanel(Panel.Level);
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
        SceneManager.LoadScene("Main");
    }

    void OnClickNormal()
    {
        Level = 2;
        SceneManager.LoadScene("Main");
    }

    void OnClickHard()
    {
        Level = 3;
        SceneManager.LoadScene("Main");
    }

    void OnClickExtreme()
    {
        Level = 4;
        SceneManager.LoadScene("Main");
    }
    #endregion
}
