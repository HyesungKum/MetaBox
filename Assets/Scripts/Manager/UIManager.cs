using KumTool.AppTransition;
using UnityEngine;
using UnityEngine.UI;
using RankingDB;
using TMPro;

public class LevelString
{
    #region LevelString
    public string levelOne = "levelOne";
    public string levelTwo = "levelTwo";
    public string levelThree = "levelThree";
    public string levelFour = "levelFour";

    public string songOneLevelOne = "songOneLevelOne";
    public string songOneLevelTwo = "songOneLevelTwo";
    public string songOneLevelThree = "songOneLevelThree";
    public string songOneLevelFour = "songOneLevelFour";

    public string songTwoLevelOne = "songTwoLevelOne";
    public string songTwoLevelTwo = "songTwoLevelTwo";
    public string songTwoLevelThree = "songTwoLevelThree";
    public string songTwoLevelFour = "songTwoLevelFour";

    public string songThreeLevelOne = "songThreeLevelOne";
    public string songThreeLevelTwo = "songThreeLevelTwo";
    public string songThreeLevelThree = "songThreeLevelThree";
    public string songThreeLevelFour = "songThreeLevelFour";

    public string songFourLevelOne = "songFourLevelOne";
    public string songFourLevelTwo = "songFourLevelTwo";
    public string songFourLevelThree = "songFourLevelThree";
    public string songFourLevelFour = "songFourLevelFour";
    #endregion
}

public class UIManager : MonoBehaviour
{
    //current activation UI Object
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI = null;

    [Header("UI Group")]
    [SerializeField] GameObject mainUIGroup;
    [SerializeField] Button optionButton;

    [Header("Game UI Group")]
    [SerializeField] GameObject heyCookUIGroup;
    [SerializeField] Button heyCookExitButton;
    [SerializeField] Button heyCookStartButton;
    [Space]
    [SerializeField] GameObject freezeUIGroup;
    [SerializeField] Button freezeExitButton;
    [SerializeField] Button freezeStartButton;
    [Space]
    [SerializeField] GameObject melodiaUIGroup;
    [SerializeField] Button melodiaExitButton;
    [SerializeField] Button melodiaStartButton;
    [Space]
    [SerializeField] GameObject dreamSketchUIGroup;
    [SerializeField] Button dreamSketchExitButton;
    [SerializeField] Button dreamSketchStartButton;

    [Header("Option UI Group")]
    [SerializeField] GameObject optionUIGrpoup;
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SfxSlider;
    [SerializeField] Button optionResumeButton;
    [SerializeField] Button optionExitButton;

    [Header("ExitCheck UI Group")]
    [SerializeField] GameObject exitCheckUIGroup;
    [SerializeField] Button exitButton;
    [SerializeField] Button resumeButton;

    [Header("MongoDB Manger")]
    [SerializeField] MongoDBManager mongoDBManager = null;
    [SerializeField] TopTenRankMgr topTenRank = null;

    [Header("Ranking Info Text")]
    [SerializeField] GameObject rankingInfo = null;

    LevelString levelString;

