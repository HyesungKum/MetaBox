using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RankingDataBase : MonoBehaviour
{
    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase dataBase = null;
    IMongoCollection<BsonDocument> collection = null;

    List<BsonDocument> userdata = new List<BsonDocument>();


    ///UserTopRanking userTopRanking = null;

    [SerializeField] GameObject userDataPerfab = null;

    void Start()
    {
        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");
        // MongoDB collection name
        collection = dataBase.GetCollection<BsonDocument>("RankingCollection");

        //GetUserData(1, 1);

        #region 데이터 추가 및 체크
        //DataDelete(9); // 해당 아이디 도큐먼트 전부 삭제

        long time = TimeSetting();
        //SaveScoreToDataBase(5, 1, 2, 5, time); // 데이터 추가
        //SaveScoreToDataBase(5, 1, 3, 6, time); // 데이터 추가

        //DataFindGameGroup(1); // 게임 그룹으로 찾기
        //DataFindGame(2, 2); // 게임 그룹 및 레벨로 찾기
        //DataFindId(1);
        #endregion
    }

    // Play Game Time Setting : Year/Month/Day/Hour/Minute
    public long TimeSetting()
    {
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm"); // 현재 시간
        long time = long.Parse(nowDate);

        Debug.Log("nowDate:" + time);

        return time;
    }

    // New User Data Save in DB
    public async void SaveScoreToDataBase(int id, int gameGroup, int gameLevel, int point, long date)
    {
        if (gameGroup > 4 && gameLevel > 4) return;

        var document = new BsonDocument { { "id", id }, { "gameGroup", gameGroup }, { "gameLevel", gameLevel }, { "point", point }, { "date", date } };

        // 체크 포인트로 유저 DB 중복 체크 안함
        //if (DataFindID(id) != null)
        //{
        await collection.InsertOneAsync(document);
        Debug.Log("## DB에 데이터 추가 완료 : " + document.ToString());
        //}

    }

    // Check ID in DB
    BsonDocument DataFindID(int id)
    {
        BsonDocument filter = new BsonDocument { { "id", id } };
        BsonDocument targetData = collection.Find(filter).FirstOrDefault();

        return targetData;
    }

    public int id { get; set; }
    public int point { get; set; }

    public int checkCount;

    public int checkRanking = 0;

    // Find Target(gameGroup, gameLevel) DB Data 
    public async Task<List<BsonDocument>> GetUserData(int gameGroup, int gameLevel, RectTransform pos)
    {
        BsonDocument find = new BsonDocument { { "gameGroup", gameGroup }, { "gameLevel", gameLevel } };
        var allScoresTask = collection.FindAsync(find);
        var scoreAwaited = await allScoresTask;

        #region test
        //if(checkid != null) 
        //{
        //    int check = (int)checkid.GetValue("id");
        //    Debug.Log("check = " + check);

        //    int point = (int)checkid.GetValue("point");
        //    Debug.Log("point = " + point);
        //}
        #endregion

        checkRanking = 0;

        foreach (var score in scoreAwaited.ToList())
        {
            //Debug.Log("score : " + score.ToString());

            id = (int)score.GetValue("id");
            //Debug.Log("id : " + id);

            point = (int)score.GetValue("point");
            //Debug.Log("point : " + point);

            checkRanking += 1;
            if (checkRanking == 11) break;

            UserDataPrefabInst(checkRanking, pos);

            userdata.Add(score);
        }

        checkCount = userdata.Count;
        Debug.Log("## 1 checkCount = " + checkCount);
        return userdata;
    }

    void UserDataPrefabInst(int count, RectTransform pos)
    {
        ///userTopRanking = userDataPerfab.GetComponent<UserTopRanking>();
        ///userTopRanking.UiSetting(id, point, count);

        GameObject inst = Instantiate(userDataPerfab, Vector2.zero, Quaternion.identity, pos);

        //Debug.Log("추가됨 ??");
    }


    public void DataSorting(int point)
    {
        bool check = userdata.Contains((BsonDocument)point);
        Debug.Log("check = " + check);
        int cur = point;
        Debug.Log("cur = " + cur);

        //userdata.Sort();
        Debug.Log("Sort : " + userdata.ToString());

    }

    // Get User ScoreData
    public int DetaFindld(int id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", id);//찾을 도큐먼트의 Name이 아닌것
        //Debug.Log(filter.ToString());
        var nullFilter = collection.Find(filter).FirstOrDefault();//if null 이면 찾지 못함
        //Debug.Log(nullFilter.ToString());
        int check = 0;
        if (nullFilter != null)
        {
            check = (int)nullFilter.GetValue("point");
            //Debug.Log(nullFilter.GetValue("point")); // 특정 필드를 찾음.
            return check;
        }
        return check;
    }


    // Fing User id 
    public BsonDocument FindId(int id)
    {
        BsonDocument findId = new BsonDocument { { "id", id } };

        BsonDocument gameGroups = collection.Find(findId).FirstOrDefault();

        Debug.Log(gameGroups.ToString());

        return gameGroups;
    }

    // DB Data Delete Many
    public void DataDelete(int id)
    {
        BsonDocument filter = new BsonDocument { { "id", id } };
        collection.DeleteMany(filter);
        Debug.Log("## DB에 데이터 삭제 완료 : " + filter.ToString());
    }
}