using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("[Production]")]
    [SerializeField] GameObject production;
    [SerializeField] GameObject viewHall;

    void Awake()
    {
        //=== screen setting ===
        Application.targetFrameRate = 60;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        optionBackBut.onClick.AddListener(delegate { OnClickOptionClosebut(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(optionBackBut.transform.position); });
    }

    void Start()
    {
        StartPanelSet(true);
        OptionPanelSet(false);
        LevelPanelSet(false);
        TutorialPanelSet(false);
        QuitPanelSet(false);

        Production();
    }

    public void StartPanelSet(bool active) => startPanel.gameObject.SetActive(active);

    public void OptionPanelSet(bool active) => optionPanel.gameObject.SetActive(active);

    public void LevelPanelSet(bool active) => levelPanel.gameObject.SetActive(active);

    public void TutorialPanelSet(bool active) => tutorialPanel.gameObject.SetActive(active);

    public void QuitPanelSet(bool active) => quitPanel.gameObject.SetActive(active);

    public void ProductionSet(bool active) => production.gameObject.SetActive(active);

    void Production()
    {
        StartCoroutine(ViewHallExtension());
    }

    IEnumerator ViewHallExtension()
    {
        ProductionSet(true);
        float timer = 0f;
        Time.timeScale = 1;

        while (viewHall.transform.localScale.x <= 30)
        {
            timer += Time.deltaTime / 40f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 35f, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.one * 30f;

        ProductionSet(false);
    }

    public void MoveScene(int sceneIndex)
    {
        StartCoroutine(ViewHallShrink(sceneIndex));
    }

    IEnumerator ViewHallShrink(int sceneIndex)
    {
        ProductionSet(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x >= 0.8f)
        {
            timer += Time.deltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.zero, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.zero;
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnClickOptionClosebut()
    {
        StartPanelSet(true);
        OptionPanelSet(false);
    }
}