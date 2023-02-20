using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using UnityEngine;
using Kum;
using System;
using System.Collections;
using UnityEngine.Timeline;

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
    public const string EASY     = "gameLevel_1_Score";
    public const string NORMAL   = "gameLevel_2_Score";
    public const string HARD     = "gameLevel_3_Score";
    public const string VERYHARD = "gameLevel_4_Score";
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
    readonly MongoClient clientData = new("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");

    private IMongoDatabase dataBase;
    private IMongoCollection<BsonDocument> collection;

    //===================================CurData=============================
    public PlayData playData;

    //===============================InternetConnection======================
    public bool OnlineMode;

    protected override void Awake()
    {
        base.Awake();

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
            collection = dataBase.GetCollection<BsonDocument>("HeyCookRank");
        }
        
        //delegate chain
        EventReceiver.saveCallBack += SaveData;
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

        playData.levelDatas[level - 1].score = score;
    }

    private void SaveMongoDB(int gameLevel, int score)
    {
        //apply filter data setting
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("playerID", playData.id);

        //apply update data setting
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set($"gameLevel_{gameLevel}_Score", score);

        //collection update
        collection.UpdateOne(filter, update);
    }

    /// <summary>
    /// New User Data Save in DB
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="gameLevel"></param>
    /// <param name="score"></param>
    /// <param name="date"></param>
    private async void SaveNewMongoDB()
    {
        if (string.IsNullOrWhiteSpace(playData.id)) return;

        BsonDocument playerData = new()
        {
            { "playerID", playData.id },
            { $"gameLevel_1_Score", 0 },
            { $"gameLevel_2_Score", 0 },
            { $"gameLevel_3_Score", 0 },
            { $"gameLevel_4_Score", 0 },
        };

        await collection.InsertOneAsync(playerData);
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
        playData.levelDatas[Level.EASY - 1].score     = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        playData.levelDatas[Level.NORMAL - 1].score   = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        playData.levelDatas[Level.HARD - 1].score     = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        playData.levelDatas[Level.VERYHARD - 1].score = PlayerPrefs.GetInt(ScoreType.VERYHARD, 0);
    }

    /// <summary>
    /// Get User ScoreData in MongoDB
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    private void LoadMongoDB(string playerID)
    {
        //apply filter to find
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("playerID", playerID);

        //find data in collection using filter
        BsonDocument document = collection.Find(filter).FirstOrDefault();

        //online data init
        int onlineEasy = 0;
        int onlineNoraml = 0;
        int onlineHard = 0;
        int onlineVeryHard = 0;

        //get local data
        int localEasy = PlayerPrefs.GetInt(ScoreType.EASY, 0);
        int localNormal = PlayerPrefs.GetInt(ScoreType.NORMAL, 0);
        int localHard = PlayerPrefs.GetInt(ScoreType.HARD, 0);
        int localVeryHard = PlayerPrefs.GetInt(ScoreType.VERYHARD, 0);

        //return pickup Data if was not null
        if (document == null)
        {
            //create new account
            SaveNewMongoDB();
        }
        else
        {
            //get online data
            onlineEasy = (int)document.GetValue(ScoreType.EASY);
            onlineNoraml = (int)document.GetValue(ScoreType.NORMAL);
            onlineHard = (int)document.GetValue(ScoreType.HARD);
            onlineVeryHard = (int)document.GetValue(ScoreType.VERYHARD);
        }

        //get highter score
        int temp1Score = playData.levelDatas[Level.EASY - 1].score = localEasy > onlineEasy ? localEasy : onlineEasy;
        int temp2Score = playData.levelDatas[Level.NORMAL - 1].score = localNormal > onlineNoraml ? localNormal : onlineNoraml;
        int temp3Score = playData.levelDatas[Level.HARD - 1].score = localHard > onlineHard ? localHard : onlineHard;
        int temp4Score = playData.levelDatas[Level.VERYHARD - 1].score = localVeryHard > onlineVeryHard ? localVeryHard : onlineVeryHard;

        //apply save data
        SaveMongoDB(Level.EASY,    temp1Score);
        SaveMongoDB(Level.NORMAL,  temp2Score);
        SaveMongoDB(Level.HARD,    temp3Score);
        SaveMongoDB(Level.VERYHARD,temp4Score);
    }

    //=============================================Getting Data===================================================
    public string GetID() => string.IsNullOrEmpty(playData.id) ? "Guest" : playData.id;
    public int GetOldScore(int level) => playData.levelDatas[level - 1].score;
}