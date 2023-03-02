using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace RankingDB
{
    #region PoliRunData
    [Serializable]
    public class PoliRunData
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

    #region DreamSketchData
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

    [Serializable]
    public class DataSorting
    {
        public string id;
        public long point;
        public long playtime;
    }

    [Serializable]
    public struct UserData
    {
        public string id;
        public int charIndex;
        public bool troughTown;
    }

    public class MongoDBManager : MonoBehaviour
    {
        MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
        public IMongoDatabase dataBase = null;

        #region Collection
        public IMongoCollection<BsonDocument> PoliRunCollection = null;
        public IMongoCollection<BsonDocument> HeyCookCollection = null;
        public IMongoCollection<BsonDocument> MelodiaCollection = null;
        public IMongoCollection<BsonDocument> DreamSketchCollection = null;
        #endregion

        #region Each Game Data
        [Header("[PoliRun Data]")]
        [SerializeField] PoliRunData poliRunData = null;
        [Header("[HeyCook Data]")]
        [SerializeField] HeyCookData heyCookData = null;
        [Header("[Melodia Data]")]
        [SerializeField] MelodiaData melodiaData = null;
        [Header("[DreamSketch Data]")]
        [SerializeField] DreamSketchData dreamSketchData = null;
        #endregion
        [Space]
        public Dictionary<string, long> levelOneDict;
        public Dictionary<string, long> levelTwoDict;
        public Dictionary<string, long> levelThreeDict;
        public Dictionary<string, long> levelFourDict;

        DataSorting dataSorting = null;
        public List<DataSorting> LevelOneList;
        public List<DataSorting> LevelTwoList;
        public List<DataSorting> LevelThreeList;
        public List<DataSorting> LevelFourList;

        #region MelodiaDictionary
        public Dictionary<string, long> melodiaSongOneLevelOneDict;
        public Dictionary<string, long> melodiaSongOneLevelTwoDict;
        public Dictionary<string, long> melodiaSongOneLevelThreeDict;
        public Dictionary<string, long> melodiaSongTwoLevelFourDict;

        public Dictionary<string, long> melodiaSongTwoLevelOneDict;
        public Dictionary<string, long> melodiaSongTwoLevelTwoDict;
        public Dictionary<string, long> melodiaSongTwoLevelThreeDict;
        public Dictionary<string, long> melodiaSongOneLevelFourDict;

        public Dictionary<string, long> melodiaSongThreeLevelOneDict;
        public Dictionary<string, long> melodiaSongThreeLevelTwoDict;
        public Dictionary<string, long> melodiaSongThreeLevelThreeDict;
        public Dictionary<string, long> melodiaSongThreeLevelFourDict;      
        
        public Dictionary<string, long> melodiaSongFourLevelOneDict;
        public Dictionary<string, long> melodiaSongFourLevelTwoDict;
        public Dictionary<string, long> melodiaSongFourLevelThreeDict;
        public Dictionary<string, long> melodiaSongFourLevelFourDict;
        #endregion

        //=== app transition ===
        [Header("[Application Setting]")]
        [SerializeField] string fileName = "TownSaveData.json";
        [SerializeField] public string mainPackName = "com.MetaBox.MetaBox_Main";
#if UNITY_EDITOR
        [SerializeField] private string localSavePath = "/MetaBox/SaveData/SaveData.json";
#else
    private string localSavePath = "/storage/emulated/0/MetaBox/SaveData/SaveData.json";
#endif
        public UserData curUserData;

        private string id;
        public string ID { get { return id; } set { id = value; } }

        public Dictionary<string, long> sortDict;
        public List<DataSorting> sortList;

        [Space]
        [SerializeField] TopTenRankMgr topTenRank = null;

        void Awake()
        {
            #region MongoDB database name
            dataBase = clientData.GetDatabase("RankingDB");
            #endregion

            #region MongoDB collection name
            PoliRunCollection = dataBase.GetCollection<BsonDocument>("PoliRunRanking");
            HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
            MelodiaCollection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
            DreamSketchCollection = dataBase.GetCollection<BsonDocument>("DreamSketchRanking");
            #endregion

            if (File.Exists(localSavePath))
            {
                curUserData = ReadSaveData(localSavePath);
                ID = curUserData.id;
                //Debug.Log("id : " + id);
            }

            #region Data Reset
            dataSorting = new DataSorting();
            poliRunData = new PoliRunData();
            heyCookData = new HeyCookData();
            melodiaData = new MelodiaData();
            dreamSketchData = new DreamSketchData();
            #endregion

            #region User Ranking Data Dictionary Reset
            levelOneDict = new Dictionary<string, long>();
            levelTwoDict = new Dictionary<string, long>();
            levelThreeDict = new Dictionary<string, long>();
            levelFourDict = new Dictionary<string, long>();

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
            #endregion

            #region DB Data All Delete
            //DeleteAllCollectionDataBase(FreezeCollection);
            //DeleteAllCollectionDataBase(HeyCookCollection);
            //DeleteAllCollectionDataBase(MelodiaCollection);
            //DeleteAllCollectionDataBase(SketchUpCollection);
            #endregion

            #region Find ID Check
            //FindID();
            #endregion
        }

        #region GetAndSetUserData
        public async void GetAllUserData(IMongoCollection<BsonDocument> collection, string levelNum, GameObject playerData,
            string findId, Dictionary<string, long> dict, RectTransform pos, int level, List<playerData> list)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = collection.FindAsync(find);
            var scoreAwited = await allDataTask;

            dict = new Dictionary<string, long>();
            //list = new List<playerData>();

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

            CheckSorting(list, level, findId, playerData, dict, pos);
            sortDict.Clear();
        }

        public async void GetUserData(IMongoCollection<BsonDocument> collection, string levelNum, List<DataSorting> listAdd, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = collection.FindAsync(find);
            var scoreAwited = await allDataTask;
             
            BsonArray levelArry;
            DataSorting newData;
            string ids;
            long points;
            long playTime;

            foreach (var check in scoreAwited.ToList())
            {
                ids = (string)check.GetValue("_id");
                levelArry = (BsonArray)check.GetValue(levelNum);
                //Debug.Log("levelArry : " + levelArry);
                points = (long)levelArry[0];
                playTime = (long)levelArry[1];

                newData = new DataSorting();
                newData.id = ids;
                newData.point = points;
                newData.playtime = playTime;

                listAdd.Add(newData);
            }

            SotringList(listAdd, pos);
        }

        void SotringList(List<DataSorting> listSorting, RectTransform pos)
        {
            sortList = listSorting;
            int listCount = listSorting.Count;
            sortList.Sort(SortPoint);

            int rank = 1;

            for (int i = 0; i < 10; ++i)
            {
                //topTenRank.InstUserData(rank, sortList[i].id, sortList[i].point , pos);

                rank += 1;
                if (rank == 11) break;
            }
        }

        int SortPoint(DataSorting a, DataSorting b)
        {
            return a.point > b.point ? -1 : 1;
        }

        #region
        public async void GetFreezeUserData(string levelNum, string findId, RectTransform pos)
        {
            BsonDocument find = new BsonDocument();
            var allDataTask = PoliRunCollection.FindAsync(find);
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
            var allDataTask = DreamSketchCollection.FindAsync(find);
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

        Dictionary<string, long> CheckSorting(List<playerData> list,int levelNum, string id, GameObject playerData,
            Dictionary<string, long> dict, RectTransform pos)
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

            topTenRank.FirstLevelPlayerData(playerData, list);
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

        private UserData ReadSaveData(string path)
        {
            string dataStr = File.ReadAllText(path);
            UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

            return readData;
        }

        #region Find ID
        public BsonDocument FindID(IMongoCollection<BsonDocument> collection, string id)
        {
            BsonDocument findId = new BsonDocument { { "_id", id } };
            BsonDocument targetData = collection.Find(findId).FirstOrDefault();
            return targetData;
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