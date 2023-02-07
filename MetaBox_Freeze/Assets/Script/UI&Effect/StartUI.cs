using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("Panel Control")]
    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] Fade fade = null;

    [Header("Start Panel Button")]
    [SerializeField] Button start = null;
    [SerializeField] Button option = null;
    [SerializeField] Button town = null;
    [SerializeField] Button exit = null;

    [Header("Level Panel Button")]
    [SerializeField] Button back = null;
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    void Awake()
    {
        SetPanel(true);
        optionPanel.SetActive(false);

        start.onClick.AddListener(() => SetPanel(false));
        option.onClick.AddListener(() => optionPanel.gameObject.SetActive(true));
        town.onClick.AddListener(() => ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main"));
        exit.onClick.AddListener(() => Application.Quit());

        back.onClick.AddListener(() => SetPanel(true));
        easy.onClick.AddListener(OnClick_Easy);
        normal.onClick.AddListener(OnClick_Normal);
        hard.onClick.AddListener(OnClick_Hard);
        extreme.onClick.AddListener(OnClick_Extreme);
    }

    void SetPanel(bool panel)
    {
        startPanel.SetActive(panel);
        levelPanel.SetActive(!panel);
    }

    void OnClick_Easy()
    {
        PlayerPrefs.SetInt("level", 1);
        fade.FadeOut();
    }
    void OnClick_Normal()
    {
        PlayerPrefs.SetInt("level", 2);
        fade.FadeOut();
    }
    void OnClick_Hard()
    {
        PlayerPrefs.SetInt("level", 3);
        fade.FadeOut();
    }
    void OnClick_Extreme()
    {
        PlayerPrefs.SetInt("level", 4);
        fade.FadeOut();
    }
}
