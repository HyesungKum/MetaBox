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

                    Debug.Log("Idle_GM");

                    checkStageLevel();
                }
                break;

            case GameStatus.Ready:
                {
                    Debug.Log("준비_GM");
                    startStage();
                }
                break;

            case GameStatus.StartGame:
                {
                    Debug.Log("시작!!_GM");
                    Time.timeScale = 1;
                }
                break;


            case GameStatus.TimeOver:
                {
                    Debug.Log("Time is over!_GM");
                    isGameOver = true;
                }
                break;

            case GameStatus.GetAllQNotes:       // go to next round
                {
                    Debug.Log("Great sucess!_GM");

                    Debug.Log($"현재 {curStage}스테이지 성공 {MyStageData.Keys.Count} 스테이지!");

                    curStage++;

                    isGameOver = true;
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    Debug.Log("Too bad, so sad!_GM");
                    isGameOver = true;
                }
                break;

            case GameStatus.ClearStage:
                {
                    Debug.Log("스테이지 클리어!");
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

        Debug.Log("쿨타임!" + myCoolTime);
    }



    void checkStageLevel()
    {
        Debug.Log("무슨 모드?");




        switch (SceneModeController.MySceneMode)
        {
            case SceneModeController.SceneMode.EasyMode:
                {
                    Debug.Log("쉬운 모드!");
                    myPlayableTime = 180f;
                    myCoolTime = 15;

                    if (MyStageData.Count == 0)
                    {
                        Debug.Log("데이터 불러와!");
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

        Debug.Log("무슨 모드? " + SceneModeController.MySceneMode);

    }



    void timeCountDown(float t)
    {
        if (t <= 0)
        {
            myDelegateGameStatus(GameStatus.TimeOver);
            Debug.Log("시간이 없어요");
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
