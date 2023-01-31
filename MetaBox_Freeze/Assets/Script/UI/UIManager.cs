using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject clearPanel = null;

    [SerializeField] Button option = null;
    [SerializeField] TextMeshProUGUI catchNumber = null;
    [SerializeField] TextMeshProUGUI gameClear = null;
    [SerializeField] CountDown countDown = null;

    //[SerializeField] ScrollRect wantedList = null;
    //[SerializeField] Wanted wantedListImage = null;
    //Dictionary<int, Wanted> wantedDic = new Dictionary<int, Wanted>();

    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;

    int wantedCount;
    int countdown;

    void Awake()
    {
        option.onClick.AddListener(OnClick_Option);
        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);

        optionPanel.SetActive(false);
        clearPanel.SetActive(false);
        catchNumber.gameObject.SetActive(false);
        gameClear.gameObject.SetActive(false);
        countDown.gameObject.SetActive(false);
    }
    
    public void DataSetting()
    {
        this.wantedCount = GameManager.Instance.StageDatas[GameManager.Instance.CurStage].wantedCount;
        this.countdown = GameManager.Instance.StageDatas[GameManager.Instance.CurStage].startCountdown;
        catchNumber.gameObject.SetActive(true);
        catchNumber.text = $"{GameManager.Instance.CatchNumber} / {wantedCount}";
    }

    /* [Obsolete]
    public void WantedListSetting(List<int> list)
    {
        wantedDic.Clear();
        wantedList.gameObject.SetActive(true);
        for (int i = 0; i < list.Count; i++)
        {
            Wanted inst = Instantiate<Wanted>(wantedListImage, wantedList.content.transform);
            inst.Init(list[i]);
            wantedDic.Add(list[i], inst);
            //범죄자 목록이 많을 경우 위 아래 투터치 드래그로 목록 변경 가능
        }
    }
    public void Catch(int id)
    {
        if (wantedDic.TryGetValue(id, out Wanted value)) value.Catch();
    }
    public void WaveClear()
    {
        for (int i = 0; i < wantedList.content.childCount; i++)
        {
            Destroy(wantedList.content.GetChild(i).gameObject);
        }
    }
    */

    public void Countdown()
    {
        option.gameObject.SetActive(false);
        StartCoroutine(nameof(RunCountdown));
    }

    IEnumerator RunCountdown()
    {
        countDown.gameObject.SetActive(true);

        for (int i = countdown; i > 0; i--)
        {
            countDown.Show(i);
            yield return wait1;
        }

        countDown.gameObject.SetActive(false);
        gameClear.gameObject.SetActive(true);
        gameClear.text = "Game Start";
        yield return waitHalf;
        gameClear.gameObject.SetActive(false);
        option.gameObject.SetActive(true);
    }

    public void Catch()
    {
        catchNumber.text = $"{GameManager.Instance.CatchNumber} / {wantedCount}";
    }
    public void Win()
    {
        clearPanel.SetActive(true);
    }

    public void Lose()
    {
        gameClear.gameObject.SetActive(true);
        gameClear.text = "You Lose";
    }

    void OnClick_Option()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        gameClear.gameObject.SetActive(false);
        optionPanel.SetActive(true);
    }
}
