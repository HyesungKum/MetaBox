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



    [Header("Game Status Info")]
    [SerializeField] int curStage = 1;
    public int CurStage { get { return curStage; } set { curStage = value; } }

    [SerializeField]
    GameStatus curStatus;
    public GameStatus CurStatus { get { return curStatus; } set { curStatus = value; } }




    [Header("Play Time")]
    [SerializeField] float myPlayableTime;
    public float MyPlayableTime { get { return myPlayableTime; } }

    [SerializeField] float stagePlayTime;




    [SerializeField] float myCoolTime;
    public float MyCoolTime { get { return myCoolTime; } }


    [SerializeField] float countDown;


    static Dictionary<int, List<int>> myStageData = new();
    public Dictionary<int, List<int>> MyStageData { get { return myStageData; } }



    private void Awake()
    {
        myDelegateGameStatus = UpdateGameStatus;
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
                    myPlayableTime = stagePlayTime;

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

                    myPlayableTime = stagePlayTime;


                    // let all know idle status 
                    myDelegateGameStatus(GameStatus.Idle);

                    // set current audio clip
                    SoundManager.Inst.SetStageMusic(curStage, 1);
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
                        if (curStatus == GameStatus.Ready || curStatus == GameStatus.ClearStage)
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

                    curStage++;
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
        if (curStage == MyStageData.Keys.Count)
        {
            curStatus = GameStatus.ClearStage;

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
        myPlayableTime = t;

        if (MyPlayableTime <= 0)
        {
            UpdateCurProcess(GameStatus.TimeOver);
        }
    }
    // time count ================================================================



    // stage info ================================================================

    void GetStageInfo(string targetStage)
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
        switch (SceneModeController.MySceneMode)
        {
            case SceneMode.EasyMode:
                {
                    stagePlayTime = 180f;
                    myCoolTime = 15;

                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo("EasyMode");
                    }
                }
                break;

            case SceneMode.NormalMode:
                {
                    //depends on mode
                    //countDown = 180f;
                }
                break;

            case SceneMode.HardMode:
                {
                    //depends on mode
                    //countDown = 180f;

                }
                break;

            case SceneMode.ExtremeMode:
                {
                    //depends on mode
                    //countDown = 180f;

                }
                break;
        }

    }

    // stage info ================================================================



}
