using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class UserData : MonoBehaviour
{
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.4ur23zq.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase dataBase = null;
    IMongoCollection<BsonDocument> collection = null;

    bool user = false;
    string userID = null;
    int gameGroup;
    int level;

    private void Awake()
    {
        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");
        // MongoDB collection name
        collection = dataBase.GetCollection<BsonDocument>("RankingCollection");

        GameManager.Instance.gameClearRecord = Record;
    }
    
    void Start()
    {
        InGame();
    }

    void InGame()
    {
        string fileName = "UserData";
        string path = $"{Application.dataPath}/{fileName}.txt";
        if (File.Exists(path))
        {
            user = true;
            userID = File.ReadAllText(path);
            gameGroup = GameManager.Instance.FreezeData.gameGroup;
            level = GameManager.Instance.FreezeData.level;
        }
    }

    void Record()
    {
        if (user == false || userID == null) return;
        DataProcess(GameManager.Instance.FreezeData.playTime - GameManager.Instance.PlayTime);
    }

    void DataProcess(int playtime)
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm");
        long time = long.Parse(nowDate);

        BsonDocument filter = new BsonDocument { { "id", userID }, { "gameGroup", gameGroup }, { "gameLevel", level } };
        BsonDocument targetData = collection.Find(filter).FirstOrDefault();


        if (targetData != null) //기록유
        {
            int prevTime = (int)targetData.GetValue("playtime");
            if (prevTime > playtime) //기록갱신
            {
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("playtime", playtime);
                //collection.FindOneAndUpdate(filter, update);
                collection.UpdateOne(filter,update);
            }
            else
            {
                //Debug.Log("이전 기록이 더 좋습니다");
            }
        }
        else //기록무
        {
            //Debug.Log("새로운 기록을 추가합니다");
            SaveScoreToDataBase(playtime, time);
        }
    }

    // New User Data Save in DB
    public async void SaveScoreToDataBase(int playtime, long date)
    {
        var document = new BsonDocument { { "id", userID }, { "gameGroup", gameGroup }, { "gameLevel", level }, { "playtime", playtime }, { "date", date } };
        await collection.InsertOneAsync(document);
    }
}
