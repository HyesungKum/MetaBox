using System.Collections;
using System.Threading;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    //=====================var=======================
    public int Score = 0;
    public int Timer = 10;

    public bool IsStart = false;
    public bool IsPause = false;
    public bool IsGameOver = false;

    public int Level = 1;

    //======================timer====================
    Coroutine GameRoutine;

    private readonly WaitForSeconds wait = new(1f);

    float SecCount = 0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        //initializing
        IsStart = false;
        IsPause = false;
        IsGameOver = false;

        //delegate chain
        EventReciver.ScoreModi += ScoreAddSub;
        EventReciver.GamePause += GamePasue;
        EventReciver.GameResume += GameResume;
        EventReciver.GameOver += GameOver;
    }

    private void Start()
    {
        SoundManager.Inst.SetBGM("MainBGM");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();

        GameRoutine = StartCoroutine(nameof(TimeUpdate));
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.ScoreModi -= ScoreAddSub;
        EventReciver.GamePause -= GamePasue;
        EventReciver.GameResume -= GameResume;
        EventReciver.GameOver -= GameOver;
    }

    IEnumerator TimeUpdate()
    {
        while (!IsGameOver)
        {
            yield return wait;

            Timer -= 1;
            EventReciver.CallTickCount();
            if (Timer == 0) EventReciver.CallGameOver();
        }
    }

    //=====================================Score Controll=========================================
    void ScoreAddSub(int value)
    {
        GameManager.Inst.Score += value;

        if (Score < 0) Score = 0;
    }

    //===================================Game Life Controll=======================================
    void GamePasue()
    {
        StopCoroutine(GameRoutine);
        GameManager.Inst.IsPause = true;
        Time.timeScale = 0;
    }
    void GameResume()
    {
        GameRoutine = StartCoroutine(nameof(TimeUpdate));
        GameManager.Inst.IsPause = false;
        Time.timeScale = 1f;   
    }
    void GameOver()
    {
        StopCoroutine(GameRoutine);
        IsGameOver = true;
        SoundManager.Inst.SetBGM("StageClear");
        SoundManager.Inst.SetBGMUnLoop();
        SoundManager.Inst.PlayBGM();
        Time.timeScale = 0f;
    }
}
