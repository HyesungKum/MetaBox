using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour
{
    static private UserData instance;
    static public UserData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserData>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(UserData), typeof(UserData)).GetComponent<UserData>();
                }
            }
            return instance;
        }
    }

    MongoClient clientData = new MongoClient("mongodb+srv://metabox:metabox@metabox.4ur23zq.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase dataBase = null;
    IMongoCollection<BsonDocument> collection = null;

    [SerializeField] TMP_InputField inputUserID = null;
    [SerializeField] Button signInButton = null;

    string userID = null;
    public bool User { get; private set; } = false;
    public int GameGroup { get; set; }
    public int Level { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void Start()
    {
        // MongoDB database name
        dataBase = clientData.GetDatabase("RankingDB");
        // MongoDB collection name
        collection = dataBase.GetCollection<BsonDocument>("RankingCollection");

        //signInButton.onClick.AddListener(delegate { OnClick_SignIn(); });
    }

    void OnClick_SignIn()
    {
        if (inputUserID.text.Length < 4) return;

        string fileName = "UserData";
        string path = $"{Application.dataPath}/{fileName}.txt";

        File.WriteAllText(path, inputUserID.text);
        SceneManager.LoadScene(1);

    }
    public void InGame()
    {
        string fileName = "UserData";
        string path = $"{Application.dataPath}/{fileName}.txt";
        if (File.Exists(path))
        {
            User = true;
            userID = File.ReadAllText(path);
        }
    }

    public void Record(int playtime)
    {
        if (User == false || userID == null) return;
        DataProcess(playtime);
    }

    void DataProcess(int playtime)
    {
        BsonDocument filter = new BsonDocument { { "id", userID }, { "gameGroup", GameGroup }, { "gameLevel", Level } };
        BsonDocument targetData = collection.Find(filter).FirstOrDefault();
        string nowDate = DateTime.Now.ToString("yyyyMMddHHmm");
        long time = long.Parse(nowDate);

        if (targetData != null) //기록유
        {
            int prevTime = (int)targetData.GetValue("playtime");
            if (prevTime > playtime) //기록갱신
            {
                Debug.Log("기록을 업데이트합니다");

                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("playtime", playtime);
                //collection.FindOneAndUpdate(filter, update);
                collection.UpdateOne(filter,update);
            }
            else
            {
                Debug.Log("이전 기록이 더 좋습니다");
            }
        }
        else //기록무
        {
            Debug.Log("새로운 기록을 추가합니다");
            SaveScoreToDataBase(playtime, time);
        }
    }

    // New User Data Save in DB
    public async void SaveScoreToDataBase(int playtime, long date)
    {
        var document = new BsonDocument { { "id", userID }, { "gameGroup", GameGroup }, { "gameLevel", Level }, { "playtime", playtime }, { "date", date } };
        await collection.InsertOneAsync(document);
    }
}
