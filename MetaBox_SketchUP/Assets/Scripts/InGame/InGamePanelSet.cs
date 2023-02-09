using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InGamePanelSet : MonoBehaviour
{
    #region Singleton
    private static InGamePanelSet instance;
    public static InGamePanelSet Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGamePanelSet>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(InGamePanelSet), typeof(InGamePanelSet)).GetComponent<InGamePanelSet>();

                }
            }
            return instance;
        }
    }
    #endregion

    #region SerializeField
    [Header("[InGame Panel Set]")]
    [SerializeField] GameObject inGameCanvas = null;
    [SerializeField] GameObject selectPanel = null;
    [SerializeField] GameObject closePlayOneBrush = null;
    [SerializeField] GameObject losePanel = null;
    [SerializeField] GameObject winPanel = null;
    [SerializeField] GameObject optionPanel = null;

    [Header("[OneBrush Paly Obj]")]
    [SerializeField] GameObject onBushObj = null;

    [Header("[Line Color and Size Change]")]
    [SerializeField] private Canvas lineChangedPanel = null;

    [Header("[ReStart]")]
    [SerializeField] Button loseReStartBut = null;
    [SerializeField] Button winReStartBut = null;
    [SerializeField] Button optionBut = null;

    [Header("[Option Button Setting]")]
    [SerializeField] Button quitBut = null;
    [SerializeField] Button resumeBut = null;

    [Header("[Wait Time Img]")]
    [SerializeField] TextMeshProUGUI startWaitTime = null;
    [SerializeField] GameObject waitTimeObjs = null;

    [Header("[Timer Text]")]
    [SerializeField] TextMeshProUGUI playTimeText = null;

    [Header("[Character Move]")]
    [SerializeField] GameObject characterMove = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds waitOnSceonds = null;
    #endregion

    // === wait 3 seconds ===
    //float waitTime = 3f;
    float waitTime = 3f;
    bool wait = false;

    // === play time total 10 seconds === 
    private float playTime = 600;
    public float PlayTime
    { get { return playTime; } set { playTime = value; } }


    float curTime = 0;
    // === test ===
    //float playTime = 6;

    bool isOptionPanelOpen = false;
    bool isTimeStop = false;

    void Awake()
    {
        waitHalf = new WaitForSeconds(0.5f);
        waitOnSceonds = new WaitForSeconds(1f);

        Time.timeScale = 1;
        // === changed (true) ===
        startWaitTime.gameObject.SetActive(false);
        waitTimeObjs.gameObject.SetActive(true);
        StartCoroutine(CountDowns());
        playTimeText.gameObject.SetActive(false);

        // === changed (false) ===
        FirstSet(false);
        OneBrushPlayPanelSet(false);
        //ClearPanelSet(false);

        // === button event Set ===
        #region
        loseReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.GameLoseSFXPlay(); });
        winReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.GameClearSFXPlay(); });
        optionBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
        resumeBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
        quitBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.ButtonSFXPlay(); });
        #endregion
    }

    void Update()
    {
        #region Timer Setting
        if (playTimeText.gameObject.active == true)
        {
            PlayTimeDown();
        }
        #endregion
    }

    IEnumerator CountDowns()
    {
        CountDown countDown = null;
        waitTimeObjs.TryGetComponent<CountDown>(out countDown);
         
        int waitTime = 3;

        for(int i = waitTime; i > 0; i --)
        {
            countDown.ShowWaitTime(waitTime);
            waitTime--;
            yield return waitOnSceonds;
        }

        waitTimeObjs.gameObject.SetActive(false);
        startWaitTime.gameObject.SetActive(true);
        startWaitTime.text = "Go";
        yield return waitHalf;
        FirstSet(true);
        playTimeText.gameObject.SetActive(true);
    }


    void PlayTimeDown()
    {
        startWaitTime.gameObject.SetActive(false);

        playTime -= 1 * Time.deltaTime;
        curTime = playTime;

        playTimeText.text = $" Time : {Mathf.Round(playTime).ToString()}";

        if (Mathf.Round(playTime) <= 0)
        {
            playTime = 0;
            playTimeText.text = $" Time : {Mathf.Round(playTime).ToString()}";
            playTimeText.gameObject.SetActive(false);
            LosePanelSet(true);
        }
        else if (playTimeText.gameObject.active == false)
        {
            playTime = 0;
        }
    }

    void FirstSet(bool selectPanel)
    {
        inGameCanvas.gameObject.SetActive(true);
        SelectPanelSet(selectPanel);
        CharacterMoveSet(selectPanel);
        LineColorAndSizeChange(false);
        InGameSet(false);
        InGameOptionSet(false);
        WinPanelSet(false);
        LosePanelSet(false);
    }

    public void SelectPanelSet(bool active)
    {
        selectPanel.gameObject.SetActive(active);
    }

    public void InGameSet(bool active) => closePlayOneBrush.gameObject.SetActive(active);

    public void CharacterMoveSet(bool active) => characterMove.gameObject.SetActive(active);

    public void InGameOptionSet(bool active) => optionPanel.gameObject.SetActive(active);

    public void OneBrushPlayPanelSet(bool active) => onBushObj.gameObject.SetActive(active);

    public void LosePanelSet(bool active) => losePanel.gameObject.SetActive(active);

    public void WinPanelSet(bool active) => winPanel.gameObject.SetActive(active);

    public void LineColorAndSizeChange(bool active) => lineChangedPanel.gameObject.SetActive(active);

    void OnClickGoStartPanel() => SceneManager.LoadScene(SceneName.StartScene);

    public void OnClickOptionBut()
    {
        if (isOptionPanelOpen == false)
        {
            InGameOptionSet(true);
            OneBrushPlayPanelSet(false);
            InGameSet(false);
            Time.timeScale = 0;
            isOptionPanelOpen = true;
        }
        else if (isOptionPanelOpen == true)
        {
            InGameOptionSet(false);
            if (selectPanel.active == true)
            {
                OneBrushPlayPanelSet(false);
            }
            else
            {
                OneBrushPlayPanelSet(true);
                InGameSet(true);
            }

            Time.timeScale = 1;
            isOptionPanelOpen = false;
        }
    }
}