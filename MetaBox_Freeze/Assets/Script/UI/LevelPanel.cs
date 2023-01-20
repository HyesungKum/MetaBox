using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] Loading loadingObject = null;
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    void Awake()
    {
        easy.onClick.AddListener(OnClick_Easy);
        normal.onClick.AddListener(OnClick_Normal);
        hard.onClick.AddListener(OnClick_Hard);
        extreme.onClick.AddListener(OnClick_Extreme);
    }

    void OnClick_Easy()
    {
        PlayerPrefs.SetInt("level", 1);
        this.gameObject.SetActive(false);
        loadingObject.gameObject.SetActive(true);
    }
    void OnClick_Normal()
    {
        PlayerPrefs.SetInt("level", 2);
        this.gameObject.SetActive(false);
        loadingObject.gameObject.SetActive(true);
    }
    void OnClick_Hard()
    {
        PlayerPrefs.SetInt("level", 3);
        this.gameObject.SetActive(false);
        loadingObject.gameObject.SetActive(true);
    }
    void OnClick_Extreme()
    {
        PlayerPrefs.SetInt("level", 4);
        this.gameObject.SetActive(false);
        loadingObject.gameObject.SetActive(true);
    }
}
