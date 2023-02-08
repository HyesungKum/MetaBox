using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("[Timer Text]")]
    [SerializeField] TextMeshProUGUI playTimeText = null;
    [SerializeField] TextMeshProUGUI startWaitTime = null;

    #endregion

    // === wait 3 seconds ===
    //float waitTime = 3f;
    float waitTime = 1f;
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
        Time.timeScale = 1;
        startWaitTime.text = $"Start";
        // === changed (true) ===
        startWaitTime.gameObject.SetActive(true);
        playTimeText.gameObject.SetActive(false);

        // === changed (false) ===
        FirstSet(false);
        OneBrushPlayPanelSet(false);
        //ClearPanelSet(false);

        // === button event Set ===
        #region
        loseReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.FailSFXPlay(); });
        winReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.ClearSFXPlay(); });
        optionBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
        resumeBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
        quitBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.ButtonSFXPlay(); });
        #endregion
    }

    void Update()
    {
        #region Timer Setting
        if (startWaitTime.gameObject.active == true)
        {
            SetWaitTime();
        }
        else if (playTimeText.gameObject.active == true)
        {
            PlayTimeDown();
        }
        #endregion
    }

    void SetWaitTime()
    {
        if (waitTime > 0)
        {
            waitTime -= 1 * Time.deltaTime;
            startWaitTime.text = $"Time : {Mathf.Round(waitTime).ToString()}";

            if (Mathf.Round(waitTime) == 0)
            {
                startWaitTime.text = "Go".ToString();
            }
        }
        else if (waitTime < 0)
        {
            startWaitTime.gameObject.SetActive(false);
            FirstSet(true);
            InGameSet(false);
            playTimeText.gameObject.SetActive(true);
            return;
        }
    }

    void PlayTimeDown()
    {
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