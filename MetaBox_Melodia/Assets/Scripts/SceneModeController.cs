using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneMode
{
    StartScene,
    LobbyScene,
    EasyMode,
    NormalMode,
    HardMode,
    ExtremeMode,
}
public class SceneModeController : MonoBehaviour
{
    public static SceneMode MySceneMode { get; set; } = SceneMode.StartScene;

    [Header("Panel Control")]
    [SerializeField] GameObject myStartPanel;
    [SerializeField] GameObject myLobbyPanel;
    [SerializeField] GameObject myOptionPanel;

    [Header("Start Panel Button")]
    [SerializeField] Button start;
    [SerializeField] Button option;
    [SerializeField] Button town;
    [SerializeField] Button exit;

    [Header("Lobby Panel Button")]
    [SerializeField] Button esay;
    [SerializeField] Button normal;
    [SerializeField] Button hard;
    [SerializeField] Button extreme;
    [SerializeField] Button back;

    void Awake()
    {
        switch (MySceneMode)
        { 
            case SceneMode.StartScene:
                SetPanel(true);
                break;

            case SceneMode.LobbyScene:
                SetPanel(false);
                break;
        }

        myOptionPanel.SetActive(false);

        start.onClick.AddListener(() => SetPanel(false));
        option.onClick.AddListener(() => myOptionPanel.SetActive(true));
        town.onClick.AddListener(() => ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main"));
        exit.onClick.AddListener(() => Application.Quit());

        esay.onClick.AddListener(OnClickEasy);
        normal.onClick.AddListener(OnClickNormal);
        hard.onClick.AddListener(OnClickHard);
        extreme.onClick.AddListener(OnClickExtreme);
        back.onClick.AddListener(() => SetPanel(true));
    }

    void SetPanel(bool startPanel)
    {
        myStartPanel.SetActive(startPanel);
        myLobbyPanel.SetActive(!startPanel);
    }

    void OnClickEasy()
    {
        MySceneMode = SceneMode.EasyMode;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickNormal()
    {
        MySceneMode = SceneMode.NormalMode;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickHard()
    {
        MySceneMode = SceneMode.HardMode;
        SceneManager.LoadScene("MelodiaEasyMode");
    }

    void OnClickExtreme()
    {
        MySceneMode = SceneMode.ExtremeMode;
        SceneManager.LoadScene("MelodiaEasyMode");
    }
}
