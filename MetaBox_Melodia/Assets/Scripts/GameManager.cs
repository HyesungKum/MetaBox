using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PitchName
{
    Do,
    Le,
    Mi,
    Pa,
    Sol,
    La,
    Si
}


public enum GameStatus
{
    Idle,
    StartGame,
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
    bool isGameStarted = false;
    bool isPaused = false;


    TouchManager myTouchManager;
    UiManager myUiManager;


    private void Awake()
    {
        myGameStatus = GameStatus.Idle;

        myUiManager = FindObjectOfType<UiManager>();
        myTouchManager = FindObjectOfType<TouchManager>();

        myTouchManager.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // if game is not started! and 
        if (!isGameStarted && myGameStatus == GameStatus.StartGame)
        {
            myTouchManager.enabled = true;
            isGameStarted = true;
        }


        if (isGameOver)
        {
            switch (myGameStatus)
            {

                case GameStatus.TimeOver:
                    {
                        Debug.Log("Time is over!");
                        myUiManager.GameResult("시간이 없어요");
                        isGameOver = false;
                    }
                    break;

                case GameStatus.GetAllQNotes:
                    {
                        Debug.Log("Great sucess!");
                        myUiManager.GameResult("다 맞췄어요!");
                        isGameOver = false;
                    }
                    break;

                case GameStatus.NoMorePlayableNote:
                    {
                        Debug.Log("Too bad, so sad!");
                        myUiManager.GameResult("음표가 더이상 없어요");
                        isGameOver = false;
                    }
                    break;

                default:
                    return;
            }

            myDelegateIsGameOver(true);

        }

    }


    public void UpdateGameStatus(GameStatus myStatus)
    {
        myGameStatus = myStatus;
        isGameOver = true;
    }



    public void OnClickQuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaStart");
    }


    // fucntion for test, reload scene 
    public void OnClickExitStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MelodiaLobby");
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MelodiaEasy");
    }


}
