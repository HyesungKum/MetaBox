using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleTon<GameManager>
{
    //=====================var=======================
    public int Score = 0;
    public int Timer = 10;

    public bool IsGameOver = false;

    public int Level = 1;

    //======================timer====================
    float SecCount = 0f;

    private void Awake()
    {
        //initializing
        IsGameOver = false;

        //delegate chain
        EventReciver.ScoreModi += ScoreAddSub;
    }

    private void Update()
    {
        TimeUpdate();

        if (GameManager.Inst.IsGameOver)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("2. MainScene");
                }
            }
        }
    }

    private void OnDisable()
    {

        EventReciver.ScoreModi -= ScoreAddSub;
    }

    void TimeUpdate()
    {
        SecCount += Time.deltaTime;

        if (SecCount <= 1f) return;
        
        Timer -= 1;
        if (Timer == 0)
        {
            IsGameOver = true;
            EventReciver.CallGameOver();
            Time.timeScale = 0;
        }
        SecCount = 0f;
    }

    void ScoreAddSub(int value)
    {
        Score += value;
    }
}
