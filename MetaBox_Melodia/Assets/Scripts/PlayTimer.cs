using System;
using System.Collections;
using UnityEngine;
using static GameManager;


public class PlayTimer : MonoBehaviour
{
    // subject => timer
    static public Action<float> DelegateTimer;

    bool isStarted = false;
    bool isGameOver = false;

    [SerializeField] float timer;
    [SerializeField] float playTime;
    [SerializeField] float myPlayableTime;

    private void Awake()
    {
        // observe game status 
        GameManager.myDelegateGameStatus += curGameStatus;
        myDelegateIsGameOver += isGameCleared;

        playTime = 0f;
    }


    void curGameStatus(GameStatus curStatus)
    {
        switch (curStatus)
        {
            case GameStatus.Idle:
                {
                    StartGame();
                }
                break;

            case GameStatus.Restart:
                {
                    playTime = GameManager.Inst.MyPlayableTime;
                }
                break;


            case GameStatus.GameResult:
                {
                    isGameOver = true;
                }
                break;
        }
    }




    void StartGame()
    {
        isStarted = false;
        isGameOver = false;

        Time.timeScale = 1;

        timer = 3f;

        StartCoroutine(startTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;

        if (!isStarted)
            return;

        timeCountDown();
    }


    void timeCountDown()
    {
        playTime -= Time.deltaTime;

        DelegateTimer(playTime);
    }


    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(1f);

        while (timer > 1)
        {
            timer -= Time.deltaTime;

            DelegateTimer(timer);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        isStarted = true;


        if (playTime == 0)
        {
            playTime = GameManager.Inst.MyPlayableTime;
        }

        GameManager.Inst.UpdateCurProcess(GameStatus.Ready);
    }



    void isGameCleared()
    {
        isGameOver = true;
    }

}
