using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Analytics;

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

    public class DataSorting
    {
        public int rank;
        public string id;
        public int point;
        public long playtime;
    }

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

        #region Freeze Dictionary
        Dictionary<string, long> freezeLevelOneDict;
        Dictionary<string, long> freezeLevelTwoDict;
        Dictionary<string, long> freezeLevelThreeDict;
        Dictionary<string, long> freezeLevelFourDict;
        #endregion

        #region HeyCook Dictionary
        Dictionary<string, long> heyCookLevelOneDict;
        Dictionary<string, long> heyCookLevelTwoDict;
        Dictionary<string, long> heyCookLevelThreeDict;
        Dictionary<string, long> heyCookLevelFourDict;
        #endregion

        #region MelodiaDictionary
        Dictionary<string, long> melodiaSongOneLevelOneDict;
        Dictionary<string, long> melodiaSongOneLevelTwoDict;
        Dictionary<string, long> melodiaSongOneLevelThreeDict;
        Dictionary<string, long> melodiaSongTwoLevelFourDict;

        Dictionary<string, long> melodiaSongTwoLevelOneDict;
        Dictionary<string, long> melodiaSongTwoLevelTwoDict;
        Dictionary<string, long> melodiaSongTwoLevelThreeDict;
        Dictionary<string, long> melodiaSongOneLevelFourDict;

        Dictionary<string, long> melodiaSongThreeLevelOneDict;
        Dictionary<string, long> melodiaSongThreeLevelTwoDict;
        Dictionary<string, long> melodiaSongThreeLevelThreeDict;
        Dictionary<string, long> melodiaSongThreeLevelFourDict;      
        
        Dictionary<string, long> melodiaSongFourLevelOneDict;
        Dictionary<string, long> melodiaSongFourLevelTwoDict;
        Dictionary<string, long> melodiaSongFourLevelThreeDict;
        Dictionary<string, long> melodiaSongFourLevelFourDict;
        #endregion

        #region SketchUP Dictionary
        Dictionary<string, long> sketchUplevelOneDict;
        Dictionary<string, long> sketchUplevelTwoDict;
        Dictionary<string, long> sketchUplevelThreeDict;
        Dictionary<string, long> sketchUplevelFourDict;
        #endregion

        [Space]
        [SerializeField] string id = null;
        public string ID { get { return id; } }

        #region LevelString
        string levelOne = "levelOne";
        string levelTwo = "levelTwo";
        string levelThree = "levelThree";
        string levelFour = "levelFour";

        string songOneLevelOne = "songOneLevelOne";
        string songOneLevelTwo = "songOneLevelTwo";
        string songOneLevelThree = "songOneLevelThree";
        string songOneLevelFour = "songOneLevelFour";

        string songTwoLevelOne = "songTwoLevelOne";
        string songTwoLevelTwo = "songTwoLevelTwo";
        string songTwoLevelThree = "songTwoLevelThree";
        string songTwoLevelFour = "songTwoLevelFour";

        string songThreeLevelOne = "songThreeLevelOne";
        string songThreeLevelTwo = "songThreeLevelTwo";
        string songThreeLevelThree = "songThreeLevelThree";
        string songThreeLevelFour = "songThreeLevelFour";

        string songFourLevelOne = "songFourLevelOne";
        string songFourLevelTwo = "songFourLevelTwo";
        string songFourLevelThree = "songFourLevelThree";
        string songFourLevelFour = "songFourLevelFour";
        #endregion

        public Dictionary<string, long> sortDict;
        [Space]
        [SerializeField] TopTenRankMgr topTenRank = null;

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
            freezeLevelOneDict = new Dictionary<string, long>();
            freezeLevelTwoDict = new Dictionary<string, long>();
            freezeLevelThreeDict = new Dictionary<string, long>();
            freezeLevelFourDict = new Dictionary<string, long>();

            heyCookLevelOneDict = new Dictionary<string, long>();
            heyCookLevelTwoDict = new Dictionary<string, long>();
            heyCookLevelThreeDict = new Dictionary<string, long>();
            heyCookLevelFourDict = new Dictionary<string, long>();

            melodiaSongOneLevelOneDict = new Dictionary<string, long>();
            melodiaSongOneLevelTwoDict = new Dictionary<string, long>();
            melodiaSongOneLevelThreeDict = new Dictionary<string, long>();
            melodiaSongTwoLevelFourDict = new Dictionary<string, long>();

            melodiaSongTwoLevelOneDict = new Dictionary<string, long>();
            melodiaSongTwoLevelTwoDict = new Dictionary<string, long>();
            melodiaSongTwoLevelThreeDict = new Dictionary<string, long>();
            melodiaSongOneLevelFourDict = new Dictionary<string, long>();

            melodiaSongThreeLevelOneDict = new Dictionary<string, long>();
            melodiaSongThreeLevelTwoDict = new Dictionary<string, long>();
            melodiaSongThreeLevelThreeDict = new Dictionary<string, long>();
            melodiaSongThreeLevelFourDict = new Dictionary<string, long>();

            melodiaSongFourLevelOneDict = new Dictionary<string, long>();
            melodiaSongFourLevelTwoDict = new Dictionary<string, long>();
            melodiaSongFourLevelThreeDict = new Dictionary<string, long>();
            melodiaSongFourLevelFourDict = new Dictionary<string, long>();

            sketchUplevelOneDict = new Dictionary<string, long>();
            sketchUplevelTwoDict = new Dictionary<string, long>();
            sketchUplevelThreeDict = new Dictionary<string, long>();
            sketchUplevelFourDict = new Dictionary<string, long>();
            #endregion

            #region Save Data to DB
            //SaveFreezeData(freezeData, "안녕1", 10);
            //SaveFreezeData(freezeData, "안녕2", 20);
            #endregion

            #region DB Data All Delete
            //DeleteAllCollectionDataBase(FreezeCollection);
            //DeleteAllCollectionDataBase(HeyCookCollection);
            //DeleteAllCollectionDataBase(MelodiaCollection);
            //DeleteAllCollectionDataBase(SketchUpCollection);
            #endregion

            #region Find ID Check
            //CheckFreezeID(ID);
            //CheckHeyCookID(ID);
            //CheckMelodiaID(ID);
            //CheckSketchUPID(ID);
            #endregion

            #region Changed Point
            //ChangedPoint(SketchUpCollection, "안녕2", "levelTwo", 250);
            //ChangedPoint(SketchUpCollection, "안녕4", "levelThree", 400);
            //ChangedPoint(SketchUpCollection, "안녕6", "levelFour", 600);
            #endregion

            #region Get Each Collection All Data
            #region Freeze
            GetAllUserData(FreezeCollection, levelOne, ID, freezeLevelOneDict, topTenRank.freezeLevelShowRanking[0], 1, topTenRank.playerDataFreezeList);
            GetAllUserData(FreezeCollection, levelTwo, ID, freezeLevelTwoDict, topTenRank.freezeLevelShowRanking[1], 2, topTenRank.playerDataFreezeList);
            GetAllUserData(FreezeCollection, levelThree, ID, freezeLevelThreeDict, topTenRank.freezeLevelShowRanking[2], 3, topTenRank.playerDataFreezeList);
            GetAllUserData(FreezeCollection, levelFour, ID, freezeLevelFourDict, topTenRank.freezeLevelShowRanking[3], 4, topTenRank.playerDataFreezeList);
            #endregion

            #region HeyCook
            GetAllUserData(HeyCookCollection, levelOne, ID, heyCookLevelOneDict, topTenRank.heyCookLevelShowPanking[0], 1, topTenRank.playerDataHeyCookList);
            GetAllUserData(HeyCookCollection, levelTwo, ID, heyCookLevelTwoDict, topTenRank.heyCookLevelShowPanking[1], 2, topTenRank.playerDataHeyCookList);
            GetAllUserData(HeyCookCollection, levelThree, ID, heyCookLevelThreeDict, topTenRank.heyCookLevelShowPanking[2], 3, topTenRank.playerDataHeyCookList);
            GetAllUserData(HeyCookCollection, levelFour, ID, heyCookLevelFourDict, topTenRank.heyCookLevelShowPanking[3], 4, topTenRank.playerDataHeyCookList);
            #endregion

            GetAllUserData(MelodiaCollection, songOneLevelOne, ID, melodiaSongOneLevelOneDict, topTenRank.melodiaSongOneRanking[0], 1,topTenRank.playerMelodiaSongOneDataList);
            GetAllUserData(MelodiaCollection, songOneLevelTwo, ID, melodiaSongOneLevelTwoDict, topTenRank.melodiaSongOneRanking[1], 2,topTenRank.playerMelodiaSongOneDataList);
            GetAllUserData(MelodiaCollection, songOneLevelThree, ID, melodiaSongOneLevelThreeDict, topTenRank.melodiaSongOneRanking[2], 3,topTenRank.playerMelodiaSongOneDataList);
            GetAllUserData(MelodiaCollection, songOneLevelFour, ID, melodiaSongOneLevelFourDict, topTenRank.melodiaSongOneRanking[3], 4,topTenRank.playerMelodiaSongOneDataList);

            GetAllUserData(MelodiaCollection, songTwoLevelOne, ID, melodiaSongTwoLevelOneDict, topTenRank.melodiaSongTwoRanking[0], 1, topTenRank.playerMelodiaSongTwoDataList);
            GetAllUserData(MelodiaCollection, songTwoLevelTwo, ID, melodiaSongTwoLevelTwoDict, topTenRank.melodiaSongTwoRanking[1], 2, topTenRank.playerMelodiaSongTwoDataList);
            GetAllUserData(MelodiaCollection, songTwoLevelThree, ID, melodiaSongTwoLevelThreeDict, topTenRank.melodiaSongTwoRanking[2], 3, topTenRank.playerMelodiaSongTwoDataList);
            GetAllUserData(MelodiaCollection, songTwoLevelFour, ID, melodiaSongTwoLevelFourDict, topTenRank.melodiaSongTwoRanking[3], 4, topTenRank.playerMelodiaSongTwoDataList);

            GetAllUserData(MelodiaCollection, songThreeLevelOne, ID, melodiaSongThreeLevelOneDict, topTenRank.melodiaSongThreeRanking[0], 1, topTenRank.playerMelodiaSongThreeDataList);
            GetAllUserData(MelodiaCollection, songThreeLevelTwo, ID, melodiaSongThreeLevelTwoDict, topTenRank.melodiaSongThreeRanking[1], 2, topTenRank.playerMelodiaSongThreeDataList);
            GetAllUserData(MelodiaCollection, songThreeLevelThree, ID, melodiaSongThreeLevelThreeDict, topTenRank.melodiaSongThreeRanking[2], 3, topTenRank.playerMelodiaSongThreeDataList);
            GetAllUserData(MelodiaCollection, songThreeLevelFour, ID, melodiaSongThreeLevelFourDict, topTenRank.melodiaSongThreeRanking[3], 4, topTenRank.playerMelodiaSongThreeDataList);

            GetAllUserData(MelodiaCollection, songFourLevelOne, ID, melodiaSongFourLevelOneDict, topTenRank.melodiaSongFourRanking[0], 1, topTenRank.playerMelodiaSongFourDataList);
            GetAllUserData(MelodiaCollection, songFourLevelTwo, ID, melodiaSongFourLevelTwoDict, topTenRank.melodiaSongFourRanking[1], 2, topTenRank.playerMelodiaSongFourDataList);
            GetAllUserData(MelodiaCollection, songFourLevelThree, ID, melodiaSongFourLevelThreeDict, topTenRank.melodiaSongFourRanking[2], 3, topTenRank.playerMelodiaSongFourDataList);
            GetAllUserData(MelodiaCollection, songFourLevelFour, ID, melodiaSongFourLevelFourDict, topTenRank.melodiaSongFourRanking[3], 4, topTenRank.playerMelodiaSongFourDataList);

            #region SketchUp
            GetAllUserData(SketchUpCollection, levelOne, ID, sketchUplevelOneDict, topTenRank.sketchUPLevelShowRanking[0], 1, topTenRank.playerDataSketchUPList);
            GetAllUserData(SketchUpCollection, levelTwo, ID, sketchUplevelTwoDict, topTenRank.sketchUPLevelShowRanking[1], 2, topTenRank.playerDataSketchUPList);
            GetAllUserData(SketchUpCollection, levelThree, ID, sketchUplevelThreeDict, topTenRank.sketchUPLevelShowRanking[2], 3, topTenRank.playerDataSketchUPList);
            GetAllUserData(SketchUpCollection, levelFour, ID, sketchUplevelFourDict, topTenRank.sketchUPLevelShowRanking[3], 4, topTenRank.playerDataSketchUPList);
            #endregion
            #endregion
        }

        #region GetAndSetUserData
        public async void GetAllUserData(IMongoCollection<BsonDocument> collection, string levelNum, 
            string findId, Dictionary<string, long> dict, RectTransform pos, int level, List<playerData> list)
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
                //Debug.Log("levelArry : " + levelArry);
                point = (long)levelArry[0];
                //Debug.Log("point : " + point);
                dict.Add(id, point);
            }

            //Debug.Log("Dict Count : " + dict.Count);
            CheckSorting(list, level, findId, dict, pos);
            sortDict.Clear();
        }

        #region
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
                //freezeRankingDictionary.Add(id, point);
            }

            //CheckSorting(findId, freezeRankingDictionary, pos);
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
                //heyCookRankingDictionary.Add(id, point);
            }

            //CheckSorting(levelNum, findId, heyCookRankingDictionary, pos);
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
                //melodiaRankingDictionary.Add(id, point);
            }

            //CheckSorting(songLevelNum, findId, melodiaRankingDictionary, pos);
            sortDict.Clear();
        }
        public async void GetSketchUpUserDatas(string levelNum, string findId, Dictionary<string, long> dict, RectTransform pos, int level)
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

            //CheckSorting(level, findId, dict, pos);
            sortDict.Clear();
        }
        #endregion

        Dictionary<string, long> CheckSorting(List<playerData> list,int levelNum, string id, Dictionary<string, long> dict, RectTransform pos)
        {
            sortDict = new Dictionary<string, long>();
            sortDict = SortDictionary(dict);

            int rank = 1;

            foreach (KeyValuePair<string, long> item in sortDict)
            {
                if (item.Key.Equals(id))
                {
                    topTenRank.PlayerDataAdd(list, levelNum, rank, item.Key, item.Value, pos);
                }

                topTenRank.InstUserData(rank, item.Key, item.Value, pos);
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
            Debug.Log("[Freeze] NewData 추가 완료 : " + newData.ToBsonDocument());
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
            Debug.Log("[HeyCook] NewData 추가 완료 : " + newData.ToBsonDocument());
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
            Debug.Log("[Melodia] NewData 추가 완료 :" + newData.ToBsonDocument());
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
            Debug.Log("[SketchUp] NewData 추가 완료 : " + newData.ToBsonDocument());
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
            string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // 현재 시간
            long time = long.Parse(nowDate);
            return time;
        }
        #endregion
    }
}