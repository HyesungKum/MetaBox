using Newtonsoft.Json;
using ObjectPoolCP;
using System.Collections;
using System.IO;
using ToolKum.AppTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct UserData
{
    public string id;
    public int charIndex;
    public bool troughTown;
}

public class StartManager : MonoBehaviour
{
    public static UserData curUserData = new();
    public static int MyLevel { get; private set; } = 0;


    [Header("Panel Control")]
    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject tutorialPanel = null;
    [SerializeField] GameObject exitPanel = null;
    [SerializeField] Fade fade = null; //for scene transitions

    [Header("Start Panel Button")]
    [SerializeField] Button start = null;
    [SerializeField] Button option = null;
    [SerializeField] Button tutorial = null;
    [SerializeField] Button exit = null;

    [Header("Level Panel Button")]
    [SerializeField] Button back = null; //activate start panel
    [SerializeField] Button easy = null;
    [SerializeField] Button normal = null;
    [SerializeField] Button hard = null;
    [SerializeField] Button extreme = null;

    [Header("Tutorial Panel Button")]
    [SerializeField] Button back_Start = null;

    [Header("Exit Panel Button")]
    [SerializeField] Button quit = null;
    [SerializeField] Button back_Game = null;

    [Header("Button Event")]
    [SerializeField] GameObject touchEff = null;
    WaitForSeconds playEff = new WaitForSeconds(2f);

    [Header("[Application Setting]")]
    [SerializeField] string mainPackName = "com.MetaBox.DreamCatcher";

#if UNITY_EDITOR
    [SerializeField] private string localSavePath = "/MetaBox/SaveData/SaveData.json";
#else
    private string localSavePath = "/storage/emulated/0/MetaBox/SaveData/SaveData.json";
#endif

    void Awake()
    {
        SetPanel(true);
        optionPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        exitPanel.SetActive(false);

        start.onClick.AddListener(() => SetPanel(false));
        option.onClick.AddListener(() => optionPanel.SetActive(true));
        tutorial.onClick.AddListener(() => tutorialPanel.SetActive(true)); //ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main")
        exit.onClick.AddListener(() => startPanel.SetActive(false));
        exit.onClick.AddListener(() => exitPanel.SetActive(true));

        back.onClick.AddListener(() => SetPanel(true));
        easy.onClick.AddListener(OnClick_Easy);
        normal.onClick.AddListener(OnClick_Normal);
        hard.onClick.AddListener(OnClick_Hard);
        extreme.onClick.AddListener(OnClick_Extreme);

        back_Start.onClick.AddListener(() => tutorialPanel.SetActive(false));

        quit.onClick.AddListener(() => AppQuit());
        back_Game.onClick.AddListener(() => startPanel.SetActive(true));
        back_Game.onClick.AddListener(() => exitPanel.SetActive(false));

        AddButtonListener();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(0);
        FileCheck();
    }

    private void FileCheck()
    {
        if (File.Exists(localSavePath))
        {
            curUserData = ReadSaveData(localSavePath);
        }
        else //존재하지 않을때 
        {
            curUserData.id = "New";
            curUserData.charIndex = 0;
            curUserData.troughTown = false;
        }
    }
    private UserData ReadSaveData(string path)
    {
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

        return readData;
    }


    #region Button
    public void AddButtonListener()
    {
        GameObject[] rootObj = GetSceneRootObject();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject go = (GameObject)rootObj[i] as GameObject;
            Component[] buttons = go.transform.GetComponentsInChildren(typeof(Button), true);
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate { OnClickButton(button.transform.position); });
                button.onClick.AddListener(() => SoundManager.Instance.PlaySFX(SFX.Button));
            }
        }
    }

    void OnClickButton(Vector3 pos)
    {
        StartCoroutine(TouchEff(pos));
    }

    IEnumerator TouchEff(Vector3 movepoint)
    {
        GameObject effect = PoolCp.Inst.BringObjectCp(touchEff);
        effect.transform.position = movepoint;

        yield return playEff;

        if (effect != null) PoolCp.Inst.DestoryObjectCp(effect);
    }

    GameObject[] GetSceneRootObject()
    {
        Scene curscene = SceneManager.GetActiveScene();
        return curscene.GetRootGameObjects();
    }

    #endregion


    void SetPanel(bool panel)
    {
        startPanel.SetActive(panel);
        levelPanel.SetActive(!panel);
    }

    void OnClick_Easy()
    {
        MyLevel = 1;
        fade.FadeOut(1);
    }
    void OnClick_Normal()
    {
        MyLevel = 2;
        fade.FadeOut(1);
    }
    void OnClick_Hard()
    {
        MyLevel = 3;
        fade.FadeOut(1);
    }
    void OnClick_Extreme()
    {
        MyLevel = 4;
        fade.FadeOut(1);
    }

    void AppQuit()
    {
        if (curUserData.troughTown) AppTrans.MoveScene(mainPackName);
        else Application.Quit();
    }
}

