using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static private UIManager instance;
    static public UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(UIManager), typeof(UIManager)).GetComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    [Header("Panel Control")]
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject gameClearPanel = null;
    [SerializeField] GameObject gameFailPanel = null;

    [SerializeField] Button option = null;
    [SerializeField] CountDown countDown = null;
    [SerializeField] Image gameStart = null;

    [Header("StageClear Control")]
    [SerializeField] GameObject stageClear = null;
    [SerializeField] Slider stageNum = null;
    [SerializeField] TextMeshProUGUI stageCount = null;

    [SerializeField] Fade fade = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;

    void Awake()
    {
        option.onClick.AddListener(OnClick_Option);
        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);

        optionPanel.SetActive(false);
        gameClearPanel.SetActive(false);
        gameFailPanel.SetActive(false);


        countDown.gameObject.SetActive(false);
        gameStart.gameObject.SetActive(false);
        stageClear.SetActive(false);
    }

    public void DataSetting()
    {
        stageClear.SetActive(false);
    }

    public void Countdown()
    {
        StartCoroutine(nameof(RunCountdown));
    }

    IEnumerator RunCountdown()
    {
        countDown.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countDown.Show(i);
            yield return wait1;
        }

        countDown.gameObject.SetActive(false);
        gameStart.gameObject.SetActive(true);
        yield return waitHalf;
        gameStart.gameObject.SetActive(false);
        GameManager.Instance.hideEff?.Invoke();
    }

    public void WaveClear()
    {
        stageClear.SetActive(true);
        stageNum.value = (float)(GameManager.Instance.CurStage + 2) / (GameManager.Instance.StageDatas.Count);
        stageCount.text = $"{GameManager.Instance.CurStage + 2}/{GameManager.Instance.StageDatas.Count}";
        SoundManager.Instance.PlaySFX(SFX.WaveClear);
    }
    public void Win()
    {
        gameClearPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.GameClear);
    }

    public void Lose()
    {
        gameFailPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.Fail);
    }

    void OnClick_Option()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        optionPanel.SetActive(true);
    }

    public void OnClick_Quit() //move to start scene
    {
        GameManager.Instance.IsGaming = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
        fade.FadeOut(0);
    }
}
