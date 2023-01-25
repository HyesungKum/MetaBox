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

    [SerializeField] TextMeshProUGUI gameClear = null;
    [SerializeField] CountDown countDown = null;

    [SerializeField] ScrollRect wantedList = null;
    Dictionary<int, Wanted> wantedDic = new Dictionary<int, Wanted>();
    [SerializeField] Wanted wantedListImage = null;
    
    [SerializeField] Button option = null;
    [SerializeField] Button exit = null;
    

    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;
    public int PlayTime { get; set; }
    int wantedCount;
    int countdown;
    

    // Start is called before the first frame update
    void Start()
    {
        option.onClick.AddListener(OnClick_Option);
        exit.onClick.AddListener(delegate { OnClick_Exit(); });
        

        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);

        optionPanel.SetActive(false);

        gameClear.gameObject.SetActive(false);
        countDown.gameObject.SetActive(false);
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
        gameClear.gameObject.SetActive(true);
        gameClear.text = "You Win" + Environment.NewLine + $"{PlayTime - GameManager.Instance.PlayTime}";
        //모든 스테이지 클리어하는데 걸린 시간이 짧은 유저가 상위에 랭크
        //플레이어 ID, 게임 분류 아이디(각 게임 테이블의 gameGroup), 게임 난이도(각 게임 테이블의 id), 플레이타임(초로 환산), 랭킹을 달성한 날짜와 시간 을 랭킹 DB에 저장
        //만약 랭킹 DB에 플레이어 id가 있을 경우 현재 결과와 이전 결과를 비교해서 현재 결과가 더 짧을 경우 현재 결과로 변경

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
            countDown.Show(countdown);
            countdown--;
            yield return wait1;
        }

        countDown.gameObject.SetActive(false);
        gameClear.gameObject.SetActive(true);
        gameClear.text = "Game Start";
        yield return waitHalf;
        gameClear.gameObject.SetActive(false);
        GameManager.Instance.WaveStart();
    }

    void OnClick_Option()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        optionPanel.SetActive(true);
    }

    void OnClick_Exit()
    {
        Application.Quit();
    }
}
