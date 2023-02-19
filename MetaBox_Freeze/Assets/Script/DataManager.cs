using System.Collections.Generic;
using UnityEngine;

public struct GameData
{
    public int level;
    public int gameGroup;
    public int stageGroup;
    public int stageCount;
    public int playTime;
    public int playerSpeed;
    public int playerArea;
}

public struct StageData
{
    public int stageGroup;
    public int thiefGroup;
    public int thiefCount;
    public int wantedCount;
    public int startCountdown;
    public int penaltyPoint;
}

public struct ThiefData
{
    public int id;
    public int thiefGroup;
    public int moveSpeed;
    public int moveTime;
}

public class DataManager
{
    #region 싱글턴
    static DataManager instance = null;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }
    #endregion

    List<GameData> listGameData = new List<GameData>();
    List<StageData> listStageData = new List<StageData>();
    List<ThiefData> listThiefData = new List<ThiefData>();

    
    public void LoadGameData()
    {
        loadGameData();
        loadStageData();
        loadThiefData();
    }

    void loadGameData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PoliceGame");

        string[] lines = ta.text.Split("\r\n");
        
        for (int i = 2; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분
            string[] columes = lines[i].Split(',');

            GameData gameData = new GameData();
            gameData.level = int.Parse(columes[0]);
            gameData.gameGroup = int.Parse(columes[1]);
            gameData.stageGroup = int.Parse(columes[2]);
            gameData.stageCount = int.Parse(columes[3]);
            gameData.playTime = int.Parse(columes[4]);
            gameData.playerSpeed = int.Parse(columes[5]);
            gameData.playerArea = int.Parse(columes[6]);

            listGameData.Add(gameData);
        }
    }


    void loadStageData()
    {
        TextAsset ta = Resources.Load<TextAsset>("PoliceStage");

        string[] lines = ta.text.Split("\r\n");

        for (int i = 2; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분한다.
            string[] columes = lines[i].Split(',');

            StageData stageData = new StageData();
            stageData.stageGroup = int.Parse(columes[1]);
            stageData.thiefGroup = int.Parse(columes[2]);
            stageData.thiefCount = int.Parse(columes[3]);
            stageData.wantedCount = int.Parse(columes[4]);
            stageData.startCountdown = int.Parse(columes[5]);
            stageData.penaltyPoint = int.Parse(columes[6]);

            listStageData.Add(stageData);
        }
    }


    void loadThiefData()
    {
        TextAsset ta = Resources.Load<TextAsset>("Thieves");

        string[] lines = ta.text.Split("\r\n");

        for (int i = 2; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분
            string[] columes = lines[i].Split(',');

            ThiefData thiefData = new ThiefData();
            thiefData.id = int.Parse(columes[0]);
            thiefData.thiefGroup = int.Parse(columes[2]);
            thiefData.moveSpeed = int.Parse(columes[3]);
            thiefData.moveTime = int.Parse(columes[4]);

            listThiefData.Add(thiefData);
        }
    }




    public GameData FindGameDataByLevel(int level)
    {
        GameData gameData = listGameData.Find(gameData => gameData.level == level);
        return gameData;
    }

    public List<StageData> FindStageDatasByStageGroup(int stageGroup, int stageCount)
    {
        List<StageData> stages = new List<StageData>();

        for (int i = 0; i < listStageData.Count; ++i)
        {
            if (listStageData[i].stageGroup == stageGroup)
            {
                stages.Add(listStageData[i]);
            }
        }

        while (stages.Count > stageCount)
        {
            stages.RemoveAt(Random.Range(0, stages.Count));
        }
        return stages;
    }

    public List<ThiefData> FindThiefDatasByThiefGroup(int thiefGroup)
    {
        List<ThiefData> Thieves = new List<ThiefData>();

        for (int i = 0; i < listThiefData.Count; ++i)
        {
            if (listThiefData[i].thiefGroup == thiefGroup)
            {
                Thieves.Add(listThiefData[i]);
            }
        }

        return Thieves;
    }

}