    private void Awake()
    {
        levelString = new LevelString();
        rankingInfo.gameObject.SetActive(false);
        //main UI Button Listener
        optionButton.onClick.AddListener(() => ShowUI(optionUIGrpoup));

        //Game UI Group button listener
        heyCookStartButton.onClick.AddListener(() => AppTrans.MoveScene(GameManager.Inst.HeyCookPackageName));
        heyCookExitButton.onClick.AddListener(() => DestroyRanking(mainUIGroup, topTenRank.heyCookLevelShowPanking));

        freezeStartButton.onClick.AddListener(() => AppTrans.MoveScene(GameManager.Inst.PoliRunPackageName));
        freezeExitButton.onClick.AddListener(() => DestroyRanking(mainUIGroup, topTenRank.poliRunLevelShowRanking));

        melodiaStartButton.onClick.AddListener(() => AppTrans.MoveScene(GameManager.Inst.MelodiaPackageName));
        melodiaExitButton.onClick.AddListener(() => DestroyMelodia(mainUIGroup));

        dreamSketchStartButton.onClick.AddListener(() => AppTrans.MoveScene(GameManager.Inst.DreamSketchPackageName));
        dreamSketchExitButton.onClick.AddListener(() => DestroyRanking(mainUIGroup, topTenRank.dreamSketchLevelShowRanking));

        //option UI Group Listener
        BgmSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));
        SfxSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));
        optionResumeButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        optionExitButton.onClick.AddListener(() => ShowUI(exitCheckUIGroup));

        //Exit UI Group button Listener
        exitButton.onClick.AddListener(() => EventReceiver.CallAppQuit());
        resumeButton.onClick.AddListener(() => ShowUI(mainUIGroup));

        //delegate chain
        EventReceiver.gameIn += OpenGamePanel;
    }

    private void OnDisable()
    {
        EventReceiver.gameIn -= OpenGamePanel;
    }

    void OpenGamePanel(string gameName)
    {
        switch (gameName)
        {
            case GameName.HeyCook:
                {
                    SoundManager.Inst.CallSfx("HeyCookSfx");
                    ShowHeyCookRank(heyCookUIGroup);
#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled($"com.MetaBox.HeyCook")) heyCookStartButton.interactable = true;
                    else heyCookStartButton.interactable = false;
#endif
                }
                break;
            case GameName.PoliRun:
                {
                    SoundManager.Inst.CallSfx("FreezeSfx");
                    ShowFreezeRank(freezeUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled($"com.MetaBox.Freeze")) freezeStartButton.interactable = true;
                    else freezeStartButton.interactable = false;
#endif
                }
                break;
            case GameName.Melodia:
                {
                    SoundManager.Inst.CallSfx("MelodiaSfx");
                    ShowMelodiaRank(melodiaUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled("com.MetaBox.Melodia")) melodiaStartButton.interactable = true;
                    else melodiaStartButton.interactable = false;
#endif
                }
                break;
            case GameName.DreamSketch:
                {
                    SoundManager.Inst.CallSfx("SketchUPSfx");
                    ShowSketchUPRank(dreamSketchUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled("com.MetaBox.DreamSketch")) dreamSketchStartButton.interactable = true;
                    else dreamSketchStartButton.interactable = false;
#endif
                }
                break;
        }
    }

    void ShowUI(GameObject targetUIObj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    void ShowHeyCookRank(GameObject targetUIobj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);
        if (targetUIobj == heyCookUIGroup)
        {
            rankingInfo.gameObject.SetActive(true);

            mongoDBManager.GetAllUserData(mongoDBManager.HeyCookCollection, levelString.levelOne, topTenRank.heyCookPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelOneDict, topTenRank.heyCookLevelShowPanking[0], 1,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.HeyCookCollection, levelString.levelTwo, topTenRank.heyCookPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelTwoDict, topTenRank.heyCookLevelShowPanking[1], 2,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.HeyCookCollection, levelString.levelThree, topTenRank.heyCookPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelThreeDict, topTenRank.heyCookLevelShowPanking[2], 3,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.HeyCookCollection, levelString.levelFour, topTenRank.heyCookPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelFourDict, topTenRank.heyCookLevelShowPanking[3], 4,
                topTenRank.playerDataList);
        }

        curUI = targetUIobj;
        Invoke(nameof(ShowRanking), 0.5f);
    }

    void ShowRanking()
    {
        rankingInfo.gameObject.SetActive(false);
        curUI.SetActive(true);
    }

    void ShowFreezeRank(GameObject targetUIobj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);
        if (targetUIobj == freezeUIGroup)
        {
            rankingInfo.gameObject.SetActive(true);

            mongoDBManager.GetAllUserData(mongoDBManager.PoliRunCollection, levelString.levelOne, topTenRank.poliRunPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelOneDict, topTenRank.poliRunLevelShowRanking[0], 1,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.PoliRunCollection, levelString.levelTwo, topTenRank.poliRunPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelTwoDict, topTenRank.poliRunLevelShowRanking[1], 2,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.PoliRunCollection, levelString.levelThree, topTenRank.poliRunPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelThreeDict, topTenRank.poliRunLevelShowRanking[2], 3,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.PoliRunCollection, levelString.levelFour, topTenRank.poliRunPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelFourDict, topTenRank.poliRunLevelShowRanking[3], 4,
                topTenRank.playerDataList);

        }
        curUI = targetUIobj;
        Invoke(nameof(ShowRanking), 0.5f);
    }

    void ShowMelodiaRank(GameObject targetUIobj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);
        if (targetUIobj == melodiaUIGroup)
        {
            rankingInfo.gameObject.SetActive(true);

            #region songOne
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songOneLevelOne, topTenRank.melodiaPlayerRank,
                mongoDBManager.ID, mongoDBManager.melodiaSongOneLevelOneDict, topTenRank.melodiaSongOneRanking[0], 1,
                topTenRank.playerMelodiaSongOneDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songOneLevelTwo, topTenRank.melodiaPlayerRank,
                mongoDBManager.ID, mongoDBManager.melodiaSongOneLevelTwoDict, topTenRank.melodiaSongOneRanking[1], 2,
                topTenRank.playerMelodiaSongOneDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songOneLevelThree, topTenRank.melodiaPlayerRank,
                mongoDBManager.ID, mongoDBManager.melodiaSongOneLevelThreeDict, topTenRank.melodiaSongOneRanking[2], 3,
                topTenRank.playerMelodiaSongOneDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songOneLevelFour, topTenRank.melodiaPlayerRank,
                mongoDBManager.ID, mongoDBManager.melodiaSongOneLevelFourDict, topTenRank.melodiaSongOneRanking[3], 4,
                topTenRank.playerMelodiaSongOneDataList);
            #endregion
            #region songTwo
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songTwoLevelOne, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongTwoLevelOneDict, topTenRank.melodiaSongTwoRanking[0], 1, topTenRank.playerMelodiaSongTwoDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songTwoLevelTwo, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongTwoLevelTwoDict, topTenRank.melodiaSongTwoRanking[1], 2, topTenRank.playerMelodiaSongTwoDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songTwoLevelThree, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongTwoLevelThreeDict, topTenRank.melodiaSongTwoRanking[2], 3, topTenRank.playerMelodiaSongTwoDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songTwoLevelFour, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongTwoLevelFourDict, topTenRank.melodiaSongTwoRanking[3], 4, topTenRank.playerMelodiaSongTwoDataList);
            #endregion

            #region songThree
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songThreeLevelOne, topTenRank.melodiaPlayerRank,
                mongoDBManager.ID, mongoDBManager.melodiaSongThreeLevelOneDict, topTenRank.melodiaSongThreeRanking[0], 1,
                topTenRank.playerMelodiaSongThreeDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songThreeLevelTwo, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongThreeLevelTwoDict, topTenRank.melodiaSongThreeRanking[1], 2, topTenRank.playerMelodiaSongThreeDataList);
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songThreeLevelThree, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongThreeLevelThreeDict, topTenRank.melodiaSongThreeRanking[2], 3, topTenRank.playerMelodiaSongThreeDataList);
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songThreeLevelFour, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongThreeLevelFourDict, topTenRank.melodiaSongThreeRanking[3], 4, topTenRank.playerMelodiaSongThreeDataList);
            #endregion
            #region songFour
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songFourLevelOne, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongFourLevelOneDict, topTenRank.melodiaSongFourRanking[0], 1, topTenRank.playerMelodiaSongFourDataList);
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songFourLevelTwo, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongFourLevelTwoDict, topTenRank.melodiaSongFourRanking[1], 2, topTenRank.playerMelodiaSongFourDataList);
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songFourLevelThree, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongFourLevelThreeDict, topTenRank.melodiaSongFourRanking[2], 3, topTenRank.playerMelodiaSongFourDataList);
            mongoDBManager.GetAllUserData(mongoDBManager.MelodiaCollection, levelString.songFourLevelFour, topTenRank.melodiaPlayerRank, mongoDBManager.ID,
                mongoDBManager.melodiaSongFourLevelFourDict, topTenRank.melodiaSongFourRanking[3], 4, topTenRank.playerMelodiaSongFourDataList);
            #endregion
        }

        curUI = targetUIobj;
        Invoke(nameof(ShowRanking), 1f);
    }

    void ShowSketchUPRank(GameObject targetUIobj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);
        if (targetUIobj == dreamSketchUIGroup)
        {
            rankingInfo.gameObject.SetActive(true);

            mongoDBManager.GetAllUserData(mongoDBManager.DreamSketchCollection, levelString.levelOne, topTenRank.dreamSketchPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelOneDict, topTenRank.dreamSketchLevelShowRanking[0], 1,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.DreamSketchCollection, levelString.levelTwo, topTenRank.dreamSketchPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelTwoDict, topTenRank.dreamSketchLevelShowRanking[1], 2,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.DreamSketchCollection, levelString.levelThree, topTenRank.dreamSketchPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelThreeDict, topTenRank.dreamSketchLevelShowRanking[2], 3,
                topTenRank.playerDataList);

            mongoDBManager.GetAllUserData(mongoDBManager.DreamSketchCollection, levelString.levelFour, topTenRank.dreamSketchPlayerRank,
                mongoDBManager.ID, mongoDBManager.levelFourDict, topTenRank.dreamSketchLevelShowRanking[3], 4,
                topTenRank.playerDataList);
        }

        curUI = targetUIobj;
        Invoke(nameof(ShowRanking), 0.5f);
    }

    void DestroyRanking(GameObject targetUIobj, RectTransform[] pos)
    {
        if (curUI.activeSelf) curUI.SetActive(false);

        topTenRank.DestroyChild(pos, 0);
        topTenRank.DestroyChild(pos, 1);
        topTenRank.DestroyChild(pos, 2);
        topTenRank.DestroyChild(pos, 3);

        DestroyDictAndList();
        curUI = targetUIobj;
        curUI.SetActive(true);
    }

    void DestroyDictAndList()
    {
        mongoDBManager.levelOneDict.Clear();
        mongoDBManager.levelTwoDict.Clear();
        mongoDBManager.levelThreeDict.Clear();
        mongoDBManager.levelFourDict.Clear();

        topTenRank.playerDataList.Clear();
    }

    void DestroyMelodia(GameObject targetUIobj)
    {
        if (curUI.activeSelf) curUI.SetActive(false);

        DestroyChildObj(topTenRank.melodiaSongOneRanking);
        DestroyChildObj(topTenRank.melodiaSongTwoRanking);
        DestroyChildObj(topTenRank.melodiaSongThreeRanking);
        DestroyChildObj(topTenRank.melodiaSongFourRanking);

        DestroyMelodiaDictAndList();
        curUI = targetUIobj;
        curUI.SetActive(true);
    }

    void DestroyChildObj(RectTransform[] pos)
    {
        topTenRank.DestroyChild(pos, 0);
        topTenRank.DestroyChild(pos, 1);
        topTenRank.DestroyChild(pos, 2);
        topTenRank.DestroyChild(pos, 3);
    }

    void DestroyMelodiaDictAndList()
    {
        mongoDBManager.melodiaSongOneLevelOneDict.Clear();
        mongoDBManager.melodiaSongOneLevelTwoDict.Clear();
        mongoDBManager.melodiaSongOneLevelThreeDict.Clear();
        mongoDBManager.melodiaSongTwoLevelFourDict.Clear();

        mongoDBManager.melodiaSongTwoLevelOneDict.Clear();
        mongoDBManager.melodiaSongTwoLevelTwoDict.Clear();
        mongoDBManager.melodiaSongTwoLevelThreeDict.Clear();
        mongoDBManager.melodiaSongOneLevelFourDict.Clear();

        mongoDBManager.melodiaSongThreeLevelOneDict.Clear();
        mongoDBManager.melodiaSongThreeLevelTwoDict.Clear();
        mongoDBManager.melodiaSongThreeLevelThreeDict.Clear();
        mongoDBManager.melodiaSongThreeLevelFourDict.Clear();

        mongoDBManager.melodiaSongFourLevelOneDict.Clear();
        mongoDBManager.melodiaSongFourLevelTwoDict.Clear();
        mongoDBManager.melodiaSongFourLevelThreeDict.Clear();
        mongoDBManager.melodiaSongFourLevelFourDict.Clear();

        topTenRank.playerMelodiaSongOneDataList.Clear();
        topTenRank.playerMelodiaSongTwoDataList.Clear();
        topTenRank.playerMelodiaSongThreeDataList.Clear();
        topTenRank.playerMelodiaSongFourDataList.Clear();
    }
}
