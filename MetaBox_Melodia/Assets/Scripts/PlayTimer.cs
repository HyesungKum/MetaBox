using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayTimer : MonoBehaviour
{
    // subject => timer
    static public Action<float> DelegateTimer;

    bool isStarted = false;
    bool isGameOver = false;

    [SerializeField] float timer = 3f;
    [SerializeField] float curTime = 0f;

    private void Start()
    {
        GameManager.myDelegateIsGameOver += gameIsOver;
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
        curTime += Time.deltaTime;

        DelegateTimer(curTime);
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

        GameManager.Inst.UpdateGameStatus(GameStatus.Ready);
        isStarted = true;
    }


    void gameIsOver(bool isOver)
    {
        isGameOver = isOver;
    }

}
