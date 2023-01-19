using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    // Start is called before the first frame update
    void Start()
    {
        easy.onClick.AddListener(delegate { OnClick_Easy(); });
        normal.onClick.AddListener(delegate { OnClick_Normal(); });
        hard.onClick.AddListener(delegate { OnClick_Hard(); });
        extreme.onClick.AddListener(delegate { OnClick_Extreme(); });
    }

    void OnClick_Easy()
    {
        GameManager.Instance.LevelSetting(1);
        this.gameObject.SetActive(false);
    }
    void OnClick_Normal()
    {
        GameManager.Instance.LevelSetting(2);
        this.gameObject.SetActive(false);
    }
    void OnClick_Hard()
    {
        GameManager.Instance.LevelSetting(3);
        this.gameObject.SetActive(false);
    }
    void OnClick_Extreme()
    {
        GameManager.Instance.LevelSetting(4);
        this.gameObject.SetActive(false);
    }
}
