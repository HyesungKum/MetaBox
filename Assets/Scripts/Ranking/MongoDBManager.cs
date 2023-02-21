using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RankingDB
{
    #region FreezeData
    [Serializable]
    public class FreezeData
    {
        public string id;

        public int level;
        public int point;
        public long date;
    }
    #endregion

    #region HeyCookData
    [Serializable]
    public class HeyCookData
    {
        public string id;
        public int level;
        public int point;
        public long date;
    }
    #endregion

    #region MelodiaData
    [Serializable]
    public class MelodiaData
    {
        public string id;
        public string song;
        public int[] songLevelArry;
    }
    #endregion

    #region SketchUpData
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


    public class MongoDBManager : MonoBehaviour
    {
        MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
        IMongoDatabase dataBase = null;
        IMongoCollection<BsonDocument> FreezeCollection = null;
        IMongoCollection<BsonDocument> HeyCookCollection = null;
        IMongoCollection<BsonDocument> MelodiaCollection = null;
        IMongoCollection<BsonDocument> SketchUpCollection = null;

        [Header("[Freeze Data]")]
        [SerializeField] FreezeData freezeData = null;

        [Header("[HeyCook Data]")]
        [SerializeField] HeyCookData heyCookData = null;

        [Header("[Melodia Data]")]
        [SerializeField] MelodiaData melodiaData = null;

        [Header("[SketchUp Data]")]
        [SerializeField] SketchUpData sketchUpData = null;

        // === UserData List
        List<BsonDocument> freezeRankingList;
        List<BsonDocument> heyCookRankingList;
        List<BsonDocument> melodiaRankingList;
        List<BsonDocument> sketchUpRankingList;

        void Awake()
        {
            #region MongoDB database name
            dataBase = clientData.GetDatabase("RankingDB");
            #endregion

            #region MongoDB collection name
            FreezeCollection = dataBase.GetCollection<BsonDocument>("FreezeRanking");
            HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
            MelodiaCollection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
            SketchUpCollection = dataBase.GetCollection<BsonDocument>("SketchUpRanking");
            #endregion

            #region Data reset
            freezeData = new FreezeData();
            heyCookData = new HeyCookData();
            melodiaData = new MelodiaData();
            sketchUpData = new SketchUpData();
            #endregion

            #region User Ranking Data List Reset
            freezeRankingList = new List<BsonDocument>();
            heyCookRankingList = new List<BsonDocument>();
            melodiaRankingList = new List<BsonDocument>();
            sketchUpRankingList = new List<BsonDocument>();
            #endregion

            // === Check Save DB Data ===
            //SaveSketchUpDataBase();
            //SaveSketchUpDataBase(sketchUpData,"�ȳ�", 10);
            //SaveSketchUpDataBase(sketchUpData,"�ȳ�1", 20);
            //SaveSketchUpDataBase(sketchUpData,"�ȳ�2", 30);
            //SaveSketchUpDataBase(sketchUpData,"�ȳ�3", 40);

            // === DB Data Delect ===
            // DeleteAllSketchUpDataBase();

            // === Find ID ===
            //CheckSketchUPID("�ȳ�");
            //SketchUPGetLevel(sketchUpData, sketchUpData.levelOne);

            // === �ȿ� �迭 �� Ȯ�� ===

            // === Check DB Data ===
            //GetFreezeUserData();
            //GetHeyCookUserData();
            //GetMelodiaUserData();
            //GetSketchUpUserData();

        }

        #region
        public async Task<List<BsonDocument>> GetFreezeUserData()
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = FreezeCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            long collectionLenght = FreezeCollection.Count(find);
            Debug.Log("Freeze) collectionLenght : " + collectionLenght);

            int point;
            foreach (var check in scoreAwited.ToList())
            {
                Debug.Log("Freeze) : " + check);
                point = (int)check.GetValue("point");
            }
            return freezeRankingList;
        }

        public async Task<List<BsonDocument>> GetHeyCookUserData()
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = HeyCookCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            long collectionLenght = HeyCookCollection.Count(find);
            Debug.Log("HeyCook) collectionLenght : " + collectionLenght);

            foreach (var check in scoreAwited.ToList())
            {
                Debug.Log("HeyCook) : " + check);
            }
            return heyCookRankingList;
        }

        public async Task<List<BsonDocument>> GetMelodiaUserData()
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = MelodiaCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            long collectionLenght = MelodiaCollection.Count(find);
            Debug.Log("Melodia) collectionLenght : " + collectionLenght);

            foreach (var check in scoreAwited.ToList())
            {
                Debug.Log("Melodia) : " + check);
            }

            return melodiaRankingList;
        }

        public async Task<List<BsonDocument>> GetSketchUpUserData()
        {
            BsonDocument find = new BsonDocument();
            //FilterDefinition
            var allDataTask = SketchUpCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            long collectionLenght = SketchUpCollection.Count(find);
            Debug.Log("SketchUp) collectionLenght : " + collectionLenght);

            object point = null;


            foreach (var check in scoreAwited.ToList())
            {
                Debug.Log("SketchUp) : " + check);

                point = (object)check.GetValue("levelOne");
                Debug.Log("point : " + point.ToString());
            }

            return sketchUpRankingList;
        }

        #endregion

        #region Find ID
        public BsonDocument CheckFreezeID(string findId)
        {
            BsonDocument filter = new BsonDocument { { "_id", findId } };
            BsonDocument targetData = FreezeCollection.Find(filter).FirstOrDefault();

            Debug.Log("Find Freeze ID : " + targetData);
            return targetData;
        }

        public BsonDocument CheckHeyCookID(string findId)
        {
            BsonDocument filter = new BsonDocument { { "_id", findId } };
            BsonDocument targetData = HeyCookCollection.Find(filter).FirstOrDefault();

            Debug.Log("Find HeyCook ID : " + targetData);
            return targetData;
        }

        public BsonDocument CheckMelodiaID(string findId)
        {
            BsonDocument filter = new BsonDocument { { "_id", findId } };
            BsonDocument targetData = MelodiaCollection.Find(filter).FirstOrDefault();

            Debug.Log("Find Melodia ID : " + targetData);
            return targetData;
        }

        public BsonDocument CheckSketchUPID(string findId)
        {
            BsonDocument filter = new BsonDocument { { "_id", findId } };
            BsonDocument targetData = SketchUpCollection.Find(filter).FirstOrDefault();

            Debug.Log("Find SketchUP : " + targetData);
            return targetData;
        }
        #endregion

        // === �켱 ���� ť�� �޾ƿ� DB ������ �ֱ�

        #region Save Data
        public async void SaveFreezeData(FreezeData newData)
        {
            newData.date = TimeSetting();
            await FreezeCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[Freeze] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }

        public async void SaveHeyCookData(HeyCookData newData)
        {
            newData.date = TimeSetting();
            await HeyCookCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[HeyCook] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }

        public async void SaveMelodiaDataBase(MelodiaData newData)
        {
            await MelodiaCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[Melodia] NewData �߰� �Ϸ� :" + newData.ToBsonDocument());
        }

        public async void SaveSketchUpDataBase(SketchUpData newData, string id, long point)
        {
            await SketchUpCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[SketchUp] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }
        #endregion

        #region

        #endregion

        #region Delete All Data
        public async void DeleteAllSketchUpDataBase()
        {
            BsonDocument find = new BsonDocument();
            await SketchUpCollection.DeleteManyAsync(find.ToBsonDocument());
        }
        #endregion

        // Play Game Time Setting : Year/Month/Day/Hour/Minute
        public long TimeSetting()
        {
            string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // ���� �ð�
            long time = long.Parse(nowDate);
            return time;
        }
    }
}