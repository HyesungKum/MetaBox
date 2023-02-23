using MongoDB.Bson;
using MongoDB.Driver;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region Data Structor
[Serializable]
public class FreezeData
{
    public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}
[Serializable]
public class HeyCookData
{
    public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}
[Serializable]
public class MelodiaData
{
    public string id;

    public long[] songOneLevelOne = new long[2];
    public long[] songOneLevelTwo = new long[2];
    public long[] songOneLevelThree = new long[2];
    public long[] songOneLevelFour = new long[2];

    public long[] songTwoLevelOne = new long[2];
    public long[] songTwoLevelTwo = new long[2];
    public long[] songTwoLevelThree = new long[2];
    public long[] songTwoLevelFour = new long[2];

    public long[] songThreeLevelOne = new long[2];
    public long[] songThreeLevelTwo = new long[2];
    public long[] songThreeLevelThree = new long[2];
    public long[] songThreeLevelFour = new long[2];

    public long[] songFourLevelOne = new long[2];
    public long[] songFourLevelTwo = new long[2];
    public long[] songFourLevelThree = new long[2];
    public long[] songFourLevelFour = new long[2];
}
public class MelodiaSongArry
{
    public long[] levelOne = new long[3];
    public long[] levelTwo = new long[3];
    public long[] levelThrrr = new long[3];
    public long[] levelFour = new long[3];
}
[Serializable]
public class SketchUpData
{
    public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}
#endregion

public class MongoIDManager : MonoBehaviour
{
    readonly MongoClient clientData = new("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase dataBase = null;
    IMongoCollection<BsonDocument> FreezeCollection = null;
    IMongoCollection<BsonDocument> HeyCookCollection = null;
    IMongoCollection<BsonDocument> MelodiaCollection = null;
    IMongoCollection<BsonDocument> SketchUpCollection = null;

    [Header("[Login UserID]")]
    [SerializeField] TMP_InputField inputID = null;
    [SerializeField] TextMeshProUGUI infoText = null;
    [SerializeField] Button IDSaveButton = null;
    [SerializeField] ChangeObject changeObject = null;

    public bool OnlineMode = true;
    int GetCharIndex => changeObject.index;
    private string id;
    public string ID { get { return id; } set { id = value; } }

    void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnlineMode = false;
            return;
        }

        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");

        // MongoDB collection name
        FreezeCollection = dataBase.GetCollection<BsonDocument>("FreezeRanking");
        HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
        MelodiaCollection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
        SketchUpCollection = dataBase.GetCollection<BsonDocument>("SketchUpRanking");

