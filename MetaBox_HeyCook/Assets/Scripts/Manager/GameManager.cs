using UnityEngine;

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
        StaticEventReciver.ScoreModi += ScoreAddSub;
        StaticEventReciver.GamePause += GamePasue;
        StaticEventReciver.GameResume += GameResume;
        StaticEventReciver.GameOver += GameOver;
    }

    private void Start()
    {
        SoundManager.Inst.SetBGM("MainBGM");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();
    }

    private void Update()
    {
        TimeUpdate();
    }

    private void OnDisable()
    {
        //delegate unchain
        StaticEventReciver.ScoreModi -= ScoreAddSub;
        StaticEventReciver.GamePause -= GamePasue;
        StaticEventReciver.GameResume -= GameResume;
        StaticEventReciver.GameOver -= GameOver;
    }

    void TimeUpdate()
    {
        if (IsGameOver) return;

        SecCount += Time.deltaTime;

        if (SecCount <= 1f) return;
        
        Timer -= 1;
        
        if (Timer == 0) StaticEventReciver.CallGameOver();

        SecCount = 0f;
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
        GameManager.Inst.IsPause = true;
        Time.timeScale = 0;
    }
    void GameResume()
    {
        GameManager.Inst.IsPause = false;
        Time.timeScale = 1f;   
    }
    void GameOver()
    {
        IsGameOver = true;
        SoundManager.Inst.SetBGM("StageClear");
        SoundManager.Inst.SetBGMUnLoop();
        SoundManager.Inst.PlayBGM();
        Time.timeScale = 0f;
    }
}
