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

    //current activation UI Object
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    //================in game ui===================
    [Header("[In Game UI]")]
    [SerializeField] GameObject inGameUI;
    [Space]
    [SerializeField] GameObject correctVfx;
    [SerializeField] GameObject wrongVfx;
    [Space]
    [SerializeField] TextMeshProUGUI timeLimitText;
    [SerializeField] TextMeshProUGUI scoreText;
    [Space]
    [SerializeField] Button optionButton;
    

    //=================option ui===================
    [Header("[Option UI]")]
    [SerializeField] GameObject optionUI;
    [Space]
    [SerializeField] Slider bgmSlider;
    [Space]
    [SerializeField] Slider sfxSlider;
    [Space]
    [SerializeField] Button opExitButton;
    [SerializeField] Button opRestartButton;
    [Space]
    [SerializeField] Button opResumeButton;

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
    [SerializeField] GameObject sceneProduction;
    [SerializeField] GameObject viewHall;
    [Space]
    [SerializeField] GameObject highScoreText;
    [SerializeField] float scoreProdDelay;

    //=================caching========================================
    private WaitUntil[] waitCount = new WaitUntil[5];

    private void Awake()
    {
        //in game button listener
        optionButton.onClick        .AddListener(() => EventReceiver.CallGamePause());
        optionButton.onClick        .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //option button listener
        bgmSlider.onValueChanged   .AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));

        sfxSlider.onValueChanged   .AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        opRestartButton.onClick    .AddListener(() => SceneMove(SceneName.Main));
        opRestartButton.onClick    .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        opExitButton.onClick       .AddListener(() => SceneMove(SceneName.Start));
        opExitButton.onClick       .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        opResumeButton.onClick     .AddListener(() => EventReceiver.CallGameResume());
        opResumeButton.onClick     .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //game end button listener
        endExitButton.onClick      .AddListener(() => SceneMove(SceneName.Start));
        endExitButton.onClick      .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        endRestartButton.onClick      .AddListener(() => SceneMove(SceneName.Main));
        endRestartButton.onClick      .AddListener(() => SoundManager.Inst.CallSfx("ButtonClick"));

        //delegate chain
        EventReceiver.ScoreModiR    += UIScoreModi;
        EventReceiver.ScoreModiL    += UIScoreModi;
        EventReceiver.CorrectIngred += UICorrectIngred;
        EventReceiver.WrongIngred   += UIWrongIngred;

        EventReceiver.SceneStart += SceneStartProd;

        EventReceiver.TickCount  += UITcikCount;
        EventReceiver.GameStart  += UIGameStart;
        EventReceiver.GamePause  += UIGamePause;
        EventReceiver.GameResume += UIGameResume;
        EventReceiver.GameOver   += UIGameOver;

        //caching
        waitCount[0] = new WaitUntil(() => GameManager.Inst.count >= 2);
        waitCount[1] = new WaitUntil(() => GameManager.Inst.count >= 3);
        waitCount[2] = new WaitUntil(() => GameManager.Inst.count >= 4);
        waitCount[3] = new WaitUntil(() => GameManager.Inst.count >= 5);
        waitCount[4] = new WaitUntil(() => GameManager.Inst.count >= 6);
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
        EventReceiver.ScoreModiR    -= UIScoreModi;
        EventReceiver.ScoreModiL    -= UIScoreModi;
        EventReceiver.CorrectIngred -= UICorrectIngred;
        EventReceiver.WrongIngred   -= UIWrongIngred;

        EventReceiver.SceneStart -= SceneStartProd;

        EventReceiver.TickCount  -= UITcikCount;
        EventReceiver.GameStart  -= UIGameStart;
        EventReceiver.GamePause  -= UIGamePause;
        EventReceiver.GameResume -= UIGameResume;
        EventReceiver.GameOver   -= UIGameOver;
    }
    //============================================Initializing UI=======================================
    void UIInitializing()
    {
        timeLimitText.text = string.Format("{0:D2} : {1:D2} ", (int)(GameManager.Inst.GetCountDown() / 60f), (int)(GameManager.Inst.GetCountDown() % 60f));
        scoreProdDelay = scoreProdDelay == 0 ? 0.1f : scoreProdDelay;
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
        //production score
        int targetScore = GameManager.Inst.GetScore();
        int tempScore = 0;
        float lerpCount = 0f;

        //score increase production
        while (tempScore != targetScore)
        {
            lerpCount += Time.fixedDeltaTime * scoreProdDelay;

            tempScore = (int)Mathf.Lerp(tempScore, targetScore, lerpCount);
            scoreResultText.text = tempScore.ToString();

            yield return null;
        }

        //end sfx out
        SoundManager.Inst.SetBGM("StageClear");
        SoundManager.Inst.SetBGMUnLoop();
        SoundManager.Inst.PlayBGM();

        //UI popup
        if (GameManager.Inst.IsHighScore) highScoreText.SetActive(true);
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
        scoreText.text = (GameManager.Inst.GetScore() + value).ToString();
    }
    void UITcikCount()
    {
        timeLimitText.text = string.Format("{0:D2} : {1:D2} ", (int)(GameManager.Inst.GetCountDown() / 60f), (int)(GameManager.Inst.GetCountDown() % 60f));
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
        StartCoroutine(nameof(EndProduction), sceneName);
    }
    /// <summary>
    /// View Hall Shrink Production
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator EndProduction(string sceneName)
    {
        sceneProduction.SetActive(true);

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
        StartCoroutine(nameof(StartProduction));
    }
    IEnumerator StartProduction()
    {
        sceneProduction.SetActive(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x <= 45)
        {
            timer += Time.fixedDeltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 46f, timer);
            yield return null;
        }

        sceneProduction.SetActive(false);

        CountDownUI.SetActive(true);
        yield return waitCount[0];

        CountThreeImage.SetActive(true);
        yield return waitCount[1];

        CountThreeImage.SetActive(false);
        CountTwoImage.SetActive(true);
        yield return waitCount[2];

        CountTwoImage.SetActive(false);
        CountOneImage.SetActive(true);
        yield return waitCount[3];

        CountOneImage.SetActive(false);
        CountStartImage.SetActive(true);
        yield return waitCount[4];

        CountDownUI.SetActive(false);
    }
}
