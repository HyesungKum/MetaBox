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
    [SerializeField] GameObject inGameOther = null;
    [SerializeField] GameObject losePanel = null;
    [SerializeField] GameObject winPanel = null;
    [SerializeField] GameObject optionPanel = null;

    [SerializeField] GameObject onBushObj = null;
    [SerializeField] GameObject clearImgPanel = null;

    [Header("[ClearImg Set]")]
    [SerializeField] GameObject ImgOne = null;
    [SerializeField] GameObject ImgTwo = null;
    [SerializeField] GameObject ImgThree = null;

    [Header("[ReStart]")]
    [SerializeField] Button loseReStartBut = null;
    [SerializeField] Button winReStartBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button inGameOptionCloseBut = null;

    [Header("[Timer Text]")]
    [SerializeField] TextMeshProUGUI playTimeText = null;
    [SerializeField] TextMeshProUGUI startWaitTime = null;
    #endregion

    // === wait 3 seconds ===
    //float waitTime = 3f;
    float waitTime = 1f;
    bool wait = false;

    // === play time total 10 seconds === 
    float playTime = 600;
    float curTime = 0;
    // === test ===
    //float playTime = 6;

    bool isOptionPanelOpen = false;
    bool isTimeStop = false;

    void Awake()
    {
        startWaitTime.text = $"Start";
        // === changed (true) ===
        startWaitTime.gameObject.SetActive(true);
        playTimeText.gameObject.SetActive(false);

        // === changed (false) ===
        FirstSetting(false);

        OneBrushSet(false);
        ClearPanelSet(false);

        // === button event Set ===
        #region
        loseReStartBut.onClick.AddListener(() => OnClickReStartPanel());
        winReStartBut.onClick.AddListener(() => OnClickReStartPanel());
        optionBut.onClick.AddListener(() => OnClickOptionBut());
        inGameOptionCloseBut.onClick.AddListener(() => InGameOptionCloseBut());
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
            FirstSetting(true);
            playTimeText.gameObject.SetActive(true);
            return;
        }
    }

    void PlayTimeDown()
    {
        playTime -= 1 * Time.deltaTime;
        curTime = playTime;

        playTimeText.text = $" Time : {Mathf.Round(playTime).ToString()}";
        //Debug.Log("## time : " + playTime);

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
            Debug.Log("Timer ºñÈ°¼ºÈ­  µÊ ? :" + playTime);
        }
    }

    void FirstSetting(bool selectPanel)
    {
        inGameCanvas.gameObject.SetActive(true);
        SelectPanelSet(selectPanel);
        InGameSet(false);
        InGameOptionSet(false);
        LosePanelSet(false);
    }

    public void SelectPanelSet(bool active)
    {
        selectPanel.gameObject.SetActive(active);
    }

    public void InGameSet(bool active)
    {
        inGameOther.gameObject.SetActive(active);
    }

    public void InGameOptionSet(bool active)
    {
        optionPanel.gameObject.SetActive(active);
    }

    public void OneBrushSet(bool active)
    {
        onBushObj.gameObject.SetActive(active);
    }

    public void LosePanelSet(bool active)
    {
        losePanel.gameObject.SetActive(active);
    }

    public void ClearPanelSet(bool active)
    {
        clearImgPanel.gameObject.SetActive(active);
    }
    public void ClearImgSet(bool oneSet, bool twoSet, bool threeSet)
    {
        ImgOne.gameObject.SetActive(oneSet);
        ImgTwo.gameObject.SetActive(twoSet);
        ImgThree.gameObject.SetActive(threeSet);
    }

    public void WinPanelSet(bool active)
    {
        winPanel.gameObject.SetActive(active);
    }

    void OnClickOptionBut()
    {
        if (isOptionPanelOpen == false)
        {
            InGameOptionSet(true);
            isOptionPanelOpen = true;
        }
        else if (isOptionPanelOpen == true)
        {
            InGameOptionSet(false);
            isOptionPanelOpen = false;
        }

    }

    void InGameOptionCloseBut()
    {
        optionPanel.gameObject.SetActive(false);
    }

    void OnClickReStartPanel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
