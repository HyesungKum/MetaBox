using MongoDB.Bson;
using MongoDB.Driver;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

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
    [SerializeField] Button IDSaveButton = null;
    [SerializeField] ChangeObject changeObject = null;

    public bool OnlineMode = true;
    int GetCharIndex => changeObject.index;
    private string id;
    public string ID { get { return id; } set { id = value; } }

    void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnlineMode = false;
            return;
        }

        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");

        // MongoDB collection name
        FreezeCollection = dataBase.GetCollection<BsonDocument>("FreezeRanking");
        HeyCookCollection = dataBase.GetCollection<BsonDocument>("HeyCookRanking");
        MelodiaCollection = dataBase.GetCollection<BsonDocument>("MelodiaRanking");
        SketchUpCollection = dataBase.GetCollection<BsonDocument>("SketchUpRanking");

        // === Button Event Setting ===
        IDSaveButton.interactable = false;
        inputID.onValueChanged.AddListener(delegate { CheckInputId(); });
        IDSaveButton.onClick.AddListener(delegate { SaveID(); });
    }

    void CheckInputId()
    {
        if (string.IsNullOrEmpty(inputID.text)) // �Է� �Ѱ� ������ ��ư ��Ȱ��ȭ
        {
            IDSaveButton.interactable = false;
        }
        else if (inputID.text.Length <= 10) // �Է��� ���̰� 10���� ���� �� 10 ���ڸ� ��ư Ȱ��ȭ
        {
            IDSaveButton.interactable = true;
        }
    }
    void SaveID()
    {
        ID = inputID.text;

        if (!OnlineMode)
        {
            EventReceiver.CallSaveEvent(ID, GetCharIndex, true);
            return;
        }

        BsonDocument checkFreezeID = CheckFreezeID(ID);
        BsonDocument checkHeyCookID = CheckHeyCookID(ID);
        BsonDocument checkMelodiaID = CheckMelodiaID(ID);
        BsonDocument checkSketchUpID = CheckSketchUpID(ID);

        if (checkFreezeID == null && checkHeyCookID == null &&
            checkMelodiaID == null && checkSketchUpID == null)
        {
            IDSaveButton.interactable = false;
            inputID.interactable = false;
            infoText.gameObject.SetActive(false);

            FreezeData freezeData = new();
            HeyCookData heyCookData = new();
            MelodiaData melodiaData = new();
            SketchUpData sketchUpData = new();

            SaveFreezeDataBase(freezeData, ID);
            SaveHeyCookDataBase(heyCookData, ID);
            SaveMelodiaDataBase(melodiaData, ID);
            SaveSketchUpDataBase(sketchUpData, ID);

            EventReceiver.CallSaveEvent(ID, GetCharIndex, true);
        }
        else
        {
            infoText.gameObject.SetActive(true);
            IDSaveButton.interactable = false;
        }
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
        newData.song = "�����";
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
