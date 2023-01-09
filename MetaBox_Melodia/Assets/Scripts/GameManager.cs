using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
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


    GameStatus myGameStatus;

    bool isGameOver = true;

    TouchManager myTouchManager;
    UiManager myUiManager;
    PlayTimer myPlayTimer;

    private void Awake()
    {
        myGameStatus = GameStatus.Idle;

        myUiManager = FindObjectOfType<UiManager>();
        myTouchManager = FindObjectOfType<TouchManager>();
        myPlayTimer = FindObjectOfType<PlayTimer>();
        myTouchManager.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            switch (myGameStatus)
            {

                case GameStatus.TimeOver:
                    {
                        Debug.Log("Time is over!");
                        myUiManager.GameResult("시간이 없어요");
                        myPlayTimer.StopTimer();
                        isGameOver = false;
                    }
                    break;

                case GameStatus.GetAllQNotes:
                    {
                        Debug.Log("Great sucess!");
                        myUiManager.GameResult("다 맞췄어요!");
                        myPlayTimer.StopTimer();
                        isGameOver = false;
                    }
                    break;

                case GameStatus.NoMorePlayableNote:
                    {
                        Debug.Log("Too bad, so sad!");
                        myUiManager.GameResult("음표가 더이상 없어요");
                        myPlayTimer.StopTimer();
                        isGameOver = false;
                    }
                    break;

                case GameStatus.StartGame:
                    {
                        myTouchManager.enabled = true;
                        isGameOver = false;
                    }
                    break;

                default:
                    return;
            }

        }

    }


    public void UpdateGameStatus(GameStatus myStatus)
    {
        myGameStatus = myStatus;
        isGameOver = true;
    }



    public void OnClickQuitGame()
    {
        SceneManager.LoadScene("DemoStart");
    }


    // fucntion for test, reload scene 
    public void OnClickRestart()
    {
        SceneManager.LoadSceneAsync("DemoMelodia");
    }



}
