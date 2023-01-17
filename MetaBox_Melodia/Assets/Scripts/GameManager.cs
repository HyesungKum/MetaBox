
using UnityEngine;
using UnityEngine.SceneManagement;



public enum GameStatus
{
    Idle,
    StartGame,
    Ready,
    TimeOver,
    GetAllQNotes,
    NoMorePlayableNote
}



public class GameManager : MonoBehaviour
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


    GameStatus myGameStatus;

    bool isGameOver = false;


    [Header("Play Time")]
    [SerializeField] float myPlayableTime;
    [SerializeField] float myCoolTime;
    [SerializeField] float countDown;


    TouchManager myTouchManager;
    UiManager myUiManager;


    private void Awake()
    {
        myGameStatus = GameStatus.Idle;

        myUiManager = FindObjectOfType<UiManager>();
        myTouchManager = FindObjectOfType<TouchManager>();

        myTouchManager.enabled = false;

        WhatLevelDoWePlay();
    }


    public void UpdateGameStatus(GameStatus targetStatus)
    {

        myGameStatus = targetStatus;

        switch (myGameStatus)
        {
            case GameStatus.Ready:
                {
                    Debug.Log("준비!");

                    PlayTimer.DelegateTimer += timeCountDown;

                    // set play time and cool time 
                    myUiManager.MyPlayableTime = myPlayableTime;
                    myUiManager.ReplayCoolTime = myCoolTime;

                    Time.timeScale = 0;

                    SoundManager.Inst.FirstPlay();
                }
                break;

            case GameStatus.StartGame:
                {
                    Debug.Log("시작!!");

                    Time.timeScale = 1;

                    myTouchManager.enabled = true;
                }
                break;


            case GameStatus.TimeOver:
                {
                    Debug.Log("Time is over!");
                    myUiManager.GameResult("시간이 없어요");
                    isGameOver = true;
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    Debug.Log("Great sucess!");
                    myUiManager.GameResult("다 맞췄어요!");
                    isGameOver = true;
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    Debug.Log("Too bad, so sad!");
                    myUiManager.GameResult("음표가 더이상 없어요");
                    isGameOver = true;
                }
                break;
        }


        if (!isGameOver)
            return;


        myDelegateIsGameOver(true);
    }



    void WhatLevelDoWePlay()
    {
        Debug.Log("무슨 모드?");

        switch (SceneModeController.MySceneMode)
        {
            case SceneModeController.SceneMode.EasyMode:
                {
                    Debug.Log("쉬운 모드!");
                    myPlayableTime = 180f;
                    myCoolTime = 15;
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
        countDown = myPlayableTime - t;
        if (countDown <= 0)
        {
            UpdateGameStatus(GameStatus.TimeOver);
            Debug.Log("시간이 없어요");
        }

    }


    public void OnClickQuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaLobby");

        // back to start scene
        SceneModeController.MySceneMode = SceneModeController.SceneMode.StartScene;      // turn start panel on, turn lobby panel off
    }


    // fucntion for test, reload scene 
    public void OnClickExitStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaLobby");

        // re-select mode 
        SceneModeController.MySceneMode = SceneModeController.SceneMode.LobbyScene;      // turn start panel on, turn lobby panel off

    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaEasyMode");
    }


}
