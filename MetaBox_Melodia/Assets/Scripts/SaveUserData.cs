using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using UnityEngine;

public struct Song
{
    public const int LittleStar = 1;
    public const int Rabbit = 2;
    public const int Butterfly = 3;
    public const int Stone = 4;
}

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
    public const string EASY1 = "songOneLevelOne";
    public const string NORMAL1 = "songOneLevelTwo";
    public const string HARD1 = "songOneLevelThree";
    public const string EXTREME1 = "songOneLevelFour";

    public const string EASY2 = "songTwoLevelOne";
    public const string NORMAL2 = "songTwoLevelTwo";
    public const string HARD2 = "songTwoLevelThree";
    public const string EXTREME2 = "songTwoLevelFour";

    public const string EASY3 = "songThreeLevelOne";
    public const string NORMAL3 = "songThreeLevelTwo";
    public const string HARD3 = "songThreeLevelThree";
    public const string EXTREME3 = "songThreeLevelFour";

    public const string EASY4 = "songFourLevelOne";
    public const string NORMAL4 = "songFourLevelTwo";
    public const string HARD4 = "songFourLevelThree";
    public const string EXTREME4 = "songFourLevelFour";
}

public class PlayData
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

public class SaveUserData : MonoBehaviour
{
    //==================================Database============================
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    public IMongoDatabase dataBase = null;
    public IMongoCollection<BsonDocument> Collection = null;

    //===================================CurData=============================
    public PlayData curPlayData = new();

