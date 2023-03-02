using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBPointChange : MonoBehaviour
{
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    public IMongoDatabase dataBase = null;

    public IMongoCollection<BsonDocument> SketchUpCollection = null;

    void Awake()
    {
        dataBase = clientData.GetDatabase("RankingDB");
        SketchUpCollection = dataBase.GetCollection<BsonDocument>("SketchUpRanking");
    }

    public void ChangedSketchUpPoint(string levleNum, string id, long point)
    {
        BsonDocument filter = new BsonDocument { { "_id", id } };
        BsonDocument targetData = SketchUpCollection.Find(filter).FirstOrDefault();
        //Debug.Log("targetData : " + targetData);

        long[] level = new long[2];
        level[0] = point;
        level[1] = TimeSetting();

        UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levleNum, level);
        SketchUpCollection.UpdateOne(targetData, updatePoint);
    }

    #region Play Game Time Setting : Year/Month/Day/Hour/Minute
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // 현재 시간
        long time = long.Parse(nowDate);
        return time;
    }
    #endregion
}
