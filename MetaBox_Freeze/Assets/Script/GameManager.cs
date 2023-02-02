using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBackFunction();

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(GameManager), typeof(GameManager)).GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public CallBackFunction freezeDataSetting = null;
    public CallBackFunction gameClearRecord = null;
    public CallBackFunction playTimerEvent = null;
    public CallBackFunction penaltyEvent = null;

    public CallBackFunction spawnThief = null;
    public CallBackFunction openThief = null;
    public CallBackFunction hideThief = null;
    public CallBackFunction removeThief = null;
    public CallBackFunction hideEff = null;

    [SerializeField] ParticleSystem catchEff = null;
    [SerializeField] GameObject wantedPost = null;
    List<GameObject> wantedPostList = new List<GameObject>();

    [SerializeField] List<ParticleSystem> waveClearEff = null;
    public GameData FreezeData { get; private set; }
    public List<StageData> StageDatas { get; private set; }
    public bool IsGaming { get; private set; } = false;
    public bool IsPreparing { get; private set; } = false;
    public int CurStage { get; private set; } = -1;
    public int PlayTime { get; private set; } = 0;
    public int CatchNumber { get; private set; } = 0;

    WaitForSeconds wait1 = null;
    WaitForSeconds waitNextWave = null;
    
    bool imgShowTime = false;

    void Awake()
    {
        DataManager.Instance.LoadGameData();
        SoundManager.Instance.AddButtonListener();
        LevelSetting(PlayerPrefs.GetInt("level"));
        if (wantedPost == null) wantedPost = (GameObject)Resources.Load(nameof(WantedPost));
        wait1 = new WaitForSeconds(1f);
        waitNextWave = new WaitForSeconds(4f);
    }

    public void LevelSetting(int level)
    {
        FreezeData = DataManager.Instance.FindGameDataByLevel(level);
        StageDatas = DataManager.Instance.FindStageDatasByStageGroup(FreezeData.stageGroup, FreezeData.stageCount);
        ShuffleList(StageDatas);
        freezeDataSetting?.Invoke(); //police¼¼ÆÃ
        PlayTime = FreezeData.playTime;
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

    private void Start()
    {
        StartCoroutine(nameof(WaveReady));
    }

    IEnumerator WaveReady()
    {
        UIManager.Instance.Option(false);
        yield return wait1;
        WaveSetting();
        playTimerEvent?.Invoke();
        yield return wait1;
        UIManager.Instance.Countdown();
        yield return waitNextWave;
        WaveStart();
        UIManager.Instance.Option(true);
    }

    public void WaveSetting()
    {
        CurStage++;
        CatchNumber = 0;
        openThief = null;
        hideThief = null;
        removeThief = null;
        spawnThief();
        UIManager.Instance.DataSetting();
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
        openThief = null;
        hideThief = null;
        removeThief = null;
        hideEff = null;
        if (CurStage == StageDatas.Count-1) GameOver(true);
        else
        {
            SoundManager.Instance.WaveClearSFX();
            for(int i = 0; i < wantedPostList.Count; i++)
            {
                PoolCp.Inst.DestoryObjectCp(wantedPostList[i]);
            }
            wantedPostList.Clear();

            for (int i = 0; i < waveClearEff.Count; i++)
            {
                waveClearEff[i].Play();
            }
            
            UIManager.Instance.WaveClear();
            StartCoroutine(nameof(WaveReady));
        }
    }

    public void ReStart()
    {
        IsGaming = false;
        removeThief?.Invoke();
        PlayTime = FreezeData.playTime;
        CurStage = -1;
        for (int i = 0; i < wantedPostList.Count; i++)
        {
            PoolCp.Inst.DestoryObjectCp(wantedPostList[i]);
        }
        wantedPostList.Clear();
        StartCoroutine(nameof(WaveReady));
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            for (int i = 0; i < wantedPostList.Count; i++)
            {
                PoolCp.Inst.DestoryObjectCp(wantedPostList[i]);
            }
            wantedPostList.Clear();
            UIManager.Instance.Win();
            gameClearRecord?.Invoke();
        }
        else UIManager.Instance.Lose();
    }

    public void ShowImg()
    {
        imgShowTime = false;
        StartCoroutine(nameof(ImgShow));
    }
    IEnumerator ImgShow()
    {
        yield return null;
        openThief?.Invoke();
        float showTime = StageDatas[CurStage].startCountdown;
        imgShowTime = true;
        while (showTime > 0f && imgShowTime)
        {
            showTime -= Time.deltaTime;
            if(IsGaming == false) imgShowTime = false;
            yield return null;
        }
        if(imgShowTime) hideThief?.Invoke();
    }
    
    public void CatchShow()
    {
        if(catchEff.isPlaying) catchEff.Stop();
        catchEff.Play();
        wantedPostList.Add(PoolCp.Inst.BringObjectCp(wantedPost));
    }
    public void PostDone()
    {
        if (wantedPostList.Count > 1) wantedPostList[wantedPostList.Count - 2].SendMessage("SetImg", SendMessageOptions.DontRequireReceiver);
    }

    public void Catch()
    {
        CatchNumber++;
        SoundManager.Instance.CatchSFX();
        UIManager.Instance.Catch();
        if (CatchNumber == StageDatas[CurStage].wantedCount) WaveClear();
    }

    public void Penalty()
    {
        PlayTime -= StageDatas[CurStage].penaltyPoint;
        SoundManager.Instance.PenaltySFX();
        penaltyEvent?.Invoke();
    }

    IEnumerator PlayTimer()
    {
        while (PlayTime > 0 && IsGaming)
        {
            PlayTime--;
            playTimerEvent();
            if (GameManager.Instance.PlayTime <= 0)
            {
                if (CatchNumber < StageDatas[CurStage].wantedCount)
                {
                    IsGaming = false;
                    GameOver(false);
                    SoundManager.Instance.WaveFailSFX();
                }
            }
            yield return wait1;
        }
    }
}
