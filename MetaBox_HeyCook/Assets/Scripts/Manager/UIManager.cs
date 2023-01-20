using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using ObjectPoolCP;

public class UIManager : MonoBehaviour
{
    [Header("GameManager Reference")]
    [SerializeField] GameManager gameManager;
    //==============master canvas==================
    [Header("Master UI")]
    [SerializeField] GameObject UICanvas;
    
    [Header("Vfx")]
    [SerializeField] GameObject correctVfx;
    [SerializeField] GameObject wrongVfx;

    //current activation UI Object
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    //================in game ui===================
    [Header("[In Game UI]")]
    [SerializeField] GameObject inGameUI;
    [Space]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score;
    [Space]
    [SerializeField] Button optionButton;

    //=================option ui===================
    [Header("[Option UI]")]
    [SerializeField] GameObject optionUI;
    [Space]
    [SerializeField] Button masterSoundButton;
    [SerializeField] Slider masterSlider;
    [Space]
    [SerializeField] Button bgmSoundButton;
    [SerializeField] Slider bgmSlider;
    [Space]
    [SerializeField] Button sfxSoundButton;
    [SerializeField] Slider sfxSlider;
    [Space]
    [SerializeField] Button opRestartButton;
    [SerializeField] Button opResumeButton;
    [Space]
    [SerializeField] Button opExitButton;

    //=================end game ui=================
    [Header("[Game Over UI]")]
    [SerializeField] GameObject gameOverUI;
    [Space]
    [SerializeField] Button restartButton;
    [SerializeField] Button endExitButton;

    private void Awake()
    {
        //===============in game button listener=======================================
        optionButton.onClick        .AddListener(() => EventReciver.CallGamePause());
        optionButton.onClick        .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //===============option button listener========================================
        opRestartButton.onClick    .AddListener(() => SceneMove(SceneName.Main));
        opRestartButton.onClick    .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        masterSoundButton.onClick  .AddListener(() => SoundManager.Inst.ToggleControll("Master", masterSlider.value));
        masterSoundButton.onClick  .AddListener(() => ToggleSlider(masterSlider));
        masterSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("Master", call));

        bgmSoundButton.onClick     .AddListener(() => SoundManager.Inst.ToggleControll("BGM", bgmSlider.value));
        bgmSoundButton.onClick     .AddListener(() => ToggleSlider(bgmSlider));
        bgmSlider.onValueChanged   .AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));

        sfxSoundButton.onClick     .AddListener(() => SoundManager.Inst.ToggleControll("SFX", sfxSlider.value));
        sfxSoundButton.onClick     .AddListener(() => ToggleSlider(sfxSlider));
        sfxSlider.onValueChanged   .AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        opResumeButton.onClick     .AddListener(() => EventReciver.CallGameResume());
        opResumeButton.onClick     .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        opExitButton.onClick       .AddListener(() => SceneMove(SceneName.Start));
        opExitButton.onClick       .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //===============game end button listener======================================
        endExitButton.onClick      .AddListener(() => SceneMove(SceneName.Start));
        endExitButton.onClick      .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        restartButton.onClick      .AddListener(() => SceneMove(SceneName.Main));
        restartButton.onClick      .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //delegate chain
        EventReciver.ScoreModi     += UIScoreModi;
        EventReciver.CorrectIngred += UICorrectIngred;
        EventReciver.WrongIngred   += UIWrongIngred;

        EventReciver.TickCount  += UITcikCount;
        EventReciver.GameStart  += UIGameStart;
        EventReciver.GamePause  += UIGamePause;
        EventReciver.GameResume += UIGameResume;
        EventReciver.GameOver   += UIGameOver;
    }
    private void Start()
    {
        //Init pool
        GameObject _correctVfx = Instantiate(correctVfx);
        PoolCp.Inst.DestoryObjectCp(_correctVfx);
        GameObject _wrongVfx = Instantiate(wrongVfx);
        PoolCp.Inst.DestoryObjectCp(_wrongVfx);

        //Init UI
        UIInitializing();
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.ScoreModi     -= UIScoreModi;
        EventReciver.CorrectIngred -= UICorrectIngred;
        EventReciver.WrongIngred   -= UIWrongIngred;

        EventReciver.TickCount  -= UITcikCount;
        EventReciver.GameStart  -= UIGameStart;
        EventReciver.GamePause  -= UIGamePause;
        EventReciver.GameResume -= UIGameResume;
        EventReciver.GameOver   -= UIGameOver;
    }
    //============================================Initializing UI=======================================
    void UIInitializing()
    {
        optionButton.interactable = false;
    }

    //=============================================In Game Production===================================
    void UICorrectIngred(Vector3 pos)
    {
         GameObject instText = PoolCp.Inst.BringObjectCp(correctVfx);
         instText.transform.SetParent(UICanvas.transform, false);
         instText.transform.position = pos;
    }
    void UIWrongIngred(Vector3 pos)
    {
        GameObject instText = PoolCp.Inst.BringObjectCp(wrongVfx);
        instText.transform.SetParent(UICanvas.transform, false);
        instText.transform.position = pos;
    }

    //=============================================UI Viewing Controll==================================
    void UIGameStart()
    {
        optionButton.interactable = true;
    }
    void UIGamePause()
    {
        ShowUI(optionUI);
    }
    void UIGameResume()
    {
        ShowUI(inGameUI);
    }
    void UIGameOver()
    {
        ShowUI(gameOverUI);
    }
    void ShowUI(GameObject targetUIObj)
    {
        curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    //=============================================UI Value Controll======================================
    void ToggleSlider(Slider target)
    {
        target.interactable = target.interactable == true ? false : true;
    }
    /// <summary>
    /// modify ui score text controll
    /// </summary>
    /// <param name="value">target time value</param>
    void UIScoreModi(int value)
    {
        score.text = gameManager.Score.ToString();
    }
    void UITcikCount()
    {
        timer.text = string.Format("{0:D2} : {1:D2} ", (int)(gameManager.Timer / 60f), (int)(gameManager.Timer % 60f));
    }

    //================================================SceneMove============================================
    void SceneMove(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
