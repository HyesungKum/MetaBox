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

    [Header("[InGame Panel Set]")]
    [SerializeField] GameObject inGameCanvas = null;
    [SerializeField] GameObject selectPanel = null;
    [SerializeField] GameObject inGameOther = null;
    [SerializeField] GameObject losePanel = null;
    [SerializeField] GameObject winPanel = null;
    [SerializeField] GameObject optionPanel = null;

    [SerializeField] GameObject onBushObj = null;
    [SerializeField] GameObject clearImgPanel = null;

    [Header("ClearImg Set")]
    [SerializeField] GameObject ImgOne = null;
    [SerializeField] GameObject ImgTwo = null;
    [SerializeField] GameObject ImgThree = null;

    [Header("[ReStart]")]
    [SerializeField] Button loseReStartBut = null;
    [SerializeField] Button winReStartBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button inGameCloseBut = null;

    bool isOptionPanelOpen = false;

    void Awake()
    {
        inGameCanvas.gameObject.SetActive(true);
        FirstSetting();
        OneBrushSet(false);
        ClearPanelSet(false);

        loseReStartBut.onClick.AddListener(() => OnClickReStartPanel());
        winReStartBut.onClick.AddListener(() => OnClickReStartPanel());
        optionBut.onClick.AddListener(() => OptionPanelSet());
        inGameCloseBut.onClick.AddListener(() => InGameOptionCloseBut());
    }

    void FirstSetting()
    {
        SelectPanelSet(true);
        InGameSet(false);
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

    public void WinPanelSet(bool active)
    {
        winPanel.gameObject.SetActive(active);
    }

    public void ClearImgSet(bool oneSet, bool twoSet, bool threeSet)
    {
        ImgOne.gameObject.SetActive(oneSet);
        ImgTwo.gameObject.SetActive(twoSet);
        ImgThree.gameObject.SetActive(threeSet);
    }

    void OptionPanelSet()
    {
        optionPanel.gameObject.SetActive(true);
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
