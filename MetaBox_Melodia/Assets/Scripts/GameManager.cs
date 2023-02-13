using System.Collections.Generic;
using UnityEngine;



public enum GameStatus
{
    Idle,
    StartGame,
    Ready,
    MusicPlaying,
    MusicStop,
    Pause,
    TimeOver,
    GetAllQNotes,
    NoMorePlayableNote,
    GameResult,
    Restart,
    ClearStage,
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

    public delegate void DelegateIsGameOver();
    public static DelegateIsGameOver myDelegateIsGameOver;

    public delegate void DelegateGameStatus(GameStatus curStatue);
    public static DelegateGameStatus myDelegateGameStatus;


    bool isGameOver = false;
    bool isGameStart = false;
    bool isGameCleared = false;
    bool isStageCleared = false;
    bool isMusicPlaying = false;
    bool isPaused = false;

    public GameData MelodiaData { get; private set; }
    public List<StageData> StageDatas { get; private set; }

    static Dictionary<int, List<int>> myStageData = new();
    public Dictionary<int, List<int>> MyStageData { get { return myStageData; } }


    public GameStatus CurStatus { get; set; } = GameStatus.Idle;

    public int CurStage { get; private set; } = 1;
    public float MyPlayableTime { get; private set; }
    public float MyCoolTime { get; private set; }


    private void Awake()
    {
        LoadGameData();
        MelodiaData = FindGameDataByLevel(StartUI.Level);
        StageDatas = FindStageDatasByStageGroup(MelodiaData.stageGroup);
        SoundManager.Inst.LoadMusicData(StartUI.MySceneMode);
        myDelegateGameStatus += UpdateGameStatus;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    }


    private void Start()
    {
        Time.timeScale = 0;

        UpdateCurProcess(GameStatus.Idle);
    }

    // receive game status 
    public void UpdateCurProcess(GameStatus targetStatus)
    {
        switch (targetStatus)
        {
            case GameStatus.Restart:
                {

                    CurStage = 1;
                    MyPlayableTime = MelodiaData.countDown;

                    myDelegateGameStatus(GameStatus.Restart);
                    UpdateCurProcess(GameStatus.Idle);
                }
                break;



            case GameStatus.Idle:
                {
                    CurStatus = GameStatus.Idle;

                    isGameStart = false;
                    isGameOver = false;

                    checkStageLevel();

                    MyPlayableTime = MelodiaData.countDown;
                    MyCoolTime = MelodiaData.replayCooltime;


                    // let all know idle status 
                    myDelegateGameStatus(GameStatus.Idle);

                    // set current audio clip
                    SoundManager.Inst.SetStageMusic(CurStage, 1);
                }
                break;

            case GameStatus.Ready:
                {
                    CurStatus = GameStatus.Ready;

                    addDelegateChainStageTimer();


                    myDelegateGameStatus(GameStatus.Ready);

                    // play Music 
                    SoundManager.Inst.PlayStageMusic();
                }
                break;


            case GameStatus.Pause:
                {
                    CurStatus = GameStatus.Pause;

                    if (isPaused)
                    {
                        // at Ready or ClearStage status, stop countdown
                        if (CurStatus == GameStatus.Ready || CurStatus == GameStatus.ClearStage)
                        {
                            Time.timeScale = 0;
                        }

                        else
                        {
                            Time.timeScale = 1;
                        }

                        isPaused = false;
                        break;
                    }

                    isPaused = true;
                    Time.timeScale = 0;

                }
                break;


            case GameStatus.MusicPlaying:
                {
                    // if music played before start game, stop timer
                    if (!isGameStart)
                    {
                        isMusicPlaying = true;
                        Time.timeScale = 0;
                    }
                }
                break;


            case GameStatus.MusicStop:
                {
                    isMusicPlaying = false;


                    // if music played before start game, stop timer
                    if (!isGameStart)
                    {
                        myDelegateGameStatus(GameStatus.StartGame);
                    }



                    else if (isGameOver == true)
                    {
                        // cleared last stage 
                        if (isStageCleared)
                        {
                            myDelegateGameStatus(GameStatus.ClearStage);
                            break;
                        }

                        myDelegateGameStatus(GameStatus.GetAllQNotes);

                    }
                }
                break;
            // Game over ============================================================



            // failed ============================================================
            case GameStatus.TimeOver:
                {
                    isGameOver = true;
                    isGameCleared = false;

                    myDelegateGameStatus(GameStatus.TimeOver);

                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    isGameOver = true;
                    isGameCleared = false;

                    CurStatus = GameStatus.NoMorePlayableNote;
                    Time.timeScale = 0;

                    myDelegateGameStatus(GameStatus.NoMorePlayableNote);
                }
                break;
            // failed ============================================================


            // sucess ============================================================
            case GameStatus.GetAllQNotes:       // go to next round
                {
                    isGameOver = true;
                    isGameCleared = true;

                    CurStatus = GameStatus.GetAllQNotes;
                    myDelegateGameStatus(GameStatus.GameResult);

                    if (isGameOver)
                    {
                        Invoke(nameof(ClearMusic), 1f);
                    }

                    CurStage++;
                }
                break;
                // sucess ============================================================
        }
    }


    // broadcasting current game status 
    void UpdateGameStatus(GameStatus targetStatus)
    {

        switch (targetStatus)
        {

            case GameStatus.StartGame:
                {
                    CurStatus = GameStatus.StartGame;

                    isGameStart = true;


                    Time.timeScale = 1;
                }
                break;


            case GameStatus.TimeOver:
                {
                    isGameOver = true;
                    CurStatus = GameStatus.TimeOver;

                    SoundManager.Inst.StopMusic();
                }
                break;


            case GameStatus.NoMorePlayableNote:
                {
                    SoundManager.Inst.StopMusic();

                    CurStatus = GameStatus.NoMorePlayableNote;

                }
                break;

            case GameStatus.ClearStage:
                {
                    CurStatus = GameStatus.ClearStage;

                }
                break;
        }


        if (!isGameOver)
            return;


        myDelegateIsGameOver();
    }


    void ClearMusic()
    {
        // is last stage cleared ? 
        if (CurStage == MyStageData.Keys.Count)
        {
            CurStatus = GameStatus.ClearStage;

            isStageCleared = true;

            Time.timeScale = 0;

            // Set Audio Clip 
            SoundManager.Inst.SetStageMusic(5, 1);
        }


        // play current stage music 
        SoundManager.Inst.PlayStageMusic();
    }




    // time count ================================================================
    void timeCountdown(float t)
    {
        MyPlayableTime = t;

        if (MyPlayableTime <= 0)
        {
            UpdateCurProcess(GameStatus.TimeOver);
        }
    }
    // time count ================================================================



    // stage info ================================================================

    void GetStageInfo(SceneMode targetStage)
    {
        myStageData = StageData(targetStage);
    }


    public Dictionary<int, List<int>> CurStageInfo()
    {
        return MyStageData;
    }


    void addDelegateChainStageTimer()
    {
        PlayTimer.DelegateTimer += timeCountdown;

        Time.timeScale = 0;
    }

    void checkStageLevel()
    {
        switch (StartUI.MySceneMode)
        {
            case SceneMode.littlestar:
                {
                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo(SceneMode.littlestar);
                    }
                }
                break;

            case SceneMode.rabbit:
                {
                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo(SceneMode.rabbit);
                    }
                }
                break;

            case SceneMode.butterfly:
                {
                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo(SceneMode.butterfly);
                    }
                }
                break;

            case SceneMode.stone:
                {
                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo(SceneMode.stone);
                    }
                }
                break;
        }

    }

    // stage info ================================================================



}
