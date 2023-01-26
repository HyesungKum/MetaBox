using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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

    [SerializeField] TextMeshProUGUI gameClear = null;
    [SerializeField] TextMeshProUGUI clearPlayTime = null; 
    [SerializeField] CountDown countDown = null;

    [SerializeField] ScrollRect wantedList = null;
    Dictionary<int, Wanted> wantedDic = new Dictionary<int, Wanted>();
    [SerializeField] Wanted wantedListImage = null;
    
    [SerializeField] Button option = null;
    
    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;
    public int PlayTime { get; set; }
    int wantedCount;
    int countdown;

    private void Awake()
    {
        GameManager.Instance.FreezeDataSetting += () => PlayTime = GameManager.Instance.FreezeData.playTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        option.onClick.AddListener(OnClick_Option);
        option.interactable = false;
        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);

        optionPanel.SetActive(false);
        clearPanel.SetActive(false);
        gameClear.gameObject.SetActive(false);
        countDown.gameObject.SetActive(false);
        StartCoroutine(nameof(GameStart));
    }

    IEnumerator GameStart()
    {
        yield return wait1;
        GameManager.Instance.PlayTimerEvent();
        WaveStart();
    }
    public void DataSetting(int wantedCount, int startCountdown)
    {
        this.wantedCount = wantedCount;
        this.countdown = startCountdown;
    }

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
        Wanted value = null;
        if (wantedDic.TryGetValue(id, out value)) value.Catch();
    }
    public void Arrest(int id)
    {
        Wanted value = null;
        if(wantedDic.TryGetValue(id, out value)) value.Arrest();
    }

    public void WaveStart()
    {
        option.interactable = false;
        this.countdown = 3;
        GameManager.Instance.reStart = false;
        StartCoroutine(nameof(RunCountdown));
    }

    public void WaveClear()
    {
        for(int i = 0; i < wantedList.content.childCount; i++)
        {
            Destroy(wantedList.content.GetChild(i).gameObject);
        }
    }

    public void Win()
    {
        clearPanel.SetActive(true);
        clearPlayTime.text = (PlayTime - GameManager.Instance.PlayTime).ToString();
    }

    public void Lose()
    {
        gameClear.gameObject.SetActive(true);
        gameClear.text = "You Lose";
    }

    IEnumerator RunCountdown()
    {
        countDown.gameObject.SetActive(true);

        for(int i = countdown; i > 0; i--)
        {
            if (GameManager.Instance.reStart) yield break;
            countDown.Show(countdown);
            countdown--;
            yield return wait1;
            if(countDown.nomal == false) yield break;
        }

        if (GameManager.Instance.reStart) yield break;
        countDown.gameObject.SetActive(false);
        gameClear.gameObject.SetActive(true);
        gameClear.text = "Game Start";
        yield return waitHalf;
        if (GameManager.Instance.reStart) yield break;
        gameClear.gameObject.SetActive(false);
        option.interactable = true;
        GameManager.Instance.WaveStart();
    }

    void OnClick_Option()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        optionPanel.SetActive(true);
    }

}
