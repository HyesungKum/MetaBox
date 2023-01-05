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
        Application.targetFrameRate = 300;

        //initializing
        IsGameOver = false;

        //delegate chain
        EventReciver.ScoreModi += ScoreAddSub;
    }

    private void Update()
    {
        TimeUpdate();
    }

    private void OnDisable()
    {

        EventReciver.ScoreModi -= ScoreAddSub;
    }

    void TimeUpdate()
    {
        if (IsGameOver) return;

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
