using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using TMPro;

[Serializable]
public struct UserData
{
    public string id;
    public int charIndex;
    public bool troughTown;
}

#region DreamSketch Data
[Serializable]
public class DreamSketchData
{
    public string id;

    public long[] levelOne = new long[2];
    public long[] levelTwo = new long[2];
    public long[] levelThree = new long[2];
    public long[] levelFour = new long[2];
}
#endregion

public class LoadIDMgr : MonoBehaviour
{
    #region Singleton
    private static LoadIDMgr instance;
    public static LoadIDMgr Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadIDMgr>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(LoadIDMgr), typeof(LoadIDMgr)).GetComponent<LoadIDMgr>();
                }
            }
            return instance;
        }
    }
    #endregion

    // === MongoDB ===
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    public IMongoDatabase dataBase = null;
    public IMongoCollection<BsonDocument> DreamSketchCollection = null;

    //=== app transition ===
    [Header("[Application Setting]")]
    [SerializeField] public string mainPackName = "com.MetaBox.DreamCatcher";
#if UNITY_EDITOR
    [SerializeField] private string localSavePath = "/MetaBox/SaveData/SaveData.json";
#else
    private string localSavePath = "/storage/emulated/0/MetaBox/SaveData/SaveData.json";
#endif

    public string id { get; set ; }
    public UserData curUserData;
    public DreamSketchData dreamSketchData;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance.gameObject);

        dataBase = clientData.GetDatabase("RankingDB");
        DreamSketchCollection = dataBase.GetCollection<BsonDocument>("DreamSketchRanking");

        if (File.Exists(localSavePath))
        {
            curUserData = ReadSaveData(localSavePath);
            id = $"아이디 : {curUserData.id}";
        }
        else//존재하지 않을때 
        {
            curUserData.id = "전설의연습생";
            id = $"아이디 : {curUserData.id}";
            if (FindID(curUserData.id) == null)
            {
                SaveSketchUpDataBase(curUserData.id, 0);
            }
        }
    }

    #region Find ID
    public BsonDocument FindID(string id)
    {
        BsonDocument findId = new BsonDocument { { "_id", id } };
        BsonDocument targetData = DreamSketchCollection.Find(findId).FirstOrDefault();
        return targetData;
    }
    #endregion

    private UserData ReadSaveData(string path)
    {
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);
        return readData;
    }

    #region SaveSketchUpDataBase
    public async void SaveSketchUpDataBase(string id, long point)
    {
        dreamSketchData.id = id;
        dreamSketchData.levelOne[0] = point;
        dreamSketchData.levelOne[1] = TimeSetting();

        dreamSketchData.levelTwo[0] = point;
        dreamSketchData.levelTwo[1] = TimeSetting();

        dreamSketchData.levelThree[0] = point;
        dreamSketchData.levelThree[1] = TimeSetting();

        dreamSketchData.levelFour[0] = point;
        dreamSketchData.levelFour[1] = TimeSetting();
        await DreamSketchCollection.InsertOneAsync(dreamSketchData.ToBsonDocument());
        Debug.Log("[DreamSketch] NewData 추가 완료 : " + dreamSketchData.ToBsonDocument());
    }
    #endregion

    #region Play Game Time Setting : Year/Month/Day/Hour/Minute
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // 현재 시간
        long time = long.Parse(nowDate);
        return time;
    }
    #endregion
}