    [SerializeField] private string localSavePath = "/MetaBox/SaveData/HCSaveData.json";
#if UNITY_EDITOR
    [SerializeField] private string defaultPath = "/MetaBox/SaveData/";
#else
    private string defaultPath = "/storage/emulated/0/MetaBox/SaveData/TownSaveData.json";
#endif

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
            Collection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
        }

        //delegate
        GameManager.Inst.gameClearRecord = SaveData;
    }

    private void Start()
    {
        curPlayData.id = StartUI.curUserData.ID;
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
        long onlineEasy1 = 0;
        long onlineNoraml1 = 0;
        long onlineHard1 = 0;
        long onlineExtreme1 = 0;

        long onlineEasy2 = 0;
        long onlineNoraml2 = 0;
        long onlineHard2 = 0;
        long onlineExtreme2 = 0;

        long onlineEasy3 = 0;
        long onlineNoraml3 = 0;
        long onlineHard3 = 0;
        long onlineExtreme3 = 0;

        long onlineEasy4 = 0;
        long onlineNoraml4 = 0;
        long onlineHard4 = 0;
        long onlineExtreme4 = 0;

        //get local data
        int localEasy1 = PlayerPrefs.GetInt(ScoreType.EASY1, 0);
        int localNormal1 = PlayerPrefs.GetInt(ScoreType.NORMAL1, 0);
        int localHard1 = PlayerPrefs.GetInt(ScoreType.HARD1, 0);
        int localExtreme1 = PlayerPrefs.GetInt(ScoreType.EXTREME1, 0);

        int localEasy2 = PlayerPrefs.GetInt(ScoreType.EASY2, 0);
        int localNormal2 = PlayerPrefs.GetInt(ScoreType.NORMAL2, 0);
        int localHard2 = PlayerPrefs.GetInt(ScoreType.HARD2, 0);
        int localExtreme2 = PlayerPrefs.GetInt(ScoreType.EXTREME2, 0);

        int localEasy3 = PlayerPrefs.GetInt(ScoreType.EASY3, 0);
        int localNormal3 = PlayerPrefs.GetInt(ScoreType.NORMAL3, 0);
        int localHard3 = PlayerPrefs.GetInt(ScoreType.HARD3, 0);
        int localExtreme3 = PlayerPrefs.GetInt(ScoreType.EXTREME3, 0);

        int localEasy4 = PlayerPrefs.GetInt(ScoreType.EASY4, 0);
        int localNormal4 = PlayerPrefs.GetInt(ScoreType.NORMAL4, 0);
        int localHard4 = PlayerPrefs.GetInt(ScoreType.HARD4, 0);
        int localExtreme4 = PlayerPrefs.GetInt(ScoreType.EXTREME4, 0);

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

            levelArry = (BsonArray)document.GetValue(ScoreType.EASY1);
            onlineEasy1 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.NORMAL1);
            onlineNoraml1 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.HARD1);
            onlineHard1 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.EXTREME1);
            onlineExtreme1 = (long)levelArry[0];


            levelArry = (BsonArray)document.GetValue(ScoreType.EASY2);
            onlineEasy2 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.NORMAL2);
            onlineNoraml2 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.HARD2);
            onlineHard2 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.EXTREME2);
            onlineExtreme2 = (long)levelArry[0];


            levelArry = (BsonArray)document.GetValue(ScoreType.EASY3);
            onlineEasy3 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.NORMAL3);
            onlineNoraml3 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.HARD3);
            onlineHard3 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.EXTREME3);
            onlineExtreme3 = (long)levelArry[0];


            levelArry = (BsonArray)document.GetValue(ScoreType.EASY4);
            onlineEasy4 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.NORMAL4);
            onlineNoraml4 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.HARD4);
            onlineHard4 = (long)levelArry[0];

            levelArry = (BsonArray)document.GetValue(ScoreType.EXTREME4);
            onlineExtreme4 = (long)levelArry[0];

            //get highter playtime
            curPlayData.songOneLevelOne[0] = localEasy1 > onlineEasy1 ? localEasy1 : onlineEasy1;
            curPlayData.songOneLevelTwo[0] = localNormal1 > onlineNoraml1 ? localNormal1 : onlineNoraml1;
            curPlayData.songOneLevelThree[0] = localHard1 > onlineHard1 ? localHard1 : onlineHard1;
            curPlayData.songOneLevelFour[0] = localExtreme1 > onlineExtreme1 ? localExtreme1 : onlineExtreme1;

            curPlayData.songTwoLevelOne[0] = localEasy2 > onlineEasy2 ? localEasy2 : onlineEasy2;
            curPlayData.songTwoLevelTwo[0] = localNormal2 > onlineNoraml2 ? localNormal2 : onlineNoraml2;
            curPlayData.songTwoLevelThree[0] = localHard2 > onlineHard2 ? localHard2 : onlineHard2;
            curPlayData.songTwoLevelFour[0] = localExtreme2 > onlineExtreme2 ? localExtreme2 : onlineExtreme2;

            curPlayData.songThreeLevelOne[0] = localEasy3 > onlineEasy3 ? localEasy3 : onlineEasy3;
            curPlayData.songThreeLevelTwo[0] = localNormal3 > onlineNoraml3 ? localNormal3 : onlineNoraml3;
            curPlayData.songThreeLevelThree[0] = localHard3 > onlineHard3 ? localHard3 : onlineHard3;
            curPlayData.songThreeLevelFour[0] = localExtreme3 > onlineExtreme3 ? localExtreme3 : onlineExtreme3;

            curPlayData.songFourLevelOne[0] = localEasy4 > onlineEasy4 ? localEasy4 : onlineEasy4;
            curPlayData.songFourLevelTwo[0] = localNormal4 > onlineNoraml4 ? localNormal4 : onlineNoraml4;
            curPlayData.songFourLevelThree[0] = localHard4 > onlineHard4 ? localHard4 : onlineHard4;
            curPlayData.songFourLevelFour[0] = localExtreme4 > onlineExtreme4 ? localExtreme4 : onlineExtreme4;
        }
    }

    /// <summary>
    /// Get User ScoreData in playerfrefs
    /// </summary>
    private void LoadLocalDB()
    {
        curPlayData.songOneLevelOne[0] = PlayerPrefs.GetInt(ScoreType.EASY1, 0);
        curPlayData.songOneLevelTwo[0] = PlayerPrefs.GetInt(ScoreType.NORMAL1, 0);
        curPlayData.songOneLevelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD1, 0);
        curPlayData.songOneLevelFour[0] = PlayerPrefs.GetInt(ScoreType.EXTREME1, 0);

        curPlayData.songTwoLevelOne[0] = PlayerPrefs.GetInt(ScoreType.EASY2, 0);
        curPlayData.songTwoLevelTwo[0] = PlayerPrefs.GetInt(ScoreType.NORMAL2, 0);
        curPlayData.songTwoLevelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD2, 0);
        curPlayData.songTwoLevelFour[0] = PlayerPrefs.GetInt(ScoreType.EXTREME2, 0);

        curPlayData.songThreeLevelOne[0] = PlayerPrefs.GetInt(ScoreType.EASY3, 0);
        curPlayData.songThreeLevelTwo[0] = PlayerPrefs.GetInt(ScoreType.NORMAL3, 0);
        curPlayData.songThreeLevelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD3, 0);
        curPlayData.songThreeLevelFour[0] = PlayerPrefs.GetInt(ScoreType.EXTREME3, 0);

        curPlayData.songFourLevelOne[0] = PlayerPrefs.GetInt(ScoreType.EASY4, 0);
        curPlayData.songFourLevelTwo[0] = PlayerPrefs.GetInt(ScoreType.NORMAL4, 0);
        curPlayData.songFourLevelThree[0] = PlayerPrefs.GetInt(ScoreType.HARD4, 0);
        curPlayData.songFourLevelFour[0] = PlayerPrefs.GetInt(ScoreType.EXTREME4, 0);
    }

    //===========================================Saving Data====================================================
    public void SaveData(int playtime)
    {
        if (curPlayData.id == null) return;
        if (GetOldScore((int)StartUI.MySceneMode, StartUI.Level) < playtime)
        {
            if (OnlineMode) SaveMongoDB(StartUI.Level, playtime);
            else SaveLocalDB(StartUI.Level, playtime);
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

        switch (StartUI.MySceneMode)
        {
            case SceneMode.littlestar:
                switch (gameLevel)
                {
                    case 1: levelString = ScoreType.EASY1; break;
                    case 2: levelString = ScoreType.NORMAL1; break;
                    case 3: levelString = ScoreType.HARD1; break;
                    case 4: levelString = ScoreType.EXTREME1; break;
                }
                break;
            case SceneMode.rabbit:
                switch (gameLevel)
                {
                    case 1: levelString = ScoreType.EASY2; break;
                    case 2: levelString = ScoreType.NORMAL2; break;
                    case 3: levelString = ScoreType.HARD2; break;
                    case 4: levelString = ScoreType.EXTREME2; break;
                }
                break;
            case SceneMode.butterfly:
                switch (gameLevel)
                {
                    case 1: levelString = ScoreType.EASY3; break;
                    case 2: levelString = ScoreType.NORMAL3; break;
                    case 3: levelString = ScoreType.HARD3; break;
                    case 4: levelString = ScoreType.EXTREME3; break;
                }
                break;
            case SceneMode.stone:
                switch (gameLevel)
                {
                    case 1: levelString = ScoreType.EASY4; break;
                    case 2: levelString = ScoreType.NORMAL4; break;
                    case 3: levelString = ScoreType.HARD4; break;
                    case 4: levelString = ScoreType.EXTREME4; break;
                }
                break;
        }
        

        long[] levelData = new long[2];
        levelData[0] = playtime;
        levelData[1] = TimeSetting();

        UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levelString, levelData);
        Collection.UpdateOne(targetData, updatePoint);
    }
    private void SaveLocalDB(int gameLevel, long playtime)
    {
        switch (StartUI.MySceneMode)
        {
            case SceneMode.littlestar:
                switch (gameLevel)
                {
                    case 1: PlayerPrefs.SetInt(ScoreType.EASY1, (int)playtime); break;
                    case 2: PlayerPrefs.SetInt(ScoreType.NORMAL1, (int)playtime); break;
                    case 3: PlayerPrefs.SetInt(ScoreType.HARD1, (int)playtime); break;
                    case 4: PlayerPrefs.SetInt(ScoreType.EXTREME1, (int)playtime); break;
                }
                break;
            case SceneMode.rabbit:
                switch (gameLevel)
                {
                    case 1: PlayerPrefs.SetInt(ScoreType.EASY2, (int)playtime); break;
                    case 2: PlayerPrefs.SetInt(ScoreType.NORMAL2, (int)playtime); break;
                    case 3: PlayerPrefs.SetInt(ScoreType.HARD2, (int)playtime); break;
                    case 4: PlayerPrefs.SetInt(ScoreType.EXTREME2, (int)playtime); break;
                }
                break;
            case SceneMode.butterfly:
                switch (gameLevel)
                {
                    case 1: PlayerPrefs.SetInt(ScoreType.EASY3, (int)playtime); break;
                    case 2: PlayerPrefs.SetInt(ScoreType.NORMAL3, (int)playtime); break;
                    case 3: PlayerPrefs.SetInt(ScoreType.HARD3, (int)playtime); break;
                    case 4: PlayerPrefs.SetInt(ScoreType.EXTREME3, (int)playtime); break;
                }
                break;
            case SceneMode.stone:
                switch (gameLevel)
                {
                    case 1: PlayerPrefs.SetInt(ScoreType.EASY4, (int)playtime); break;
                    case 2: PlayerPrefs.SetInt(ScoreType.NORMAL4, (int)playtime); break;
                    case 3: PlayerPrefs.SetInt(ScoreType.HARD4, (int)playtime); break;
                    case 4: PlayerPrefs.SetInt(ScoreType.EXTREME4, (int)playtime); break;
                }
                break;
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
        PlayData newData = new();

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

        await Collection.InsertOneAsync(newData.ToBsonDocument());
    }
    #endregion


    //=============================================Getting Data===================================================
    public string GetID() => curPlayData.id;
    public int GetOldScore(int song,  int level)
    {
        switch (song)
        {
            case 0:
                switch (level)
                {
                    case 1: return (int)curPlayData.songOneLevelOne[0];
                    case 2: return (int)curPlayData.songOneLevelTwo[0];
                    case 3: return (int)curPlayData.songOneLevelThree[0];
                    case 4: return (int)curPlayData.songOneLevelFour[0];
                    default: return 0;
                }
            case 1:
                switch (level)
                {
                    case 1: return (int)curPlayData.songTwoLevelOne[0];
                    case 2: return (int)curPlayData.songTwoLevelTwo[0];
                    case 3: return (int)curPlayData.songTwoLevelThree[0];
                    case 4: return (int)curPlayData.songTwoLevelFour[0];
                    default: return 0;
                }
            case 2:
                switch (level)
                {
                    case 1: return (int)curPlayData.songThreeLevelOne[0];
                    case 2: return (int)curPlayData.songThreeLevelTwo[0];
                    case 3: return (int)curPlayData.songThreeLevelThree[0];
                    case 4: return (int)curPlayData.songThreeLevelFour[0];
                    default: return 0;
                }
            case 3:
                switch (level)
                {
                    case 1: return (int)curPlayData.songFourLevelOne[0];
                    case 2: return (int)curPlayData.songFourLevelTwo[0];
                    case 3: return (int)curPlayData.songFourLevelThree[0];
                    case 4: return (int)curPlayData.songFourLevelFour[0];
                    default: return 0;
                }
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
