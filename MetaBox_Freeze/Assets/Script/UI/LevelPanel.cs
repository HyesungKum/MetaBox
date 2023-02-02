using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] GameObject startPanel = null;
    [SerializeField] Fade fade = null;
    [SerializeField] Button back = null;
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    void Awake()
    {
        back.onClick.AddListener(() => startPanel.SetActive(true));
        easy.onClick.AddListener(OnClick_Easy);
        normal.onClick.AddListener(OnClick_Normal);
        hard.onClick.AddListener(OnClick_Hard);
        extreme.onClick.AddListener(OnClick_Extreme);
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
