using MongoDB.Bson;
using MongoDB.Driver;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public int level;
    public int point;
    public long date;
}
#endregion

public class MongoIDManager : MonoBehaviour
{
    readonly MongoClient clientData = new("mongodb+srv://metabox:metabox@metabox.fon8dvx.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase dataBase = null;
    IMongoCollection<BsonDocument> FreezeCollection = null;
    IMongoCollection<BsonDocument> HeyCookCollection = null;
    IMongoCollection<BsonDocument> MelodiaCollection = null;
    IMongoCollection<BsonDocument> SketchUpCollection = null;

    [Header("[Login UserID]")]
    [SerializeField] TMP_InputField inputID = null;
    [SerializeField] TextMeshProUGUI infoText = null;
    [SerializeField] Button saveId = null;
    [SerializeField] ChangeCharacter changeCharacter = null;

    private string id;
    public string ID { get { return id; } set { id = value; } }

    void Awake()
    {
        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");

        // MongoDB collection name
        FreezeCollection = dataBase.GetCollection<BsonDocument>("FreezeRanking");
        HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
        MelodiaCollection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
        SketchUpCollection = dataBase.GetCollection<BsonDocument>("SketchUpRanking");

        // === Button Event Setting ===
        saveId.interactable = false; // 버튼 비활성화서 ID 입력 후 활성화 해주기
        inputID.onValueChanged.AddListener(delegate { CheckInputId(); });
        saveId.onClick.AddListener(delegate { GetID(); });

        // === setting ===
        infoText.gameObject.SetActive(false);
    }

    void CheckInputId()
    {
        if (string.IsNullOrEmpty(inputID.text)) // 입력 한게 없으면 버튼 비활성화
        {
            saveId.interactable = false;
        }
        else if (inputID.text.Length <= 10) // 입력한 길이가 10글자 이하 및 10 글자면 버튼 활성화
        {
            saveId.interactable = true;
        }
    }
    string GetID()
    {
        ID = inputID.text;

        BsonDocument checkFreezeID = CheckFreezeID(ID);
        BsonDocument checkHeyCookID = CheckHeyCookID(ID);
        BsonDocument checkMelodiaID = CheckMelodiaID(ID);
        BsonDocument checkSketchUpID = CheckSketchUpID(ID);

        if (checkFreezeID == null && checkHeyCookID == null &&
            checkMelodiaID == null && checkSketchUpID == null)
        {
            saveId.interactable = true;

            FreezeData freezeData = new();
            HeyCookData heyCookData = new();
            MelodiaData melodiaData = new();
            SketchUpData sketchUpData = new();

            SaveFreezeDataBase(freezeData, ID);
            SaveHeyCookDataBase(heyCookData, ID);
            SaveMelodiaDataBase(melodiaData, ID);
            SaveSketchUpDataBase(sketchUpData, ID);

            EventReceiver.CallSelectDone();
            EventReceiver.CallSave(ID, changeCharacter.index ,true);
        }
        else
        {
            infoText.gameObject.SetActive(true);
            saveId.interactable = false;
        }
        return ID;
    }

    BsonDocument CheckFreezeID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = FreezeCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckHeyCookID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = HeyCookCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckMelodiaID(string findId)
    {
        BsonDocument filter = new () { { "_id", findId } };
        BsonDocument targetData = MelodiaCollection.Find(filter).FirstOrDefault();

        return targetData;
    }
    BsonDocument CheckSketchUpID(string findId)
    {
        BsonDocument filter = new() { { "_id", findId } };
        BsonDocument targetData = SketchUpCollection.Find(filter).FirstOrDefault();

        return targetData;
    }

    public async void SaveFreezeDataBase(FreezeData newData , string id)
    {
        newData.id = id;
        await FreezeCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveHeyCookDataBase(HeyCookData newData, string id)
    {
        newData.id = id;
        await HeyCookCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveMelodiaDataBase(MelodiaData newData, string id)
    {
        newData.id = id;
        newData.song = "나비야";
        int level = 1;
        int point = 2;
        int date = 3;

        newData.songLevelArry = new int[3];
        newData.songLevelArry[0] = level;
        newData.songLevelArry[1] = point;
        newData.songLevelArry[2] = date;

        await MelodiaCollection.InsertOneAsync(newData.ToBsonDocument());
    }
    public async void SaveSketchUpDataBase(SketchUpData newData, string id)
    {
        newData.id = id;
        await SketchUpCollection.InsertOneAsync(newData.ToBsonDocument());
    }

}
