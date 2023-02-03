using ObjectPoolCP;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //==============master canvas==================
    [Header("[Master UI]")]
    [SerializeField] GameObject UICanvas;
    
    [Header("[Vfx]")]
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
    [SerializeField] TextMeshProUGUI scoreResultText;
    [Space]
    [SerializeField] Button endRestartButton;
    [SerializeField] Button endExitButton;

    //================IngameProduction===============================
    [Header("[Production]")]
    [SerializeField] GameObject CountDownUI;
    [Space]
    [SerializeField] GameObject CountThreeImage;
    [SerializeField] GameObject CountTwoImage;
    [SerializeField] GameObject CountOneImage;
    [SerializeField] GameObject CountStartImage;
    [Space]
    [SerializeField] GameObject production;
    [SerializeField] GameObject viewHall;

    //=================caching========================================
    [Header("Score Production Value")]
    [SerializeField] float ScoreDelay;
    private WaitForSecondsRealtime waitSec;

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

        endRestartButton.onClick      .AddListener(() => SceneMove(SceneName.Main));
        endRestartButton.onClick      .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //========================Caching==============================================
        waitSec = new WaitForSecondsRealtime(ScoreDelay);


        //delegate chain
        EventReciver.ScoreModi     += UIScoreModi;
        EventReciver.CorrectIngred += UICorrectIngred;
        EventReciver.WrongIngred   += UIWrongIngred;

        EventReciver.SceneStart += SceneStartProd;

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


        EventReciver.SceneStart -= SceneStartProd;

        EventReciver.TickCount  -= UITcikCount;
        EventReciver.GameStart  -= UIGameStart;
        EventReciver.GamePause  -= UIGamePause;
        EventReciver.GameResume -= UIGameResume;
        EventReciver.GameOver   -= UIGameOver;
    }
    //============================================Initializing UI=======================================
    void UIInitializing()
    {
        timer.text = string.Format("{0:D2} : {1:D2} ", (int)(GameManager.Inst.countDown / 60f), (int)(GameManager.Inst.countDown % 60f));
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
        masterSlider.value = SoundManager.Inst.GetVolume("Master");
        bgmSlider.value = SoundManager.Inst.GetVolume("BGM");
        sfxSlider.value = SoundManager.Inst.GetVolume("SFX");
    }
    void UIGameResume()
    {
        ShowUI(inGameUI);
    }
    void UIGameOver()
    {
        ShowUI(gameOverUI);
        StartCoroutine(nameof(GameOverProd));
    }
    IEnumerator GameOverProd()
    {
        int tempScore = 0;

        while (tempScore < GameManager.Inst.Score)
        {
            tempScore++;
            scoreResultText.text = tempScore.ToString();

            yield return waitSec;
        }
        
        endExitButton.gameObject.SetActive(true);
        endRestartButton.gameObject.SetActive(true);
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
        score.text = (GameManager.Inst.Score + value).ToString();
    }
    void UITcikCount()
    {
        timer.text = string.Format("{0:D2} : {1:D2} ", (int)(GameManager.Inst.countDown / 60f), (int)(GameManager.Inst.countDown % 60f));
    }

    //===============================================UI Transition========================================
    void ShowUI(GameObject targetUIObj)
    {
        curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    //================================================SceneMove===========================================
    void SceneMove(string sceneName)
    {
        StartCoroutine(nameof(ViewHallShrink), sceneName);
    }
    IEnumerator ViewHallShrink(string sceneName)
    {
        production.SetActive(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x >= 0.8f)
        {
            timer += Time.fixedDeltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.zero, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.zero;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    //=============================================SceneProduction========================================
    void SceneStartProd()
    {
        StartCoroutine(nameof(CountDown));
    }
    IEnumerator CountDown()
    {
        production.SetActive(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x <= 45)
        {
            timer += Time.fixedDeltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 46f, timer);
            yield return null;
        }

        production.SetActive(false);

        CountDownUI.SetActive(true);
        yield return new WaitUntil(() => GameManager.Inst.count >= 2);
        CountThreeImage.SetActive(true);
        yield return new WaitUntil(() => GameManager.Inst.count >= 3);
        CountThreeImage.SetActive(false);
        CountTwoImage.SetActive(true);
        yield return new WaitUntil(() => GameManager.Inst.count >= 4);
        CountTwoImage.SetActive(false);
        CountOneImage.SetActive(true);
        yield return new WaitUntil(() => GameManager.Inst.count >= 5);
        CountOneImage.SetActive(false);
        CountStartImage.SetActive(true);
        yield return new WaitUntil(() => GameManager.Inst.count >= 6);

        CountDownUI.SetActive(false);
    }
}
