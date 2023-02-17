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
    Fail,
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

    public Action<int> DelegateCountDown;
    public Action<int> DelegateTimer;
    public Action<int> gameClearRecord;
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
    bool gameClear = false;

    private void Awake()
    {
        LoadGameData();
        MelodiaData = FindGameDataByLevel(StartUI.Level);
        StageDatas = FindStageDatasByStageGroup(MelodiaData.stageGroup);
        MyStageData = StageData(StartUI.MySceneMode);
        SoundManager.Inst.LoadMusicData(StartUI.MySceneMode);
        SoundManager.Inst.AddButtonListener();
        SoundManager.Inst.BGMPlay(2);
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
                    gameClear = false;
                    CurStatus = GameStatus.Idle;

                    DelegateTimer(MyPlayableTime);

                    // let all know idle status 
                    myDelegateGameStatus(GameStatus.Idle);

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
                    if (gameClear) myDelegateGameStatus(GameStatus.GameClear);
                    else if (stageClear && CurStage.Equals(MyStageData.Keys.Count)) UpdateCurProcess(GameStatus.GameClear);
                    else if(stageClear) myDelegateGameStatus(GameStatus.GetAllQNotes);
                    else UpdateCurProcess(GameStatus.GamePlaying);
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    stageClear = true;
                    CurStatus = GameStatus.GetAllQNotes;
                    SoundManager.Inst.SFXPlay(SFX.StageClear);
                    Invoke(nameof(ClearMusic), 1f);
                }
                break;

            case GameStatus.Fail:
                {
                    SoundManager.Inst.StopMusic();

                    SoundManager.Inst.SFXPlay(SFX.GameFail);

                    CurStatus = GameStatus.Fail;

                    myDelegateGameStatus(GameStatus.Fail);
                }
                break;

            case GameStatus.GameClear:
                {
                    CurStatus = GameStatus.GameClear;
                    gameClear = true;
                    CurStage++;
                    
                    Invoke(nameof(ClearMusic), 1f);
                    gameClearRecord(MelodiaData.countDown - MyPlayableTime);
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
        yield return wait1;

        for (int i = 3; i >= 0; i--)
        {
            DelegateCountDown(i);
            yield return wait1;
        }

        DelegateCountDown(0);

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
            if (MyPlayableTime.Equals(30)) SoundManager.Inst.SFXPlay(SFX.TimeLimit);
            if (MyPlayableTime <= 0)
            {
                UpdateCurProcess(GameStatus.Fail);
            }
            yield return wait1;
        }
    }
}
