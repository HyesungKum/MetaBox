using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBackFunction();
public delegate void CallBackSpawn(StageData stageData);

public class GameManager : MonoBehaviour
{
    public CallBackFunction FreezeDataSetting = null;
    public CallBackFunction GameClearRecord = null;
    public CallBackFunction WaveClearEvent = null;
    public CallBackFunction PlayTimerEvent = null;
    public CallBackFunction PenaltyEvent = null;

    public CallBackSpawn spawnThief = null;
    public CallBackFunction hideThief = null;
    public CallBackFunction openThief = null;
    public CallBackFunction removeThief = null;


    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    instance = new GameObject(nameof(GameManager), typeof(GameManager)).GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] ParticleSystem waveClearEff = null;
    WaitForSeconds wait1 = null;
    WaitForSeconds waitNextWave = null;

    public GameData FreezeData { get; set; }
    public List<StageData> StageDatas { get; set; }
    public bool IsGaming { get; set; } = false;
    public int PlayTime { get; set; }

    int stage;
    int penalty;
    int catchNumber;
    public bool reStart { get; set; }

    private void Awake()
    {
        DataManager.Instance.LoadGameData();
        SoundManager.Instance.AddButtonListener();
        LevelSetting(PlayerPrefs.GetInt("level"));
    }

    private void Start()
    {
        wait1 = new WaitForSeconds(1f);
        waitNextWave = new WaitForSeconds(3f);
    }

    public void LevelSetting(int level)
    {
        FreezeData = DataManager.Instance.FindGameDataByLevel(level);
        StageDatas = DataManager.Instance.FindStageDatasByStageGroup(FreezeData.stageGroup, FreezeData.stageCount);
        ShuffleList(StageDatas);
        FreezeDataSetting?.Invoke();
        stage = 0;
        PlayTime = FreezeData.playTime;
        WaveSetting();
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count -1; i > 0; i--)
        {
            int random = Random.Range(0, i);

            T temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }
        return list;
    }


    public void WaveSetting()
    {
        openThief = null;
        hideThief = null;
        removeThief = null;
        spawnThief?.Invoke(StageDatas[stage]);
        openThief?.Invoke();
        penalty = StageDatas[stage].penaltyPoint;
        catchNumber = 0;
        UIManager.Instance.DataSetting(StageDatas[stage].wantedCount, StageDatas[stage].startCountdown);
        if(stage != 0 || reStart) UIManager.Instance.WaveStart();
        stage++;
    }

    public void WaveStart()
    {
        IsGaming = true; 
        StartCoroutine(nameof(PlayTimer));
        hideThief?.Invoke();
    }

    public void WaveClear()
    {
        IsGaming = false;
        removeThief?.Invoke();
        if (stage == StageDatas.Count) GameOver(true);
        else
        {
            UIManager.Instance.WaveClear();
            StartCoroutine(nameof(NextWave));
        }
    }
    public void ReStart()
    {
        reStart = true;
        IsGaming = false;
        removeThief?.Invoke();
        UIManager.Instance.WaveClear();
        stage = 0;
        PlayTime = FreezeData.playTime;
        PlayTimerEvent();
        WaveSetting();
    }
    public void GameOver(bool win)
    {
        if (win)
        {
            UIManager.Instance.Win();
            GameClearRecord();
        }
        else UIManager.Instance.Lose();
    }

    public void ShowImg()
    {
        openThief?.Invoke();
    }

    public void Catch(int id)
    {
        hideThief?.Invoke();
        catchNumber++;
        UIManager.Instance.Arrest(id);
        if (catchNumber == StageDatas[stage-1].wantedCount)
        {
            WaveClear();
        }
    }

    public void Penalty()
    {
        hideThief?.Invoke();
        PlayTime -= penalty;
        PenaltyEvent();
    }

    IEnumerator PlayTimer()
    {
        while (PlayTime > 0 && IsGaming)
        {
            PlayTime--;
            PlayTimerEvent();
            if (GameManager.Instance.PlayTime <= 0)
            {
                if (catchNumber < StageDatas[stage - 1].wantedCount)
                {
                    IsGaming = false;
                    GameOver(false);
                }
            }
            yield return wait1;
        }
    }

    IEnumerator NextWave()
    {
        waveClearEff.Play();
        WaveClearEvent?.Invoke(); //스테이지클리어 연출
        yield return waitNextWave;
        WaveSetting();
    }
}
