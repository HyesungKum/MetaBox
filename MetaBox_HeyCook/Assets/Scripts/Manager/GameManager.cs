using System.Collections;
using System.Threading;
using UnityEngine;
using Kum;
using System.Linq;

public class GameManager : MonoSingleTon<GameManager>
{
    //=====================Data Controll=======================
    [SerializeField] private LevelData levelData;
    [SerializeField] private LevelTable curLevelData;

    [Header("Aquire Setting")]
    [SerializeField] IngredientSpawner rightSpawner;
    [SerializeField] IngredientSpawner leftSpawner;
    [SerializeField] BeltZone beltZoneR;
    [SerializeField] BeltZone beltZoneL;
    [SerializeField] GuestTable guestTableR;
    [SerializeField] GuestTable guestTableL;
    [SerializeField] SpriteRenderer beltR;
    [SerializeField] SpriteRenderer beltL;
    [SerializeField] SpriteRenderer kitchenR;
    [SerializeField] SpriteRenderer kitchenL;
    [SerializeField] SpriteRenderer submissionImage;
    [SerializeField] SpriteRenderer back;
    [SerializeField] SpriteRenderer bottom;

    //=====================var=======================
    [Header("Currnet Score")]
    [SerializeField] private int score = 0;
    
    [Header("Limit Time")]
    [SerializeField] private int countDown = 180;
    private float timer = 0f; 
    [HideInInspector] public int count = 0;

    [Header("Imminent Time")]
    [SerializeField] private int imTime = 30;
    private bool audioToken = true;

    [Header("Current Game Status")]
    public bool IsGameIn;
    public bool IsStart;
    public bool IsPause;
    public bool IsGameOver;

    [Header("Game Difficulty Level")]
    private int level = 1;

    //======================timer====================
    Coroutine TickRoutine;

    private readonly WaitForSeconds wait = new(1f);

    private new void Awake()
    {
        //delegate chain
        EventReciver.ScoreModi += ScoreAddSub;

        EventReciver.GamePause += GamePasue;
        EventReciver.GameResume += GameResume;
        EventReciver.GameOver += GameOver;

        //initializing
        level = LevelTransfer.Inst.Level;
        curLevelData = levelData.levelTables[level];

        //divide each data
        countDown       = curLevelData.countDown;

        rightSpawner.SpawnTable = curLevelData.ingredGroup.ingredObjs.ToList();
        leftSpawner.SpawnTable = curLevelData.ingredGroup.ingredObjs.ToList();

        //rightSpawner.spawnTime = curLevelDtat.spawnTime;
        //leftSpawner.spawnTime = curLevelDtat.spawnTime;
        beltZoneL.beltSpeed    = curLevelData.beltSpeed;
        beltZoneR.beltSpeed    = curLevelData.beltSpeed;
        guestTableR.guestGroup = curLevelData.guestGroup;
        guestTableL.guestGroup = curLevelData.guestGroup;

        guestTableR.FoodList = curLevelData.foodDataGroup.foodDatas.ToList();
        guestTableL.FoodList = curLevelData.foodDataGroup.foodDatas.ToList();

        beltR.sprite           = curLevelData.conveyorBeltImage;
        beltL.sprite           = curLevelData.conveyorBeltImage;
        kitchenR.sprite        = curLevelData.kitchenImage;
        kitchenL.sprite        = curLevelData.kitchenImage;
        submissionImage.sprite = curLevelData.submissionImage;
        //back.sprite            = curLevelDtat.backGroundImage1;
        //bottom.sprite          = curLevelDtat.backGroundImage2;

        IsGameIn = true;
        IsStart = false;
        IsPause = false;
        IsGameOver = false;

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
        EventReciver.ScoreModi -= ScoreAddSub;

        EventReciver.GamePause -= GamePasue;
        EventReciver.GameResume -= GameResume;
        EventReciver.GameOver -= GameOver;
    }

    //============================================Score Controll============================================
    void ScoreAddSub(int value)
    {
        score += value;

        if (score < 0) score = 0;
    }

    //=========================================Game Life Controll============================================
    void GamePasue()
    {
        StopCoroutine(TickRoutine);
        IsPause = true;
        Time.timeScale = 0;
    }
    void GameResume()
    {
        TickRoutine = StartCoroutine(nameof(TickUpdate));
        IsPause = false;
        Time.timeScale = 1f;   
    }
    void GameOver()
    {
        StopCoroutine(TickRoutine);
        IsGameOver = true;
        SoundManager.Inst.SetBGM("StageClear");
        SoundManager.Inst.SetBGMUnLoop();
        SoundManager.Inst.PlayBGM();
        Time.timeScale = 0f;

        if (SaveLoadManger.Inst.GetOldScore(level) < score)
        {
            EventReciver.CallSaveCallBack(level, score);
        }
    }

    //============================================Timer Controll==========================================
    IEnumerator BeforeStart()
    {
        SoundManager.Inst.StopBGM();
        EventReciver.CallSceneStart();

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
        EventReciver.CallGameStart();
        TickRoutine = StartCoroutine(nameof(TickUpdate));
    }
    IEnumerator TickUpdate()
    {
        while (!IsGameOver && IsGameIn)
        {
            yield return wait;

            countDown -= 1;
            EventReciver.CallTickCount();
            if (audioToken && countDown == imTime)
            {
                audioToken = false;
                SoundManager.Inst.SetBGMSpeed(1.3f);
            }
            if (countDown == 0) EventReciver.CallGameOver();
        }
    }

    //============================================Getting Data=========================================
    public int GetScore() => score;
    public int GetCountDown() => countDown;
    public int GetLevel() => level;
}
