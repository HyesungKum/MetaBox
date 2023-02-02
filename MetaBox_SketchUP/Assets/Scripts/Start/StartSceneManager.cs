using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    #region Singleton
    private static StartSceneManager instance;
    public static StartSceneManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StartSceneManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(StartSceneManager), typeof(StartSceneManager)).GetComponent<StartSceneManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("[Panel Setting]")]
    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject tutorialPanel = null;
    [SerializeField] GameObject quitPanel = null;

    [Header("[Option Close Button]")]
    [SerializeField] Button optionBackBut = null;

    void Awake()
    {
        optionBackBut.onClick.AddListener(delegate { OnClickOptionClosebut(); SoundManager.Inst.ButtonSFXPlay(); });
    }

    void Start()
    {
        StartPanelSet(true);
        OptionPanelSet(false);
        LevelPanelSet(false);
        TutorialPanelSet(false);
        QuitPanelSet(false);
    }

    public void StartPanelSet(bool active) => startPanel.gameObject.SetActive(active);

    public void OptionPanelSet(bool active) => optionPanel.gameObject.SetActive(active);

    public void LevelPanelSet(bool active) => levelPanel.gameObject.SetActive(active);

    public void TutorialPanelSet(bool active) => tutorialPanel.gameObject.SetActive(active);

    public void QuitPanelSet(bool active) => quitPanel.gameObject.SetActive(active);

    public void OnClickOptionClosebut()
    {
        StartPanelSet(true);
        OptionPanelSet(false);
    }
}
