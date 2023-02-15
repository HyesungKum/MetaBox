using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    Idle,
    GamePlaying,
    Pause,
    MusicPlaying,
    MusicStop,
    GetAllQNotes,
    NoMorePlayableNote,
    TimeOver,
    GameClear,
    Restart
}

public class GameManager : DataLoader
{
    #region Singleton

    private static GameManager instance = null;
    public static GameManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    public Action<int> DelegateTimer;
    public delegate void DelegateGameStatus(GameStatus curStatue);
    public DelegateGameStatus myDelegateGameStatus;

    public GameStatus CurStatus { get; private set; }
    public GameData MelodiaData { get; private set; }
    public List<StageData> StageDatas { get; private set; }
    public Dictionary<int, List<int>> MyStageData { get; private set; }


    public int CurStage { get; private set; }
    public int MyPlayableTime { get; private set; }

    WaitForSeconds wait1 = null;
    bool stageClear = false;

    private void Awake()
    {
        LoadGameData();
        MelodiaData = FindGameDataByLevel(StartUI.Level);
        StageDatas = FindStageDatasByStageGroup(MelodiaData.stageGroup);
        MyStageData = StageData(StartUI.MySceneMode);
        SoundManager.Inst.LoadMusicData(StartUI.MySceneMode);
    }

    private void Start()
    {
        CurStage = 0;
        MyPlayableTime = MelodiaData.countDown;
        wait1 = new WaitForSeconds(1);
        UpdateCurProcess(GameStatus.Idle);
    }

    // receive game status 
    public void UpdateCurProcess(GameStatus targetStatus)
    {
        switch (targetStatus)
        {
            case GameStatus.Idle: //스테이지 세팅
                {
                    CurStage++;
                    stageClear = false;
                    CurStatus = GameStatus.Idle;

                    // let all know idle status 
                    myDelegateGameStatus(GameStatus.Idle);

                    // set current audio clip
                    SoundManager.Inst.SetStageMusic(CurStage, 1);

                    StartCoroutine(nameof(CountDown3));
                }
                break;

            case GameStatus.GamePlaying:
                {
                    CurStatus = GameStatus.GamePlaying;
                    myDelegateGameStatus(GameStatus.GamePlaying);
                    StartCoroutine(nameof(PlayTimer));
                    //Time.timeScale = 1;
                    //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
                break;

            case GameStatus.Pause:
                {
                    CurStatus = GameStatus.Pause;
                    myDelegateGameStatus(GameStatus.Pause);
                    StopCoroutine(nameof(PlayTimer));
                    //Time.timeScale = 0;
                    //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
                break;

            case GameStatus.MusicPlaying:
                {
                    UpdateCurProcess(GameStatus.Pause);
                }
                break;

            case GameStatus.MusicStop:
                {
                    if (stageClear && CurStage.Equals(MyStageData.Keys.Count)) UpdateCurProcess(GameStatus.GameClear);
                    else if(stageClear) myDelegateGameStatus(GameStatus.GetAllQNotes);
                    else UpdateCurProcess(GameStatus.GamePlaying);
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    stageClear = true;

                    Invoke(nameof(ClearMusic), 1f);
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    SoundManager.Inst.StopMusic();

                    CurStatus = GameStatus.NoMorePlayableNote;

                    myDelegateGameStatus(GameStatus.NoMorePlayableNote);
                }
                break;

            case GameStatus.TimeOver:
                {
                    CurStatus = GameStatus.TimeOver;

                    SoundManager.Inst.StopMusic();
                    myDelegateGameStatus(GameStatus.TimeOver);
                }
                break;

            case GameStatus.GameClear:
                {
                    CurStatus = GameStatus.GameClear;
                    SoundManager.Inst.SetStageMusic(MyStageData.Keys.Count +1, 1);
                    Invoke(nameof(ClearMusic), 1f);
                    myDelegateGameStatus(GameStatus.GameClear);
                }
                break;

            case GameStatus.Restart:
                {
                    CurStage = 0;
                    MyPlayableTime = MelodiaData.countDown;
                    UpdateCurProcess(GameStatus.Idle);
                }
                break;
        }
    }

    IEnumerator CountDown3()
    {
        yield return wait1;

        for (int i = 3; i > 0; i--)
        {
            DelegateTimer(i);
            yield return wait1;
        }

        DelegateTimer(999);
        yield return wait1;
        SoundManager.Inst.PlayStageMusic();
    }

    void ClearMusic()
    {
        // play current stage music 
        SoundManager.Inst.PlayStageMusic();
    }


    IEnumerator PlayTimer()
    {
        DelegateTimer(MyPlayableTime);
        yield return wait1;

        while (MyPlayableTime > 0)
        {
            MyPlayableTime--;
            DelegateTimer(MyPlayableTime);
            if (MyPlayableTime <= 0)
            {
                UpdateCurProcess(GameStatus.TimeOver);
            }
            yield return wait1;
        }
    }
}
