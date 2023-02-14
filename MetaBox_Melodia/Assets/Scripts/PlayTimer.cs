using System;
using System.Collections;
using UnityEngine;
using static PlayTimer;


public class PlayTimer : MonoBehaviour
{
    // subject => timer
    static public Action<float> DelegateTimer;
    public delegate void TimerStop();
    public static TimerStop timerStop;

    bool timerStopped = false;

    bool isStarted = false;
    bool isGameOver = false;

    [SerializeField] float timer;
    [SerializeField] float playTime;
    [SerializeField] float myPlayableTime;

    private void Awake()
    {
        // observe game status 
        GameManager.Inst.myDelegateGameStatus += curGameStatus;
        GameManager.Inst.myDelegateIsGameOver += isGameFinished;
        timerStop = TimeStop;
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

        StartCoroutine(nameof(startTimer));
    }

    void TimeStop()
    {
        if(timerStopped == false) timerStopped = true;
        else timerStopped = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;

        if (!isStarted)
            return;

        if (timerStopped)
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



    void isGameFinished()
    {
        isGameOver = true;
    }

}
