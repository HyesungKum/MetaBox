using Kum;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    //=====================Data Controll=======================
    [SerializeField] private LevelData levelData;
    [SerializeField] private LevelTable curLevelData;

    [Header("Acquire Setting")]
    [SerializeField] IngredientSpawner rightSpawner;
    [SerializeField] IngredientSpawner leftSpawner;
    [SerializeField] BeltZone beltZoneR;
    [SerializeField] BeltZone beltZoneL;
    [SerializeField] GuestTable guestTableR;
    [SerializeField] GuestTable guestTableL;
    [SerializeField] SpriteRenderer kitchenR;
    [SerializeField] SpriteRenderer kitchenL;
    [SerializeField] SpriteRenderer background;

    //=====================var=======================
    [Header("Currnet Score")]
    [SerializeField] private int score;
    
    [Header("Limit Time")]
    [SerializeField] private int countDown;
    private float timer; 
    [HideInInspector] public int count;

    [Header("Imminent Time")]
    [SerializeField] private int imTime;
    private bool audioToken = true;

    [Header("Current Game Status")]
    public bool IsGameIn;
    public bool IsStart;
    public bool IsPause;
    public bool IsGameOver;
    public bool IsHighScore;

    [Header("Game Difficulty Level")]
    private int level;

    //======================timer====================
    Coroutine TickRoutine;

    private readonly WaitForSeconds wait = new(1f);

    private new void Awake()
    {
        //delegate chain
        EventReceiver.ScoreModi += ScoreAddSub;
        EventReceiver.ScoreModiR += ScoreAddSub;
        EventReceiver.ScoreModiL += ScoreAddSub;

        EventReceiver.GamePause += GamePasue;
        EventReceiver.GameResume += GameResume;
        EventReceiver.GameOver += GameOver;

        //initializing
        level = LevelTransfer.Inst.Level == 0 ? 1 : LevelTransfer.Inst.Level;
        curLevelData = levelData.levelTables[level - 1];

        //value setting
        timer = 0f;
        countDown = curLevelData.countDown;
        imTime = curLevelData.ImmeTime;

        if (beltZoneL != null   ) beltZoneL.beltSpeed = curLevelData.beltSpeed;
        if (beltZoneR != null   ) beltZoneR.beltSpeed = curLevelData.beltSpeed;
        if (rightSpawner != null) rightSpawner.spawnTime = curLevelData.spawnTime;
        if (leftSpawner != null ) leftSpawner.spawnTime = curLevelData.spawnTime;

        //data setting
        if (guestTableR != null ) guestTableR.guestGroup  = curLevelData.guestGroup;
        if (guestTableL != null ) guestTableL.guestGroup  = curLevelData.guestGroup;
        if (rightSpawner != null) rightSpawner.SpawnTable = curLevelData.ingredGroup.ingredObjs.ToList();
        if (leftSpawner != null ) leftSpawner.SpawnTable = curLevelData.ingredGroup.ingredObjs.ToList();
        if (guestTableR != null ) guestTableR.FoodList    = curLevelData.foodDataGroup.foodDatas.ToList();
        if (guestTableL != null ) guestTableL.FoodList    = curLevelData.foodDataGroup.foodDatas.ToList();

        //image setting
        if (kitchenR != null    ) kitchenR.sprite         = curLevelData.kitchenImage;
        if (kitchenL != null    ) kitchenL.sprite         = curLevelData.kitchenImage;
        if (background != null        ) background.sprite            = curLevelData.backGroundImage;

        //flag setting
        IsGameIn = true;
        IsStart = false;
        IsPause = false;
        IsGameOver = false;
        IsHighScore = false;

        score = 0;
        timer = 0f;
    }

    private void Start()
    {
        StartCoroutine(nameof(BeforeStart));
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReceiver.ScoreModi -= ScoreAddSub;
        EventReceiver.ScoreModiR -= ScoreAddSub;
        EventReceiver.ScoreModiL -= ScoreAddSub;

        EventReceiver.GamePause -= GamePasue;
        EventReceiver.GameResume -= GameResume;
        EventReceiver.GameOver -= GameOver;
    }

    //============================================Score Controll============================================
    void ScoreAddSub(int value)
    {
        score += value;

        if (score < 0) score = 0;
    }

    //=========================================Game Life Controll============================================
    private void GamePasue()
    {
        StopCoroutine(TickRoutine);
        IsPause = true;
        Time.timeScale = 0;
    }
    private void GameResume()
    {
        TickRoutine = StartCoroutine(nameof(TickUpdate));
        IsPause = false;
        Time.timeScale = 1f;   
    }
    private void GameOver()
    {
        if (IsGameOver) return;

        StopCoroutine(TickRoutine);
        IsGameOver = true;
        Time.timeScale = 0f;

        SoundManager.Inst.SetBGM("StageClear");
        SoundManager.Inst.SetBGMUnLoop();
        SoundManager.Inst.PlayBGM();


        if (SaveLoadManger.Inst.GetOldScore(level) < score)
        {
            IsHighScore = true;
            EventReceiver.CallSaveCallBack(level, score);
        }
    }

    //============================================Timer Controll==========================================
    IEnumerator BeforeStart()
    {
        SoundManager.Inst.StopBGM();
        EventReceiver.CallSceneStart();

        while (timer <= 6f)
        {
            timer += Time.fixedDeltaTime;
            count = (int)timer;
            yield return null;
        }

        //bgm setting
        SoundManager.Inst.SetBGM("MainBGM");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();

        //main loop start
        IsStart = true;
        EventReceiver.CallGameStart();
        TickRoutine = StartCoroutine(nameof(TickUpdate));
    }
    IEnumerator TickUpdate()
    {
        while (!IsGameOver && IsGameIn)
        {
            yield return wait;

            countDown -= 1;
            EventReceiver.CallTickCount();
            if (audioToken && countDown == imTime)
            {
                audioToken = false;
                EventReceiver.CallGameImminent(curLevelData.ImmeSpawnTime , curLevelData.ImmeBeltSpeed);
                SoundManager.Inst.SetBGMSpeed(1.3f);
            }
            if (countDown == 0) EventReceiver.CallGameOver();
        }
    }

    //============================================Getting Data=========================================
    public int GetScore() => score;
    public int GetCountDown() => countDown;
    public int GetLevel() => level;
}
