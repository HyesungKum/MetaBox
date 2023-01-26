using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void Notice();

public class GameManager : MonoBehaviour
{
    public Notice FreezeDataSetting = null;
    public Notice GameClearRecord = null;
    public Notice WaveClearEvent = null;
    public Notice PlayTimerEvent = null;
    public Notice PenaltyEvent = null;

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

    [SerializeField] ThiefSpawner thiefSpawner = null;
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
        thiefSpawner.Spawn(StageDatas[stage]);
        thiefSpawner.Open();
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
        thiefSpawner.Hide();
    }

    public void WaveClear()
    {
        IsGaming = false;
        thiefSpawner.Remove();
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
        thiefSpawner.Remove();
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
        thiefSpawner.Open();
    }

    public void Catch(int id)
    {
        thiefSpawner.Hide();
        catchNumber++;
        UIManager.Instance.Arrest(id);
        if (catchNumber == StageDatas[stage-1].wantedCount)
        {
            WaveClear();
        }
    }

    public void Penalty()
    {
        thiefSpawner.Hide();
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
