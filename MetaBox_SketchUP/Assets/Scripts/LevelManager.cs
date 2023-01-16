using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager instance;
    public static LevelManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(LevelManager), typeof(LevelManager)).GetComponent<LevelManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("[Start Panel]")]
    [SerializeField] GameObject StartPanel = null;
    [SerializeField] Button gameStart = null;
    [SerializeField] Button opTion = null;
    [SerializeField] Button gotoTown = null;
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";
    [SerializeField] Button Exit = null;

    [Header("[Level Panel]")]
    [SerializeField] GameObject LevelPanel = null;
    [SerializeField] Button levelone = null;
    [SerializeField] Button leveltwo = null;
    [SerializeField] Button levelthree = null;
    [SerializeField] Button levelfour = null;

    [Header("[Select Panel]")]
    [SerializeField] GameObject stagePanel = null;
    [SerializeField] Button imageOne = null;
    [SerializeField] Button imageTwo = null;
    [SerializeField] Button imageThree = null;

    [Header("[InGame Timer]")]
    [SerializeField] GameObject inGameTime = null;
    [SerializeField] GameObject waitTime = null;
    [SerializeField] TextMeshProUGUI waitTimes = null;
    [SerializeField] GameObject Timers = null;

    [Header("[InGame Panel]")]
    [SerializeField] GameObject inGamePanel = null;
    [SerializeField] GameObject ImgOneObj = null;
    [SerializeField] GameObject ImgTwoObj = null;
    [SerializeField] GameObject ImgThreeObj = null;
    [SerializeField] RectTransform prantTransform = null;

    [SerializeField] GameObject clearPanel = null;

    void Awake()
    {
        PanelSetting(true, false, false, false, false);
        InGamepanelObjSetting(false, false, false);
    }

    void Start()
    {
        // Button OnClick Event Setting
        gameStart.onClick.AddListener(() => OnClickStartBut());
        gotoTown.onClick.AddListener(() => MoveTown(mainPackName));
        Exit.onClick.AddListener(() => AppQuit());

        // LevelButton Event Setting
        levelone.onClick.AddListener(() => OnClickLevelBut());
        //leveltwo.onClick.AddListener(() => );
        //levelthree.onClick.AddListener(() => );
        //levelfour.onClick.AddListener(() => );

        // =============
        imageOne.onClick.AddListener(() => OnClickImageOne());
        imageTwo.onClick.AddListener(() => OnClickImageTwo());
        imageThree.onClick.AddListener(() => OnClickImageThree());
    }

    void OnClickStartBut()
    {
        PanelSetting(false, true, false, false, false);
    }

    void OnClickLevelBut()
    {
        PanelSetting(false, false, true, false, false);
    }

    void OnClickImageOne()
    {
        PanelSetting(false, false, false, true, true);
        // Obj setting
        StartCoroutine(TimeDelayImgOne(ImgOneObj, true));
        //SelectPanelSetting(false, true, true);
    }

    void OnClickImageTwo()
    {
        PanelSetting(false, false, false, true, true);
        //SelectPanelSetting(true, false, true);
        // Obj setting
        StartCoroutine(TimeDelayImgOne(ImgTwoObj, true));
    }


    void OnClickImageThree()
    {
        PanelSetting(false, false, false, true, true);
        //SelectPanelSetting(true, true, false);
        // Obj setting
        StartCoroutine(TimeDelayImgOne(ImgThreeObj, true));
    }

    void SelectPanelSetting(bool imgOneActive, bool imgTwoActive, bool imgThreeActive)
    {
        imageOne.gameObject.SetActive(imgOneActive);
        imageTwo.gameObject.SetActive(imgTwoActive);
        imageThree.gameObject.SetActive(imgThreeActive);
    }

    void InGamepanelObjSetting(bool objOne, bool ObjTwo, bool ObjThree)
    {
        ImgOneObj.SetActive(false);
        ImgTwoObj.SetActive(false);
        ImgThreeObj.SetActive(false);
    }

    public void SelectAgain(int Count)
    {
        stagePanel.gameObject.SetActive(true);

        if (Count == 1)
        {
            stagePanel.gameObject.SetActive(true);
            Timers.gameObject.SetActive(false);
            imageTwo.onClick.RemoveListener(() => OnClickImageTwo());

        }
        else if (Count == 2)
        {
            stagePanel.gameObject.SetActive(true);
            Timers.gameObject.SetActive(false);
        }
        else if (Count == 3)
        {
            stagePanel.gameObject.SetActive(true);
            Timers.gameObject.SetActive(false);

        }
    }


    /// <summary>
    /// ImgeObj One , ImgeObj Two, ImgeObj Three Setting and 3Seconds waiting
    /// </summary>
    /// <param name="objOne"></param>
    /// <param name="objTwo"></param>
    /// <param name="objThree"></param>
    IEnumerator TimeDelayImgOne(GameObject Insobj, bool isTrue)
    {
        waitTime.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        Insobj.gameObject.SetActive(isTrue);
        Timers.gameObject.SetActive(true);
        //Debug.Log("## 코루틴 시간 지연 : 3초 실행 됨 ??");
    }

    /// <summary>
    /// StartPanel.LevelPanel,stagePanel,inGamePanel Setting
    /// </summary>
    /// <param name="startPanels"></param>
    /// <param name="LevelPanels"></param>
    /// <param name="StatgePanels"></param>
    /// <param name="IngamePanels"></param>
    void PanelSetting(bool startPanels, bool LevelPanels, bool StatgePanels, bool IngameTime, bool inGame)
    {
        StartPanel.gameObject.SetActive(startPanels);
        LevelPanel.gameObject.SetActive(LevelPanels);
        stagePanel.gameObject.SetActive(StatgePanels);
        inGameTime.gameObject.SetActive(IngameTime);
        inGamePanel.gameObject.SetActive(inGame);
    }

    void MoveTown(string pakageName)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", pakageName);

        jo.Call("startActivity", intent);
    }

    private void AppQuit()
    {
        Application.Quit();
    }
}
