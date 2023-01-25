using System;
using System.Collections;
using UnityEngine;


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
        }
    }




    void StartGame()
    {
        isStarted = false;
        isGameOver = false;

        Time.timeScale = 1;

        timer = 3f;
        playTime = 0f;

        GameManager.myDelegateIsGameOver = gameIsOver;
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
        playTime += Time.deltaTime;

        float remainTime = myPlayableTime - playTime;

        DelegateTimer(remainTime);
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

        GameManager.myDelegateGameStatus(GameStatus.Ready);

        myPlayableTime = GameManager.Inst.MyPlayableTime;
        isStarted = true;
    }


    void gameIsOver(bool isOver)
    {
        isGameOver = isOver;
    }

}
