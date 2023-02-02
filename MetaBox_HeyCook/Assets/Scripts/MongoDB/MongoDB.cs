using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class RankingDataBase : MonoBehaviour
{
    readonly MongoClient clientData = new("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majori");

    private IMongoDatabase dataBase;
    private IMongoCollection<BsonDocument> collection;

    void Awake()
    {
        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");
        // MongoDB collection name
        collection = dataBase.GetCollection<BsonDocument>("HeyCookRank");
    }

    // New User Data Save in DB
    public async void SaveScoreToDataBase(int id, int gameGroup, int gameLevel, int point, long date)
    {
        if (gameGroup > 4 && gameLevel > 4) return;
        var document = new BsonDocument { { "id", id }, { "gameGroup", gameGroup }, { "gameLevel", gameLevel }, { "point", point }, { "long", date } };
        await collection.InsertOneAsync(document);
        Debug.Log("## DB에 데이터 추가 완료 : " + document.ToString());
    }
}