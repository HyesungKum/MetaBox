using ObjectPoolCP;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Button Event")]
    [SerializeField] GameObject touchEff = null;

    WaitForSeconds waitHalf = new WaitForSeconds(0.5f);
    WaitForSeconds wait1 = new WaitForSeconds(1f);

    void Awake()
    {
        option.onClick.AddListener(OnClick_Option);

        optionPanel.SetActive(false);
        gameClearPanel.SetActive(false);
        gameFailPanel.SetActive(false);

        countDown.gameObject.SetActive(false);
        gameStart.gameObject.SetActive(false);
        stageClear.SetActive(false);

        AddButtonListener();
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

        yield return wait1;
        yield return wait1;

        if (effect != null) PoolCp.Inst.DestoryObjectCp(effect);
    }

    GameObject[] GetSceneRootObject()
    {
        Scene curscene = SceneManager.GetActiveScene();
        return curscene.GetRootGameObjects();
    }

    #endregion

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
