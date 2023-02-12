using System.Collections;
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

    [Header("[Wait Time Img]")]
    [SerializeField] GameObject waitTimeObjs = null;

    [Header("[Timer Text]")]
    [SerializeField] TextMeshProUGUI playTimeText = null;
    [SerializeField] GameObject clockPrefab = null;

    [Header("[Character Move]")]
    [SerializeField] GameObject characterMove = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds waitOnSceonds = null;
    #endregion

    GameObject instClock = null;

    // === One Level play time total 10 seconds === 
    float seconds;
    int minute;

    // === ClearCount ===
    private int clearCount = 3;
    public int ClearCount { get { return clearCount; } set { clearCount = value; } }

    bool isOptionPanelOpen = false;

    void Awake()
    {
        waitHalf = new WaitForSeconds(0.5f);
        waitOnSceonds = new WaitForSeconds(1f);

        Time.timeScale = 1;

        minute = 9;
        seconds = 60;

        // === changed (true) ===
        waitTimeObjs.gameObject.SetActive(true);
        StartCoroutine(CountDowns());
        playTimeText.gameObject.SetActive(false);

        // === changed (false) ===
        FirstSet(false);
        optionBut.gameObject.SetActive(false);
        OneBrushPlayPanelSet(false);

        // === button event Set ===
        #region
        loseReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.GameLoseSFXPlay(); });
        winReStartBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.GameClearSFXPlay(); });
        quitBut.onClick.AddListener(delegate { OnClickGoStartPanel(); SoundManager.Inst.ButtonSFXPlay(); });
        optionBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
        resumeBut.onClick.AddListener(delegate { OnClickOptionBut(); SoundManager.Inst.ButtonSFXPlay(); });
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
        for (int i = waitTime; i > 0; i--)
        {
            countDown.ShowWaitTime(waitTime);
            waitTime--;
            yield return waitOnSceonds;
        }

        waitTimeObjs.gameObject.SetActive(false);
        yield return waitHalf;
        FirstSet(true);
        optionBut.gameObject.SetActive(true);
        playTimeText.gameObject.SetActive(true);
    }

    void PlayTimeDown()
    {
        seconds -= 1 * Time.deltaTime;
        playTimeText.text = string.Format("{0:D2} : {1:D2}", minute, (int)seconds);

        if ((int)seconds < 0)
        {
            seconds = 59;
            minute -= 1;
        }

        if (minute == 0 && (int)seconds == 30)
        {
            if (instClock == null)
                instClock = ObjectPoolCP.PoolCp.Inst.BringObjectCp(clockPrefab); // 여러개 생성 안되게 막기 코루틴으로 바꾸던가
        }
        if (minute == 0 && (int)seconds == 29)
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instClock);

        if(clearCount == 0)
        {
            Time.timeScale = 0;
            LineColorAndSizeChange(false);
            OneBrushPlayPanelSet(false);
            WinPanelSet(true);
        }
        else if (minute == 0 && (int)seconds <= 0)
        {
            playTimeText.text = $"00 : 00";
            playTimeText.gameObject.SetActive(false);
            OneBrushPlayPanelSet(false);
            LineColorAndSizeChange(false);
            LosePanelSet(true);
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

    public void SelectPanelSet(bool active) => selectPanel.gameObject.SetActive(active);

    public void InGameSet(bool active) => closePlayOneBrush.gameObject.SetActive(active);

    public void CharacterMoveSet(bool active) => characterMove.gameObject.SetActive(active);

    public void InGameOptionSet(bool active) => optionPanel.gameObject.SetActive(active);

    public void OneBrushPlayPanelSet(bool active) => onBushObj.gameObject.SetActive(active);

    public void LosePanelSet(bool active) => losePanel.gameObject.SetActive(active);

    public void WinPanelSet(bool active) => winPanel.gameObject.SetActive(active);

    public void LineColorAndSizeChange(bool active) => lineChangedPanel.gameObject.SetActive(active);

    public void OnClickGoStartPanel() => SceneManager.LoadScene(SceneName.StartScene);

    public void OnClickOptionBut()
    {
        if (isOptionPanelOpen == false)
        {
            InGameOptionSet(true);
            OneBrushPlayPanelSet(false);
            //SelectPanelSet(false);
            InGameSet(false);
            LineColorAndSizeChange(false);
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
                LineColorAndSizeChange(true);
                InGameSet(true);
            }

            Time.timeScale = 1;
            isOptionPanelOpen = false;
        }
    }
}