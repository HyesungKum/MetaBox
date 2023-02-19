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

    public CallBackFunction freezeDataSetting = null; //police set data according to game level.
    public CallBackFunction gameClearRecord = null; //When the game is cleared, records are updated in the DB.
    public CallBackFunction playTimerEvent = null; //play time that changes every second is applied to the UI.
    public CallBackFunction penaltyEvent = null; //directing a penalty when arresting a citizen.
    public CallBackFunction policeReset = null; //police animator set - idle


    [Header("Thief Control")]
    public CallBackFunction spawnThief = null;
    public CallBackFunction openThief = null;
    public CallBackFunction hideThief = null;
    public CallBackFunction removeThief = null;
    public CallBackFunction hideEff = null;

    
    [SerializeField] GameObject postPrefab = null;
    List<GameObject> wantedPostList = new List<GameObject>();

    [SerializeField] ParticleSystem catchEff = null;
    [SerializeField] List<ParticleSystem> waveClearEff = null;


    public GameData FreezeData { get; private set; }
    public List<StageData> StageDatas { get; private set; }
    public bool IsGaming { get; private set; } = false;
    public bool IsPreparing { get; private set; } = false; //wave Ready
    public int CurStage { get; private set; } = -1;
    public int PlayTime { get; private set; } = 0;
    public int CatchNumber { get; private set; } = 0;

    WaitForSeconds wait1 = null;
    WaitForSeconds waitWaveStart = null;
    
    bool imgShowTime = false;

    void Awake()
    {
        DataManager.Instance.LoadGameData();
        SoundManager.Instance.AddButtonListener();
        LevelSetting(StartUI.MyLevel);
        if (postPrefab == null) postPrefab = (GameObject)Resources.Load(nameof(postPrefab));
        wait1 = new WaitForSeconds(1f);
        waitWaveStart = new WaitForSeconds(4f);
        SoundManager.Instance.MusicStart(1);
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
        yield return waitWaveStart;
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
        hideEff = null;
        spawnThief();
        UIManager.Instance.DataSetting();
    }

    public void WaveStart()
    {
        IsGaming = true; 
        StartCoroutine(nameof(PlayTimer));
        hideThief?.Invoke();
        SoundManager.Instance.PlaySFX(SFX.HideNPC);
    }

    public void WaveClear()
    {
        IsGaming = false;
        removeThief?.Invoke();
        
        if (CurStage == StageDatas.Count-1) GameOver(true);
        else
        {
            SoundManager.Instance.PlaySFX(SFX.WaveClear);
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
        policeReset?.Invoke();
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
            UIManager.Instance.Invoke("Win", 1f);
            gameClearRecord?.Invoke();
            for (int i = 0; i < wantedPostList.Count; i++)
            {
                PoolCp.Inst.DestoryObjectCp(wantedPostList[i]);
            }
            wantedPostList.Clear();
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
            if(IsGaming == false) yield break; //If the last thief is apprehended, the function to hide the image is not executed.
            yield return null;
        }
        if(imgShowTime) hideThief?.Invoke();
    }
    
    public void CatchShow()
    {
        if(catchEff.isPlaying) catchEff.Stop();
        catchEff.Play();
        wantedPostList.Add(PoolCp.Inst.BringObjectCp(postPrefab));
        if (wantedPostList.Count > 1) wantedPostList[wantedPostList.Count - 2].SendMessage("SetImg", SendMessageOptions.DontRequireReceiver);
        if(wantedPostList.Count > 2) wantedPostList[wantedPostList.Count - 3].SendMessage("HideImg", SendMessageOptions.DontRequireReceiver);
    }
    
    public void Catch()
    {
        CatchNumber++;
        SoundManager.Instance.PlaySFX(SFX.Catch);
        UIManager.Instance.Catch();
        if (CatchNumber == StageDatas[CurStage].wantedCount) WaveClear();
    }

    public void Penalty()
    {
        PlayTime -= StageDatas[CurStage].penaltyPoint;
        SoundManager.Instance.PlaySFX(SFX.Penalty);
        penaltyEvent?.Invoke();
    }

    IEnumerator PlayTimer()
    {
        while (PlayTime > 0 && IsGaming)
        {
            PlayTime--;
            playTimerEvent();
            if (PlayTime <= 0)
            {
                if (CatchNumber < StageDatas[CurStage].wantedCount)
                {
                    IsGaming = false;
                    GameOver(false);
                    SoundManager.Instance.PlaySFX(SFX.Fail);
                }
            }
            yield return wait1;
        }
    }
}
