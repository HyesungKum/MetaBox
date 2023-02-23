using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RankingDB
{
    #region FreezeData
    [Serializable]
    public class FreezeData
    {
        public string id;

        public long[] levelOne = new long[2];
        public long[] levelTwo = new long[2];
        public long[] levelThree = new long[2];
        public long[] levelFour = new long[2];
    }
    #endregion

    #region HeyCookData
    [Serializable]
    public class HeyCookData
    {
        public string id;

        public long[] levelOne = new long[2];
        public long[] levelTwo = new long[2];
        public long[] levelThree = new long[2];
        public long[] levelFour = new long[2];
    }
    #endregion

    #region MelodiaData
    [Serializable]
    public class MelodiaData
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


    public class MelodiaSongArry
    {
        public long[] levelOne = new long[3];
        public long[] levelTwo = new long[3];
        public long[] levelThrrr = new long[3];
        public long[] levelFour = new long[3];
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
        public IMongoDatabase dataBase = null;
        public IMongoCollection<BsonDocument> FreezeCollection = null;
        public IMongoCollection<BsonDocument> HeyCookCollection = null;
        public IMongoCollection<BsonDocument> MelodiaCollection = null;
        public IMongoCollection<BsonDocument> SketchUpCollection = null;

        #region Each Game Data
        [Header("[Freeze Data]")]
        [SerializeField] FreezeData freezeData = null;

        [Header("[HeyCook Data]")]
        [SerializeField] HeyCookData heyCookData = null;

        [Header("[Melodia Data]")]
        [SerializeField] MelodiaData melodiaData = null;

        [Header("[SketchUp Data]")]
        [SerializeField] SketchUpData sketchUpData = null;
        #endregion

        Dictionary<string, long> freezeRankingDictionary;
        Dictionary<string, long> heyCookRankingDictionary;
        Dictionary<string, long> melodiaRankingDictionary;
        Dictionary<string, long> sketchUpRankingDictionary;

        Dictionary<string, long> levelOneDict;
        Dictionary<string, long> levelTwoDict;
        Dictionary<string, long> levelThreeDict;
        Dictionary<string, long> levelFourDict;

        public Dictionary<string, long> sortDict;

        //[Space]
        //[SerializeField] TopTenRankMgr topTenRank = null;

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

            #region Data Reset
            freezeData = new FreezeData();
            heyCookData = new HeyCookData();
            melodiaData = new MelodiaData();
            sketchUpData = new SketchUpData();
            #endregion

            #region User Ranking Data Dictionary Reset
            freezeRankingDictionary = new Dictionary<string, long>();
            heyCookRankingDictionary = new Dictionary<string, long>();
            melodiaRankingDictionary = new Dictionary<string, long>();
            sketchUpRankingDictionary = new Dictionary<string, long>();

            levelOneDict = new Dictionary<string, long>();
            levelTwoDict = new Dictionary<string, long>();
            levelThreeDict = new Dictionary<string, long>();
            levelFourDict = new Dictionary<string, long>();
            #endregion

            #region Save Data to DB
            //SaveFreezeData(freezeData, "�ȳ�1", 10);
            //SaveFreezeData(freezeData, "�ȳ�2", 20);
            //SaveFreezeData(freezeData, "�ȳ�3", 30);
            //SaveFreezeData(freezeData, "�ȳ�4", 40);
            //SaveFreezeData(freezeData, "�ȳ�5", 50);
            //SaveFreezeData(freezeData, "�ȳ�6", 60);
            //SaveFreezeData(freezeData, "�ȳ�7", 70);
            //SaveFreezeData(freezeData, "�ȳ�8", 80);
            //SaveFreezeData(freezeData, "�ȳ�9", 90);
            //SaveFreezeData(freezeData, "�ȳ�10", 100);
            //SaveFreezeData(freezeData, "�ȳ�11", 200);

            //SaveHeyCookData(heyCookData, "�ȳ�1", 10);
            //SaveHeyCookData(heyCookData, "�ȳ�2", 20);
            //SaveHeyCookData(heyCookData, "�ȳ�3", 30);
            //SaveHeyCookData(heyCookData, "�ȳ�4", 40);
            //SaveHeyCookData(heyCookData, "�ȳ�5", 50);
            //SaveHeyCookData(heyCookData, "�ȳ�6", 60);
            //SaveHeyCookData(heyCookData, "�ȳ�7", 70);
            //SaveHeyCookData(heyCookData, "�ȳ�8", 80);
            //SaveHeyCookData(heyCookData, "�ȳ�9", 90);
            //SaveHeyCookData(heyCookData, "�ȳ�10", 100);
            //SaveHeyCookData(heyCookData, "�ȳ�11", 200);

            //SaveMelodiaDataBase(melodiaData, "�ȳ�1", 10);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�2", 20);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�3", 30);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�4", 40);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�5", 50);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�6", 60);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�7", 70);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�8", 80);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�9", 90);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�10", 100);
            //SaveMelodiaDataBase(melodiaData, "�ȳ�11", 200);

            //SaveSketchUpDataBase(sketchUpData, "�ȳ�1", 10);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�2", 20);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�3", 30);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�4", 40);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�5", 50);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�6", 60);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�7", 70);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�8", 80);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�9", 90);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�10", 100);
            //SaveSketchUpDataBase(sketchUpData, "�ȳ�11", 200);
            #endregion

            #region DB Data All Delete
            //DeleteAllCollectionDataBase(FreezeCollection);
            //DeleteAllCollectionDataBase(HeyCookCollection);
            //DeleteAllCollectionDataBase(MelodiaCollection);
            //DeleteAllCollectionDataBase(SketchUpCollection);
            #endregion

            #region Find ID Check
            //CheckFreezeID("�ȳ�1");
            //CheckHeyCookID("�ȳ�1");
            //CheckMelodiaID("�ȳ�1");
            //CheckSketchUPID("�ȳ�1");
            #endregion

            #region Changed Point
            //ChangedPoint(SketchUpCollection, "�ȳ�2", "levelTwo", 250);
            //ChangedPoint(SketchUpCollection, "�ȳ�4", "levelThree", 400);
            //ChangedPoint(SketchUpCollection, "�ȳ�6", "levelFour", 600);
            #endregion

            #region Get Each Collection All Data
            //GetSketchUpUserDatas("levelOne", "�ȳ�5", levelOneDict, topTenRank.instEasyPos);
            //GetSketchUpUserDatas("levelTwo", "�ȳ�5", levelTwoDict, topTenRank.instNormalPos);
            //GetSketchUpUserDatas("levelThree", "�ȳ�5", levelThreeDict, topTenRank.instHardPos);
            //GetSketchUpUserDatas("levelFour", "�ȳ�5", levelFourDict, topTenRank.instExtremePos);
            #endregion
        }

        #region GetAndSetUserData
        public async void GetAllUserData(IMongoCollection<BsonDocument> collection, string levelNum, string findId, Dictionary<string, long> dict, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = collection.FindAsync(find);
            var scoreAwited = await allDataTask;

            BsonArray levelArry;
            string id;
            long point;

            foreach (var check in scoreAwited.ToList())
            {
                id = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(levelNum);
                point = (long)levelArry[0];
                dict.Add(id, point);
            }

            CheckSorting(findId, dict, pos);
            sortDict.Clear();
        }

        public async void GetFreezeUserData(string levelNum, string findId, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = FreezeCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            BsonArray levelArry;
            string id;
            long point;

            foreach (var check in scoreAwited.ToList())
            {
                //Debug.Log("Freeze) : " + check);
                id = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(levelNum);
                point = (long)levelArry[0];
                freezeRankingDictionary.Add(id, point);
            }

            CheckSorting(findId, freezeRankingDictionary, pos);
            sortDict.Clear();
        }

        public async void GetHeyCookUserData(string levelNum, string findId, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = HeyCookCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            BsonArray levelArry;
            string id;
            long point;

            foreach (var check in scoreAwited.ToList())
            {
                //Debug.Log("HeyCook) : " + check);
                id = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(levelNum);
                point = (long)levelArry[0];
                heyCookRankingDictionary.Add(id, point);
            }

            CheckSorting(findId, heyCookRankingDictionary, pos);
            sortDict.Clear();
        }

        public async void GetMelodiaUserData(string songLevelNum, string findId, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = MelodiaCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            BsonArray levelArry;
            string id;
            long point;

            foreach (var check in scoreAwited.ToList())
            {
                //Debug.Log("Melodia) : " + check);
                id = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(songLevelNum);
                point = (long)levelArry[0];
                melodiaRankingDictionary.Add(id, point);
            }

            CheckSorting(findId, melodiaRankingDictionary, pos);
            sortDict.Clear();
        }

        public async void GetSketchUpUserDatas(string levelNum, string findId, Dictionary<string, long> dict, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = SketchUpCollection.FindAsync(find);
            var scoreAwited = await allDataTask;

            BsonArray levelArry;
            string id;
            long point;

            foreach (var check in scoreAwited.ToList())
            {
                id = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(levelNum);
                point = (long)levelArry[0];
                dict.Add(id, point);
            }

            Debug.Log("Dict Count : " + dict.Count);
            string userId;
            long pointChanged;
            CheckSorting(findId, dict, pos);
            sortDict.Clear();
        }

        Dictionary<string, long> CheckSorting(string id, Dictionary<string, long> dict, RectTransform pos)
        {
            sortDict = new Dictionary<string, long>();
            sortDict = SortDictionary(dict);

            int rank = 1;

            foreach (KeyValuePair<string, long> item in sortDict)
            {
                if (item.Key.Equals(id))
                {

                    //topTenRank.ShowPlayerData(rank, item.Key, item.Value, pos);

                }
                //else
                //{
                //    topTenRank.PlayerNoTopTenRank("ž 10 ��ŷ�� �����ϴ� !");
                //}

                //topTenRank.InstUserData(rank, item.Key, item.Value, pos);
                rank += 1;

                if (rank == 11)
                    break;
            }

            return sortDict;
        }
        #endregion

        #region Dictipnary Sorting
        public Dictionary<string, long> Sorting(Dictionary<string, long> dic)
        {
            return dic.OrderBy(item => item.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, long> SortDictionary(Dictionary<string, long> dict)
        {
            var sortVar = from item in dict
                          orderby item.Value descending
                          select item;

            return sortVar.ToDictionary(x => x.Key, x => x.Value);
        }
        #endregion

        #region Find ID
        public BsonDocument FindID(IMongoCollection<BsonDocument> collection, string id)
        {
            BsonDocument findId = new BsonDocument { { "_id", id } };
            BsonDocument targetData = collection.Find(findId).FirstOrDefault();
            return targetData;
        }

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

        #region Save Data
        public async void SaveFreezeData(FreezeData newData, string id, long point)
        {
            newData.id = id;
            newData.levelOne[0] = point;
            newData.levelOne[1] = TimeSetting();

            newData.levelTwo[0] = point;
            newData.levelTwo[1] = TimeSetting();

            newData.levelThree[0] = point;
            newData.levelThree[1] = TimeSetting();

            newData.levelFour[0] = point;
            newData.levelFour[1] = TimeSetting();
            await FreezeCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[Freeze] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }

        public async void SaveHeyCookData(HeyCookData newData, string id, long point)
        {
            newData.id = id;
            newData.levelOne[0] = point;
            newData.levelOne[1] = TimeSetting();

            newData.levelTwo[0] = point;
            newData.levelTwo[1] = TimeSetting();

            newData.levelThree[0] = point;
            newData.levelThree[1] = TimeSetting();

            newData.levelFour[0] = point;
            newData.levelFour[1] = TimeSetting();

            await HeyCookCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[HeyCook] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }

        public async void SaveMelodiaDataBase(MelodiaData newData, string id, long point)
        {
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
            await MelodiaCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[Melodia] NewData �߰� �Ϸ� :" + newData.ToBsonDocument());
        }

        public async void SaveSketchUpDataBase(SketchUpData newData, string id, long point)
        {
            newData.id = id;
            newData.levelOne[0] = point;
            newData.levelOne[1] = TimeSetting();

            newData.levelTwo[0] = point;
            newData.levelTwo[1] = TimeSetting();

            newData.levelThree[0] = point;
            newData.levelThree[1] = TimeSetting();

            newData.levelFour[0] = point;
            newData.levelFour[1] = TimeSetting();
            await SketchUpCollection.InsertOneAsync(newData.ToBsonDocument());
            Debug.Log("[SketchUp] NewData �߰� �Ϸ� : " + newData.ToBsonDocument());
        }
        #endregion

        #region Changed Point
        public void ChangedPoint(IMongoCollection<BsonDocument> collection, string id, string levelNum, long point)
        {
            BsonDocument filter = new BsonDocument { { "_id", id } };
            BsonDocument targetData = collection.Find(filter).FirstOrDefault();

            long[] level = new long[2];
            level[0] = point;
            level[1] = TimeSetting();

            UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levelNum, level);
            collection.UpdateOne(targetData, updatePoint);
        }

        public void ChangedSketchUpPoint(SketchUpData newData, string levleNum, string id)
        {
            BsonDocument filter = new BsonDocument { { "_id", id } };
            BsonDocument targetData = SketchUpCollection.Find(filter).FirstOrDefault();
            //Debug.Log("targetData : " + targetData);

            long point = 2000;

            long[] level = new long[2];
            level[0] = point;
            level[1] = TimeSetting();

            UpdateDefinition<BsonDocument> updatePoint = Builders<BsonDocument>.Update.Set(levleNum, level);
            SketchUpCollection.UpdateOne(targetData, updatePoint);
        }
        #endregion

        #region Delete All Data
        public async void DeleteAllCollectionDataBase(IMongoCollection<BsonDocument> mongoCollection)
        {
            BsonDocument find = new BsonDocument();
            await mongoCollection.DeleteManyAsync(find.ToBsonDocument());
        }
        #endregion

        #region Play Game Time Setting : Year/Month/Day/Hour/Minute
        public long TimeSetting()
        {
            string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // ���� �ð�
            long time = long.Parse(nowDate);
            return time;
        }
        #endregion
    }
}