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
    public const int EXTREME = 4;
}

//playtime type string constantize
public struct ScoreType
{
    public const string EASY = "levelOne";
    public const string NORMAL = "levelTwo";
    public const string HARD = "levelThree";
    public const string EXTREME = "levelFour";
}

public class PlayData
{
    public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}

public class SaveUserData : MonoBehaviour
{
    //==================================Database============================
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    public IMongoDatabase dataBase = null;
    public IMongoCollection<BsonDocument> Collection = null;

    //===================================CurData=============================
    public PlayData curPlayData = new();


    //===============================InternetConnection======================
    public bool OnlineMode;

    void Awake()
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
            Collection = dataBase.GetCollection<BsonDocument>("PoliRunRanking");
        }

        //delegate
        GameManager.Instance.gameClearRecord = SaveData;
    }

    private void Start()  
    {
        curPlayData.id = StartManager.curUserData.id;
        LoadData(curPlayData.id);
    }

    //==============================================Loading Data=================================================
    public void LoadData(string playerID)
    {
        if (OnlineMode) LoadMongoDB(playerID);
        else LoadLocalDB();
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
        BsonDocument document = Collection.Find(filter).FirstOrDefault();

        //online data init
        long onlineEasy = 0;
        long onlineNoraml = 0;
        long onlineHard = 0;
        long onlineExtreme = 0;

        //get local data
        int localEasy = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        int localNormal = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        int localHard = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        int localExtreme = PlayerPrefs.GetInt(ScoreType.EXTREME, 0);

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

            levelArry = (BsonArray)document.GetValue(ScoreType.EASY);
            onlineEasy = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.NORMAL);
            onlineNoraml = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.HARD);
            onlineHard = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.EXTREME);
            onlineExtreme = (long)levelArry[0];

            //get highter playtime
            curPlayData.levelOne[0] = localEasy > onlineEasy ? localEasy : onlineEasy;
            curPlayData.levelTwo[0] = localNormal > onlineNoraml ? localNormal : onlineNoraml;
            curPlayData.levelThree[0] = localHard > onlineHard ? localHard : onlineHard;
            curPlayData.levelFour[0] = localExtreme > onlineExtreme ? localExtreme : onlineExtreme;
        }
    }

    /// <summary>
    /// Get User ScoreData in playerfrefs
    /// </summary>
    private void LoadLocalDB()
    {
        curPlayData.levelOne[0] = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        curPlayData.levelTwo[0] = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        curPlayData.levelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        curPlayData.levelFour[0] = PlayerPrefs.GetInt(ScoreType.EXTREME, 0);
    }

    //===========================================Saving Data====================================================
    public void SaveData()
    {
        if (curPlayData.id == null) return;
        if (GetOldScore(StartManager.MyLevel) < GameManager.Instance.PlayTime)
        {
            switch (StartManager.MyLevel)
            {
                case 1: curPlayData.levelOne[0] = GameManager.Instance.PlayTime; break;
                case 2: curPlayData.levelTwo[0] = GameManager.Instance.PlayTime; break;
                case 3: curPlayData.levelThree[0] = GameManager.Instance.PlayTime; break;
                case 4: curPlayData.levelFour[0] = GameManager.Instance.PlayTime; break;
            }

            if (OnlineMode) SaveMongoDB(StartManager.MyLevel, GameManager.Instance.PlayTime);
            else SaveLocalDB(StartManager.MyLevel, GameManager.Instance.PlayTime);
        }
    }
    private void SaveMongoDB(int gameLevel, long playtime)
    {
        BsonDocument filter = new BsonDocument { { "_id", curPlayData.id } };
        BsonDocument targetData = Collection.Find(filter).FirstOrDefault();

        if (targetData == null)
        {
            SaveDefaultID(curPlayData.id);
            targetData = Collection.Find(filter).FirstOrDefault();
        }

        string levelString = null;

        switch (gameLevel)
        {
            case 1: levelString = ScoreType.EASY; break;
            case 2: levelString = ScoreType.NORMAL; break;
            case 3: levelString = ScoreType.HARD; break;
            case 4: levelString = ScoreType.EXTREME; break;
        }

        long[] levelData = new long[2];
        levelData[0] = playtime;
        levelData[1] = TimeSetting();

        UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levelString, levelData);
        Collection.UpdateOne(targetData, updatePoint);
    }
    private void SaveLocalDB(int gameLevel, long playtime)
    {
        switch (gameLevel)
        {
            case 1: PlayerPrefs.SetInt(ScoreType.EASY, (int)playtime); break;
            case 2: PlayerPrefs.SetInt(ScoreType.NORMAL, (int)playtime); break;
            case 3: PlayerPrefs.SetInt(ScoreType.HARD, (int)playtime); break;
            case 4: PlayerPrefs.SetInt(ScoreType.EXTREME, (int)playtime); break;
        }
    }

    #region create instance Id
    void SaveDefaultID(string id)
    {
        long curTime = TimeSetting();

        SaveDataBase(id, 0, curTime);
    }
    public async void SaveDataBase(string id, long point, long time)
    {
        PlayData newPlayData = new();

        newPlayData.id = id;

        newPlayData.levelOne[0] = point;
        newPlayData.levelOne[1] = time;

        newPlayData.levelTwo[0] = point;
        newPlayData.levelTwo[1] = time;

        newPlayData.levelThree[0] = point;
        newPlayData.levelThree[1] = time;

        newPlayData.levelFour[0] = point;
        newPlayData.levelFour[1] = time;

        await Collection.InsertOneAsync(newPlayData.ToBsonDocument());
    }
    #endregion
    

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

    //===============================================Get Play Time================================================
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm");
        long time = long.Parse(nowDate);
        return time;
    }

}