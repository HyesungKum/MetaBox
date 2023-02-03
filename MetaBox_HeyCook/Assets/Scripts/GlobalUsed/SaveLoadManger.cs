using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using UnityEngine;
using Kum;
using System;
using System.Collections;

public enum Difficulty
{
    EASY = 0,
    NORMAL = 1,
    HARD = 2,
    VERYHARD = 3
}

public struct ScoreType
{
    public const string EASY = "easyScore";
    public const string NORMAL = "nomalScore";
    public const string HARD = "hardScore";
    public const string VERYHARD = "veryHardScore";
}

[Serializable]
public struct PlayLevelData
{
    public int score;
    public float date;
}

[Serializable]
public class PlayData
{
    [SerializeField] public string id;

    public PlayLevelData[] levelDatas = 
    { 
        new PlayLevelData(), 
        new PlayLevelData(), 
        new PlayLevelData(), 
        new PlayLevelData() 
    };
}

public class SaveLoadManger : MonoSingleTon<SaveLoadManger>
{
    //==================================Database============================
    readonly MongoClient clientData = new("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majori");

    private IMongoDatabase dataBase;
    private IMongoCollection<BsonDocument> collection;

    //===================================CurData=============================
    [SerializeField] private PlayData playData;

    //===============================InternetConnection======================
    [SerializeField] private bool OnlineMode;

    protected override void Awake()
    {
        base.Awake();

        ////check internet connection
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    OnlineMode = true;
        //
        //    // MongoDB database name
        //    dataBase = clientData.GetDatabase("RankingDB");
        //    // MongoDB collection name
        //    collection = dataBase.GetCollection<BsonDocument>("HeyCookRank");
        //}
        //else
        //{
        //    OnlineMode = false;
        //}
        playData.id = "guest";
        OnlineMode = false;

        //delegate chain
        EventReciver.saveCallBack += SaveData;
    }

    private void Start()
    {
        LoadData(playData.id);
    }

    //===========================================Saving Data====================================================
    public void SaveData(int level, int score)
    {
        if (OnlineMode) SaveMongoDB(level, score);
        else SaveLocalDB(level, score);
    }

    /// <summary>
    /// New User Data Save in DB
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="gameLevel"></param>
    /// <param name="score"></param>
    /// <param name="date"></param>
    private async void SaveMongoDB(int gameLevel, int score)
    {
        if (string.IsNullOrWhiteSpace(playData.id)) return;

        BsonDocument playerData = new()
        { 
            { "playerID",  playData.id }, 
            { "gameLevel", gameLevel }, 
            { "score",     score     }
        };

        await collection.InsertOneAsync(playerData);
        Debug.Log("## MongoDB Database Save Working Done : " + playerData.ToString());
    }
    private void SaveLocalDB(int gameLevel, int score)
    {
        switch (gameLevel)
        {
            case 0: PlayerPrefs.SetInt(ScoreType.EASY, score); break;
            case 1: PlayerPrefs.SetInt(ScoreType.NORMAL, score); break;
            case 2: PlayerPrefs.SetInt(ScoreType.HARD, score); break;
            case 3: PlayerPrefs.SetInt(ScoreType.VERYHARD, score); break;
        }

        LoadData(playData.id);
    }

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
        playData.levelDatas[(int)Difficulty.EASY].score     = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        playData.levelDatas[(int)Difficulty.NORMAL].score   = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        playData.levelDatas[(int)Difficulty.HARD].score     = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        playData.levelDatas[(int)Difficulty.VERYHARD].score = PlayerPrefs.GetInt(ScoreType.VERYHARD, 0);
    }
    /// <summary>
    /// Get User ScoreData in MongoDB
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    private void LoadMongoDB(string playerID)
    {
        //apply filter to find
        var filter = Builders<BsonDocument>.Filter.Eq("playerID", playerID);

        //find data in collection using filter
        BsonDocument nullFilter = collection.Find(filter).FirstOrDefault();

        //return pickup Data if was not null
        if (nullFilter != null)
        {
            playData.levelDatas[(int)Difficulty.EASY].score     = (int)nullFilter.GetValue(ScoreType.EASY);
            playData.levelDatas[(int)Difficulty.NORMAL].score   = (int)nullFilter.GetValue(ScoreType.NORMAL);
            playData.levelDatas[(int)Difficulty.HARD].score     = (int)nullFilter.GetValue(ScoreType.HARD);
            playData.levelDatas[(int)Difficulty.VERYHARD].score = (int)nullFilter.GetValue(ScoreType.VERYHARD);
        }
        else
        {
            playData.levelDatas[(int)Difficulty.EASY].score     = 0;
            playData.levelDatas[(int)Difficulty.NORMAL].score   = 0;
            playData.levelDatas[(int)Difficulty.HARD].score     = 0;
            playData.levelDatas[(int)Difficulty.VERYHARD].score = 0;
        }
    }

    //=============================================Getting Data===================================================
    public string GetID() => playData.id;
    public int GetOldScore(int level) => playData.levelDatas[level - 1].score;
}