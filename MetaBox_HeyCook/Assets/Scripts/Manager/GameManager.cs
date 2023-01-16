using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

public class GameManager : MonoSingleTon<GameManager>
{
    //=====================var=======================
    public int Score = 0;
    public int Timer = 10;

    public bool IsPause = false;
    public bool IsGameOver = false;

    public int Level = 1;

    //======================timer====================
    float SecCount = 0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        //initializing
        IsPause = false;
        IsGameOver = false;

        //delegate chain
        EventReciver.ScoreModi += ScoreAddSub;
        EventReciver.GamePause += GamePasue;
        EventReciver.GameResume += GameResume;
    }

    private void Start()
    {
        SoundManager.Inst.SetBGM("MainBGM");
        SoundManager.Inst.PlayBGM();
    }

    private void Update()
    {
        TimeUpdate();
    }

    private void OnDisable()
    {
        EventReciver.ScoreModi -= ScoreAddSub;
        EventReciver.GamePause -= GamePasue;
        EventReciver.GameResume -= GameResume;
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
        GameManager.Inst.Score += value;

        if (Score < 0) Score = 0;
    }

    void GamePasue()
    {
        GameManager.Inst.IsPause = true;
        Time.timeScale = 0;
    }

    void GameResume()
    {
        GameManager.Inst.IsPause = false;
        Time.timeScale = 1f;   
    }
}
