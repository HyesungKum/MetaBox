using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using Unity.VisualScripting;

public class InGamePanelSet : MonoBehaviour
{
    #region Singleton
    private static InGamePanelSet instance;
    public static InGamePanelSet Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGamePanelSet>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(InGamePanelSet), typeof(InGamePanelSet)).GetComponent<InGamePanelSet>();

                }
            }
            return instance;
        }
    }
    #endregion

    #region SerializeField
    [Header("[InGame Panel Set]")]
    [SerializeField] GameObject inGameCanvas = null;
    [SerializeField] GameObject selectPanel = null;
    [SerializeField] GameObject closePlayOneBrush = null;
    [SerializeField] GameObject losePanel = null;
    [SerializeField] GameObject winPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject stageClearPanel = null;

    [Header("[OneBrush Paly Obj]")]
    [SerializeField] GameObject onBushObj = null;
    [SerializeField] GameObject QOne = null;
    [SerializeField] GameObject QTwo = null;
    [SerializeField] GameObject QThree = null;

    [Header("[Line Color and Size Change]")]
    [SerializeField] GameObject lineChangedPanel = null;
    [SerializeField] GameObject colorPanelObj = null;
    [SerializeField] GameObject LineSizeObj = null;

    [Header("[ReStart]")]
    [SerializeField] Button loseReStartBut = null;
    [SerializeField] Button winReStartBut = null;

    [Header("[Go SelectPanel]")]
    [SerializeField] Button goSelectPanelBut = null;

    [Header("[InGame Button]")]
    [SerializeField] Button optionBut = null;
    [SerializeField] Button closePanelBut = null;

    [Header("[Option Button Setting]")]
    [SerializeField] Button quitBut = null;
    [SerializeField] Button resumeBut = null;

    [Header("[Wait Time Img]")]
    [SerializeField] GameObject waitTimeObjs = null;

    [Header("[Timer Text]")]
    [SerializeField] TextMeshProUGUI playTimeText = null;
    [SerializeField] GameObject clockPrefab = null;

    [Header("[Character Move]")]
    [SerializeField] GameObject characterMove = null;

    [Header("[Game Clear Effect ]")]
    [SerializeField] GameObject gameClearEffect = null;
    [SerializeField] GameObject stageClearEffect = null;

    [Header("[Play Time Setting]")]
    [SerializeField] private int minute;
    [SerializeField] private float seconds;
    [SerializeField] int playPoint;
    [SerializeField] string id;

    public float Secondes { get { return seconds; } set { seconds = value; } }
    public int Minute { get { return minute; } set { minute = value; } }

    #endregion

    [Header("[Others]")]
    [SerializeField] ClearAnimalImgData clearAnimalImgData = null;

    [Header("[Production]")]
    [SerializeField] GameObject production;
    [SerializeField] GameObject viewHall;

    [Header("[Ranking]")]
    [SerializeField] TextMeshProUGUI playerPoint = null;
    [SerializeField] TextMeshProUGUI rankingUpdata = null;

    [Space]
    public LineColorChanged ColorPanel;
    public LineSizeChange LineSize;

    GameObject instClock = null;
    GameObject instEffect = null;
    GameObject clearEffect = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds waitOnSceonds = null;

    // === ClearCount ===
    public int clearCount = 3;
    public int ClearCount { get { return clearCount; } set { clearCount = value; } }

    // === Object Index ===
    public int ObjIndexs;
    public int ObjTwoIndex;
    public int ObjThreeIndex;

    int levelIndexCheck;
    public int totalPlayTime;
    public long savePalyTime;
    long GetPoint;

    bool isOptionPanelOpen = false;

    void Awake()
    {
        if (clearAnimalImgData == null)
            clearAnimalImgData = Resources.Load<ClearAnimalImgData>("Data/ClearAnimalImgData");

        #region MongoDB database
        id = LoadIDMgr.Inst.curUserData.id;
        //Debug.Log("id : " + id);
        #endregion

        levelIndexCheck = SoundManager.Inst.LevelIndex;

        waitHalf = new WaitForSeconds(0.5f);
        waitOnSceonds = new WaitForSeconds(1f);

        Time.timeScale = 1;
        colorPanelObj.TryGetComponent<LineColorChanged>(out ColorPanel);
        LineSizeObj.TryGetComponent<LineSizeChange>(out LineSize);

        Production();
        playTimeText.gameObject.SetActive(false);

        // === changed (false) ===
        FirstSet(false);
        optionBut.gameObject.SetActive(false);
        OneBrushPlayPanelSet(false);

        #region Button Event Setting
        loseReStartBut.onClick.AddListener(delegate
        {
            OnClickGoStartPanel();
            SoundManager.Inst.GameLoseSFXPlay();
            SoundManager.Inst.ButtonEffect(loseReStartBut.transform.position);
        });

        winReStartBut.onClick.AddListener(delegate
        {
            OnClickGoStartPanel();
            SoundManager.Inst.GameClearSFXPlay();
            SoundManager.Inst.ButtonEffect(winReStartBut.transform.position);
        });

        quitBut.onClick.AddListener(delegate
        {
            OnClickGoStartPanel();
            SoundManager.Inst.ButtonSFXPlay();
            SoundManager.Inst.ButtonEffect(quitBut.transform.position);
        });

        optionBut.onClick.AddListener(delegate
        {
            SoundManager.Inst.ButtonEffect(optionBut.transform.position);
            Invoke(nameof(OnClickOptionBut), 0.4f);
            SoundManager.Inst.ButtonSFXPlay();
        });

        resumeBut.onClick.AddListener(delegate
        {
            OnClickOptionBut();
            SoundManager.Inst.ButtonSFXPlay();
            SoundManager.Inst.ButtonEffect(resumeBut.transform.position);
        });

        closePanelBut.onClick.AddListener(delegate
        {
            OnClickInGameGoSelectPanel();
            SoundManager.Inst.ButtonSFXPlay();
            SoundManager.Inst.ButtonEffect(closePanelBut.transform.position);
        });

        goSelectPanelBut.onClick.AddListener(delegate
        {
            OnClickInGameGoSelectPanel();
            SoundManager.Inst.ButtonSFXPlay();
            SoundManager.Inst.ButtonEffect(goSelectPanelBut.transform.position);
        });
        #endregion
    }

    void Update()
    {
        #region Timer Setting
        if (playTimeText.gameObject.active == true)
        {
            PlayTimeDown();
        }
        #endregion
    }

    IEnumerator CountDowns()
    {
        CountDown countDown = null;
        waitTimeObjs.TryGetComponent<CountDown>(out countDown);

        int waitTime = 3;
        for (int i = waitTime; i > 0; i--)
        {
            countDown.ShowWaitTime(waitTime);
            waitTime--;
            yield return waitOnSceonds;
        }

        waitTimeObjs.gameObject.SetActive(false);
        yield return waitHalf;
        FirstSet(true);
        optionBut.gameObject.SetActive(true);
        playTimeText.gameObject.SetActive(true);
    }

    void PlayTimeDown()
    {
        if (clearCount != 0) seconds -= 1 * Time.deltaTime;

        playTimeText.text = string.Format("{0:D2} : {1:D2}", Minute, (int)Secondes);

        if ((int)Secondes < 0)
        {
            Secondes = 59;
            Minute -= 1;
        }

        if (Minute == 0 && (int)Secondes == 30)
        {
            if (instClock == null)
            {
                instClock = ObjectPoolCP.PoolCp.Inst.BringObjectCp(clockPrefab);
                SoundManager.Inst.AlarmClockSFXPlay();
            }
        }

        if (Minute == 0 && (int)Secondes == 29)
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instClock);

        else if (Minute == 0 && (int)Secondes <= 0)
        {
            playTimeText.text = $"00 : 00";
            playTimeText.gameObject.SetActive(false);
            OneBrushPlayPanelSet(false);
            LineColorAndSizeChange(false);
            LosePanelSet(true);
        }

        if (clearCount == 0)
        {
            LineColorAndSizeChange(false);
            OneBrushPlayPanelSet(false);
            savePalyTime = (Minute * 60) + (int)Secondes;
            Invoke(nameof(WinPanelSetting), 0.3f);

            return;
        }
    }

    void FirstSet(bool selectPanel)
    {
        inGameCanvas.gameObject.SetActive(true);
        SelectPanelSet(selectPanel);
        CharacterMoveSet(selectPanel);
        LineColorAndSizeChange(false);
        InGameSet(false);
        InGameOptionSet(false);
        WinPanelSet(false);
        LosePanelSet(false);
    }

    public void QOneSet(bool active) => QOne.gameObject.SetActive(active);
    public GameObject QOneObj() => QOne.gameObject;
    public void QTwoSet(bool active) => QTwo.gameObject.SetActive(active);
    public GameObject QTwoObj() => QTwo.gameObject;
    public void QThreeSet(bool active) => QThree.gameObject.SetActive(active);
    public GameObject QThreeObj() => QThree.gameObject;
    void InstGameClearEffect() => instEffect = ObjectPoolCP.PoolCp.Inst.BringObjectCp(gameClearEffect);
    public void SelectPanelSet(bool active) => selectPanel.gameObject.SetActive(active);
    public void InGameSet(bool active) => closePlayOneBrush.gameObject.SetActive(active);
    public void CharacterMoveSet(bool active) => characterMove.gameObject.SetActive(active);
    public void InGameOptionSet(bool active) => optionPanel.gameObject.SetActive(active);
    public void OneBrushPlayPanelSet(bool active) => onBushObj.gameObject.SetActive(active);
    public void LosePanelSet(bool active) => losePanel.gameObject.SetActive(active);
    public void WinPanelSet(bool active) => winPanel.gameObject.SetActive(active);
    public void StageClearPanelSet(bool active) => stageClearPanel.gameObject.SetActive(active);
    public void LineColorAndSizeChange(bool active) => lineChangedPanel.gameObject.SetActive(active);
    public void ProductionSet(bool active) => production.gameObject.SetActive(active);
    void Production() => StartCoroutine(ViewHallExtension());
    public void MoveScene(int sceneIndex) => StartCoroutine(ViewHallShrink(sceneIndex));
    public void InstAnimalClearEffect() => clearEffect = ObjectPoolCP.PoolCp.Inst.BringObjectCp(stageClearEffect);

    IEnumerator ViewHallExtension()
    {
        ProductionSet(true);
        float timer = 0f;
        Time.timeScale = 1;

        while (viewHall.transform.localScale.x <= 30)
        {
            timer += Time.deltaTime / 20f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 35f, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.one * 30f;

        ProductionSet(false);
        waitTimeObjs.gameObject.SetActive(true);
        StartCoroutine(CountDowns());
    }

    IEnumerator ViewHallShrink(int sceneIndex)
    {
        ProductionSet(true);

        float timer = 0f;
        while (viewHall.transform.localScale.x >= 0.8f)
        {
            timer += Time.deltaTime / 15f;

            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.zero, timer);
            yield return null;
        }

        viewHall.transform.localScale = Vector3.zero;
        SceneManager.LoadScene(sceneIndex);
    }

    public void DestroyAnimalClearEffect()
    {
        if (clearEffect != null)
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(clearEffect);
    }

    public void OnClickGoStartPanel()
    {
        Time.timeScale = 1;
        MoveScene(SceneName.StartScene);
        SoundManager.Inst.TitleBGMPlay(); // 타이틀 BGM 바꿔주기
    }

    public void OnClickOptionBut()
    {
        if (isOptionPanelOpen == false)
        {
            InGameOptionSet(true);
            OneBrushPlayPanelSet(false);
            InGameSet(false);
            LineColorAndSizeChange(false);
            Time.timeScale = 0;
            isOptionPanelOpen = true;
        }
        else if (isOptionPanelOpen == true)
        {
            InGameOptionSet(false);

            if (selectPanel.active == true)
            {
                OneBrushPlayPanelSet(false);
            }
            else
            {
                OneBrushPlayPanelSet(true);
                InGameSet(true);
                LineColorAndSizeChange(true);
            }

            Time.timeScale = 1;
            isOptionPanelOpen = false;
        }
    }

    void OnClickInGameGoSelectPanel()
    {
        SelectPanelSet selectPanelSet = null;
        selectPanel.TryGetComponent<SelectPanelSet>(out selectPanelSet);
        DrawLineCurve drawLine = null;
        QOne.transform.GetChild(0).TryGetComponent<DrawLineCurve>(out drawLine);
        ObjIndexs = drawLine.ObjIndex;

        QTwo.transform.GetChild(0).TryGetComponent<DrawLineCurve>(out drawLine);
        ObjTwoIndex = drawLine.ObjIndex;

        QThree.transform.GetChild(0).TryGetComponent<DrawLineCurve>(out drawLine);
        ObjThreeIndex = drawLine.ObjIndex;

        SelectPanelSet(true);
        DestroyAnimalClearEffect();
        SelectPanelClearImgSet(selectPanelSet);

        selectPanelSet.character.transform.localPosition = new Vector2(915f, 400f);
        OneBrushPlayPanelSet(false);
        StageClearPanelSet(false);
        InGameSet(false);
    }

    void SelectPanelClearImgSet(SelectPanelSet selectPanelSet)
    {
        Image selectImage = null;
        Button button = null;

        if (SoundManager.Inst.LevelIndex == 1 && ObjIndexs == 5)
        {
            selectPanelSet.oneBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelOneClearAniamlImg[0];
            selectPanelSet.oneBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 1 && ObjTwoIndex == 5)
        {
            selectPanelSet.twoBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelOneClearAniamlImg[1];
            selectPanelSet.twoBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 1 && ObjThreeIndex == 5)
        {
            selectPanelSet.threeBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelOneClearAniamlImg[2];
            selectPanelSet.threeBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 2 && ObjIndexs == 5)
        {
            selectPanelSet.oneBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelTwoClearAniamlImg[0];
            selectPanelSet.oneBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 2 && ObjTwoIndex == 5)
        {
            selectPanelSet.twoBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelTwoClearAniamlImg[1];
            selectPanelSet.twoBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 2 && ObjThreeIndex == 5)
        {
            selectPanelSet.threeBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelTwoClearAniamlImg[2];
            selectPanelSet.threeBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 3 && ObjIndexs == 5)
        {
            selectPanelSet.oneBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelThreeClearAniamlImg[0];
            selectPanelSet.oneBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 3 && ObjTwoIndex == 5)
        {
            selectPanelSet.twoBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelThreeClearAniamlImg[1];
            selectPanelSet.twoBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 3 && ObjThreeIndex == 5)
        {
            selectPanelSet.threeBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelThreeClearAniamlImg[2];
            selectPanelSet.threeBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 4 && ObjIndexs == 5)
        {
            selectPanelSet.oneBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelFourClearAniamlImg[0];
            selectPanelSet.oneBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 4 && ObjTwoIndex == 5)
        {
            selectPanelSet.twoBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelFourClearAniamlImg[1];
            selectPanelSet.twoBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
        if (SoundManager.Inst.LevelIndex == 4 && ObjThreeIndex == 5)
        {
            selectPanelSet.threeBrushBut.TryGetComponent<Image>(out selectImage);
            selectImage.sprite = clearAnimalImgData.LevelFourClearAniamlImg[2];
            selectPanelSet.threeBrushBut.TryGetComponent<Button>(out button);
            button.enabled = false;
        }
    }

    void WinPanelSetting()
    {
        WinPanelSet(true);
        rankingUpdata.gameObject.SetActive(false);
        playPoint = 600;
        playerPoint.text = $"점수 : {savePalyTime}";

        ChangedPoint(savePalyTime);

        if (instEffect == null)
            InstGameClearEffect();
    }

    #region Changed Point
    public void ChangedPoint(long point)
    {
        BsonDocument filter = new BsonDocument { { "_id", id } };
        BsonDocument targetData = LoadIDMgr.Inst.DreamSketchCollection.Find(filter).FirstOrDefault();

        BsonArray levelArry;
        string levelNum = CheckLevel();
        levelArry = (BsonArray)targetData.GetValue(levelNum);
        GetPoint = (long)levelArry[0];

        if (savePalyTime > GetPoint)
        {
            rankingUpdata.gameObject.SetActive(true);

            long[] level = new long[2];
            level[0] = point;
            level[1] = LoadIDMgr.Inst.TimeSetting();

            UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levelNum, level);
            LoadIDMgr.Inst.DreamSketchCollection.UpdateOne(targetData, updatePoint);
        }
    }
    #endregion

    string CheckLevel()
    {
        if (levelIndexCheck == 1)
        {
            return "levelOne";
        }
        else if (levelIndexCheck == 2)
        {
            return "levelTwo";
        }
        else if (levelIndexCheck == 3)
        {
            return "levelThree";
        }
        else if (levelIndexCheck == 4)
        {
            return "levelFour";
        }
        else
        {
            return null;
        }
    }
}