
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
                    Debug.Log("�غ�!");

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
                    Debug.Log("����!!");

                    Time.timeScale = 1;

                    myTouchManager.enabled = true;
                }
                break;


            case GameStatus.TimeOver:
                {
                    Debug.Log("Time is over!");
                    myUiManager.GameResult("�ð��� �����");
                    isGameOver = true;
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    Debug.Log("Great sucess!");
                    myUiManager.GameResult("�� ������!");
                    isGameOver = true;
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    Debug.Log("Too bad, so sad!");
                    myUiManager.GameResult("��ǥ�� ���̻� �����");
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
        Debug.Log("���� ���?");

        switch (SceneModeController.MySceneMode)
        {
            case SceneModeController.SceneMode.EasyMode:
                {
                    Debug.Log("���� ���!");
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

        Debug.Log("���� ���? " + SceneModeController.MySceneMode);

    }



    void timeCountDown(float t)
    {
        countDown = myPlayableTime - t;
        if (countDown <= 0)
        {
            UpdateGameStatus(GameStatus.TimeOver);
            Debug.Log("�ð��� �����");
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
