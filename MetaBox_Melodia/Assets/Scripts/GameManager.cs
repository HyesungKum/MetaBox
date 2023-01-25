using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum GameStatus
{
    Idle,
    StartGame,
    Ready,
    TimeOver,
    GetAllQNotes,
    NoMorePlayableNote,
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

    public delegate void DelegateIsGameOver(bool isOver);
    public static DelegateIsGameOver myDelegateIsGameOver;

    public delegate void DelegateGameStatus(GameStatus curStatue);
    public static DelegateGameStatus myDelegateGameStatus;


    bool isGameOver = false;

    int curStage = 1;
    public int CurState { get { return curStage; } set { curStage = value; } }


    [Header("Play Time")]
    [SerializeField] float myPlayableTime;
    public float MyPlayableTime { get { return myPlayableTime; } }

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

        myDelegateGameStatus(GameStatus.Idle);
    }


    public void CheckStage()
    {
        if (MyStageData.Keys.Count == curStage)
        { 
            myDelegateGameStatus(GameStatus.ClearStage);
            return;
        }


        myDelegateGameStatus(GameStatus.GetAllQNotes);
    }






    public void UpdateGameStatus(GameStatus targetStatus)
    {

        switch (targetStatus)
        {
            case GameStatus.Idle:
                {
                    isGameOver = false;

                    checkStageLevel();
                }
                break;

            case GameStatus.Ready:
                {
                    startStage();
                }
                break;

            case GameStatus.StartGame:
                {
                    Time.timeScale = 1;
                }
                break;


            case GameStatus.TimeOver:
                {
                    isGameOver = true;
                }
                break;

            case GameStatus.GetAllQNotes:       // go to next round
                {

                    curStage++;

                    isGameOver = true;
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    isGameOver = true;
                }
                break;

            case GameStatus.ClearStage:
                {
                    curStage = 1;
                    isGameOver = true;
                }
                break;


        }


        if (!isGameOver)
            return;


        myDelegateIsGameOver(true);
    }



    void startStage()
    {
        PlayTimer.DelegateTimer += timeCountDown;

        Time.timeScale = 0;
    }



    void checkStageLevel()
    {



        switch (SceneModeController.MySceneMode)
        {
            case SceneModeController.SceneMode.EasyMode:
                {

                    myPlayableTime = 180f;
                    myCoolTime = 15;

                    if (MyStageData.Count == 0)
                    {
                        GetStageInfo("EasyMode");
                    }
                }
                break;

            case SceneModeController.SceneMode.NormalMode:
                {
                    //depends on mode
                    //countDown = 180f;
                }
                break;

            case SceneModeController.SceneMode.DifficultMode:
                {
                    //depends on mode
                    //countDown = 180f;

                }
                break;

            case SceneModeController.SceneMode.ExtremeMode:
                {
                    //depends on mode
                    //countDown = 180f;

                }
                break;
        }

    }



    void timeCountDown(float t)
    {
        if (t <= 0)
        {
            myDelegateGameStatus(GameStatus.TimeOver);
        }
    }



    // stage info ================================================================

    void GetStageInfo(string targetStage)
    {
        myStageData = StageData(targetStage);
    }


    public Dictionary<int, List<int>> CurStageInfo()
    {
        return MyStageData;
    }

    // stage info ================================================================



}
