using Kum;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public struct Level
{
    public const int EASY = 1;
    public const int NORMAL = 2;
    public const int HARD = 3;
    public const int VERYHARD = 4;
}

//score type string constantize
public struct ScoreType
{
    public const string EASY     = "levelOne";
    public const string NORMAL   = "levelTwo";
    public const string HARD     = "levelThree";
    public const string VERYHARD = "levelFour";
}

[Serializable]
public struct PlayLevelData
{
    public int score;
    public float date;
}

[Serializable]
public class HeyCookData
{
    [SerializeField] public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}

[Serializable]
public struct UserData
{
    public string id;
    public int charIndex;
    public bool troughTown;
}


public class SaveLoadManger : MonoSingleTon<SaveLoadManger>
{
    //==================================Database============================
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    public IMongoDatabase dataBase = null;
    public IMongoCollection<BsonDocument> HeyCookCollection = null;

    //===================================CurData=============================
    public HeyCookData curPlayData;
    public UserData curUserData;

    #if UNITY_EDITOR
    [SerializeField] private string localSavePath = "/MetaBox/SaveData/SaveData.json";
    #else
    private string localSavePath = "/storage/emulated/0/MetaBox/SaveData/SaveData.json";
    #endif

    //===============================InternetConnection======================
    public bool OnlineMode;

    private void Start()
    {
        //check internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnlineMode = false;
        }
        else
        {
            OnlineMode = true;

            // MongoDB database name
            dataBase = clientData.GetDatabase("RankingDB");
            // MongoDB collection name
            HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
        }

        //delegate chain
        EventReceiver.saveCallBack += SaveData;

        FileCheck();
        LoadData(GetID());
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReceiver.saveCallBack -= SaveData;
    }

    //===========================================Saving Data====================================================
    public void SaveData(int level, long score)
    {
        switch (level)
        {
            case 1: curPlayData.levelOne[0] = score; break;
            case 2: curPlayData.levelTwo[0] = score; break;
            case 3: curPlayData.levelThree[0] = score; break;
            case 4: curPlayData.levelFour[0] = score; break;
        }

        if (OnlineMode) SaveMongoDB(level, score);
        else SaveLocalDB(level, score);
    }
    private void SaveMongoDB(int gameLevel, long score)
    {
        BsonDocument filter = new() { { "_id", GetID() } };
        BsonDocument targetData = HeyCookCollection.Find(filter).FirstOrDefault();

        if (targetData == null)
        {
            SaveDefaultID(GetID());
            targetData = HeyCookCollection.Find(filter).FirstOrDefault();
        }

        string levelString = null;

        switch (gameLevel)
        {
            case 1: levelString = ScoreType.EASY; break;
            case 2: levelString = ScoreType.NORMAL; break;
            case 3: levelString = ScoreType.HARD; break;
            case 4: levelString = ScoreType.VERYHARD; break;
        }

        long[] levelData = new long[2];
        levelData[0] = score;
        levelData[1] = TimeSetting();

        UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levelString, levelData);
        HeyCookCollection.UpdateOne(targetData, updatePoint);
    }
    private void SaveLocalDB(int gameLevel, long score)
    {
        switch (gameLevel)
        {
            case 1: PlayerPrefs.SetInt(ScoreType.EASY, (int)score); break;
            case 2: PlayerPrefs.SetInt(ScoreType.NORMAL, (int)score); break;
            case 3: PlayerPrefs.SetInt(ScoreType.HARD, (int)score); break;
            case 4: PlayerPrefs.SetInt(ScoreType.VERYHARD, (int)score); break;
        }
    }

    #region create instance Id
    void SaveDefaultID(string id)
    {
        HeyCookData heyCookData = new();

        long curTime = TimeSetting();

        SaveHeyCookDataBase(heyCookData, id, 0, curTime);
    }
    public async void SaveHeyCookDataBase(HeyCookData newData, string id, long point, long time)
    {
        newData.id = id;

        newData.levelOne[0] = point;
        newData.levelOne[1] = time;

        newData.levelTwo[0] = point;
        newData.levelTwo[1] = time;

        newData.levelThree[0] = point;
        newData.levelThree[1] = time;

        newData.levelFour[0] = point;
        newData.levelFour[1] = time;

        await HeyCookCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    #endregion

    //==============================================Loading Data=================================================
    public void LoadData(string playerID)
    {
        if (OnlineMode) LoadMongoDB(playerID);
        else LoadLocalDB();
    }
    /// <summary>
    /// Get User ScoreData in playerfrefs
    /// </summary>
    private void LoadLocalDB()
    {
        curPlayData.levelOne[0]   = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        curPlayData.levelTwo[0]   = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        curPlayData.levelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        curPlayData.levelFour[0]  = PlayerPrefs.GetInt(ScoreType.VERYHARD, 0);
    }
    /// <summary>
    /// Get User ScoreData in MongoDB
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    private void LoadMongoDB(string playerID)
    {
        //apply filter to find
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", playerID);

        //find data in collection using filter
        BsonDocument document = HeyCookCollection.Find(filter).FirstOrDefault();

        //online data init
        long onlineEasy;
        long onlineNoraml;
        long onlineHard;
        long onlineVeryHard;

        //get local data
        int localEasy = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        int localNormal = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        int localHard = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        int localVeryHard = PlayerPrefs.GetInt(ScoreType.VERYHARD, 0);

        //return pickup Data if was not null
        if (document == null)
        {
            //create new account
            SaveDefaultID(curPlayData.id);
        }
        else
        {
            //get online data
            BsonArray levelArry;

            levelArry = (BsonArray)document.GetValue("levelOne");
            onlineEasy = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue("levelTwo");
            onlineNoraml = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue("levelThree");
            onlineHard = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue("levelFour");
            onlineVeryHard = (long)levelArry[0];

            //get highter score
            curPlayData.levelOne[0] = localEasy > onlineEasy ? localEasy : onlineEasy;
            curPlayData.levelTwo[0] = localNormal > onlineNoraml ? localNormal : onlineNoraml;
            curPlayData.levelThree[0] = localHard > onlineHard ? localHard : onlineHard;
            curPlayData.levelFour[0] = localVeryHard > onlineVeryHard ? localVeryHard : onlineVeryHard;

            return;
        }
    }

    //=============================================Getting Data===================================================
    public string GetID() => curPlayData.id;
    public int GetOldScore(int level)
    {
        switch (level)
        {
            case 1: return (int)curPlayData.levelOne[0];
            case 2: return (int)curPlayData.levelTwo[0];
            case 3: return (int)curPlayData.levelThree[0];
            case 4: return (int)curPlayData.levelFour[0];
            default: return 0;
        }
    }

    //=============================================Check Local File===============================================
    private void FileCheck()
    {
        if (File.Exists(localSavePath))
        {
            curUserData = ReadSaveData(localSavePath);
        }
        else//존재하지 않을때 
        {
            curUserData.id = "전설의연습생";
        }

        curPlayData.id = curUserData.id;
    }
    private UserData ReadSaveData(string path)
    {
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

        return readData;
    }

    //===============================================Get Play Time================================================
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm");
        long time = long.Parse(nowDate);
        return time;
    }
}