        // === Button Event Setting ===
        IDSaveButton.interactable = false;
        inputID.onValueChanged.AddListener(delegate { CheckInputId(); });
        IDSaveButton.onClick.AddListener(delegate { SaveID(); });
    }

    void CheckInputId()
    {
        if (string.IsNullOrEmpty(inputID.text))
        {
            IDSaveButton.interactable = false;
        }
        else if (inputID.text.Length <= 10)
        {
            IDSaveButton.interactable = true;
        }
    }
    void SaveID()
    {
        ID = inputID.text;

        if (!OnlineMode)
        {
            EventReceiver.CallSaveEvent(ID, GetCharIndex, true);
            return;
        }

        BsonDocument checkFreezeID = CheckFreezeID(ID);
        BsonDocument checkHeyCookID = CheckHeyCookID(ID);
        BsonDocument checkMelodiaID = CheckMelodiaID(ID);
        BsonDocument checkSketchUpID = CheckSketchUpID(ID);

        if (checkFreezeID == null && checkHeyCookID == null &&
            checkMelodiaID == null && checkSketchUpID == null)
        {
            IDSaveButton.interactable = false;
            inputID.interactable = false;
            infoText.gameObject.SetActive(false);

            FreezeData freezeData = new();
            HeyCookData heyCookData = new();
            MelodiaData melodiaData = new();
            SketchUpData sketchUpData = new();

            long curTime = TimeSetting();

            SaveFreezeDataBase(freezeData, ID, 0, curTime);
            SaveHeyCookDataBase(heyCookData, ID, 0, curTime);
            SaveMelodiaDataBase(melodiaData, ID, 0, curTime);
            SaveSketchUpDataBase(sketchUpData, ID, 0, curTime);

            EventReceiver.CallSaveEvent(ID, GetCharIndex, true);
        }
        else
        {
            infoText.gameObject.SetActive(true);
            IDSaveButton.interactable = false;
        }
    }

    //==============================================================Check ID Overlap on MongoDB===========================================================
    BsonDocument CheckFreezeID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = FreezeCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckHeyCookID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = HeyCookCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckMelodiaID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = MelodiaCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckSketchUpID(string findId)
    {
        BsonDocument filter = new() { { "_id", findId } };
        BsonDocument targetData = SketchUpCollection.Find(filter).FirstOrDefault();

        return targetData;
    }

    //==================================================================Save Default Data=================================================================
    public async void SaveFreezeDataBase(FreezeData newData, string id, long point, long time)
    {
        newData.id = id;
        newData.levelOne[0] = point;
        newData.levelOne[1] = TimeSetting();

        newData.levelTwo[0] = point;
        newData.levelTwo[1] = TimeSetting();

        newData.levelThree[0] = point;
        newData.levelThree[1] = TimeSetting();

        newData.levelFour[0] = point;
        newData.levelFour[1] = TimeSetting();
        await FreezeCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveHeyCookDataBase(HeyCookData newData, string id, long point, long time)
    {
        newData.id = id;
        newData.levelOne[0] = point;
        newData.levelOne[1] = TimeSetting();

        newData.levelTwo[0] = point;
        newData.levelTwo[1] = TimeSetting();

        newData.levelThree[0] = point;
        newData.levelThree[1] = TimeSetting();

        newData.levelFour[0] = point;
        newData.levelFour[1] = TimeSetting();

        await HeyCookCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveMelodiaDataBase(MelodiaData newData, string id, long point, long time)
    {
        newData.id = id;
        #region
        newData.songOneLevelOne[0] = point;
        newData.songOneLevelOne[1] = TimeSetting();
        newData.songOneLevelTwo[0] = point;
        newData.songOneLevelTwo[1] = TimeSetting();
        newData.songOneLevelThree[0] = point;
        newData.songOneLevelThree[1] = TimeSetting();
        newData.songOneLevelFour[0] = point;
        newData.songOneLevelFour[1] = TimeSetting();

        newData.songTwoLevelOne[0] = point;
        newData.songTwoLevelOne[1] = TimeSetting();
        newData.songTwoLevelTwo[0] = point;
        newData.songTwoLevelTwo[1] = TimeSetting();
        newData.songTwoLevelThree[0] = point;
        newData.songTwoLevelThree[1] = TimeSetting();
        newData.songTwoLevelFour[0] = point;
        newData.songTwoLevelFour[1] = TimeSetting();

        newData.songThreeLevelOne[0] = point;
        newData.songThreeLevelOne[1] = TimeSetting();
        newData.songThreeLevelTwo[0] = point;
        newData.songThreeLevelTwo[1] = TimeSetting();
        newData.songThreeLevelThree[0] = point;
        newData.songThreeLevelThree[1] = TimeSetting();
        newData.songThreeLevelFour[0] = point;
        newData.songThreeLevelFour[1] = TimeSetting();

        newData.songFourLevelOne[0] = point;
        newData.songFourLevelOne[1] = TimeSetting();
        newData.songFourLevelTwo[0] = point;
        newData.songFourLevelTwo[1] = TimeSetting();
        newData.songFourLevelThree[0] = point;
        newData.songFourLevelThree[1] = TimeSetting();
        newData.songFourLevelFour[0] = point;
        newData.songFourLevelFour[1] = TimeSetting();
        #endregion
        await MelodiaCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveSketchUpDataBase(SketchUpData newData, string id, long point , long time)
    {
        newData.id = id;
        newData.levelOne[0] = point;
        newData.levelOne[1] = TimeSetting();

        newData.levelTwo[0] = point;
        newData.levelTwo[1] = TimeSetting();

        newData.levelThree[0] = point;
        newData.levelThree[1] = TimeSetting();

        newData.levelFour[0] = point;
        newData.levelFour[1] = TimeSetting();
        await SketchUpCollection.InsertOneAsync(newData.ToBsonDocument());
    }

    //============================================== Play Game Time Setting : Year/Month/Day/Hour/Minute=================================================
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // 현재 시간
        long time = long.Parse(nowDate);
        return time;
    }
}
