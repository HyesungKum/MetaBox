using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct GameData
{
    public int level;
    public int gameGroup;
    public int stageGroup;
    public int countDown;
    public int invenNote;
    public int replayCooltime;
}

public struct StageData
{
    public int musicGroup;
    public int stageGroup;
    public int emptyNote;
}
public class DataLoader : MonoBehaviour
{
    List<GameData> listGameData = new List<GameData>();
    List<StageData> listStageData = new List<StageData>();

    protected void LoadGameData()
    {
        loadGameData();
        loadStageData();
    }

    void loadGameData()
    {
        TextAsset ta = Resources.Load<TextAsset>("Data/melodia");

        string[] lines = ta.text.Split("\r\n");

        for (int i = 2; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분
            string[] columes = lines[i].Split(',');

            GameData gameData = new GameData();
            gameData.level = int.Parse(columes[0]);
            gameData.gameGroup = int.Parse(columes[2]);
            gameData.stageGroup = int.Parse(columes[3]);
            gameData.countDown = int.Parse(columes[4]);
            gameData.invenNote = int.Parse(columes[5]);
            gameData.replayCooltime = int.Parse(columes[6]);

            listGameData.Add(gameData);
        }
    }

    void loadStageData()
    {
        TextAsset ta = Resources.Load<TextAsset>("Data/stage");

        string[] lines = ta.text.Split("\r\n");

        int selectMusic = 0;

        switch (StartUI.MySceneMode)
        {
            case SceneMode.littlestar:
                {
                    selectMusic = 0;
                }
                break;

            case SceneMode.rabbit:
                {
                    selectMusic = 1;
                }
                break;

            case SceneMode.butterfly:
                {
                    selectMusic = 2;
                }
                break;

            case SceneMode.stone:
                {
                    selectMusic = 3;
                }
                break;
        }

        for (int i = 2; i < lines.Length - 1; ++i)
        {
            // 데이터 1줄을 컴마로 구분한다.
            string[] columes = lines[i].Split(',');

            if(selectMusic == int.Parse(columes[0]))
            {
                StageData stageData = new StageData();
                stageData.musicGroup = int.Parse(columes[0]);
                stageData.stageGroup = int.Parse(columes[2]);
                stageData.emptyNote = int.Parse(columes[3]);

                listStageData.Add(stageData);
            }
        }
    }

    protected GameData FindGameDataByLevel(int level)
    {
        GameData gameData = listGameData.Find(gameData => gameData.level == level);
        return gameData;
    }

    protected List<StageData> FindStageDatasByStageGroup(int stageGroup)
    {
        List<StageData> stages = new List<StageData>();

        for (int i = 0; i < listStageData.Count; ++i)
        {
            if (listStageData[i].stageGroup == stageGroup)
            {
                stages.Add(listStageData[i]);
            }
        }

        return stages;
    }

    protected Dictionary<int, List<int>> StageData(SceneMode targetStage)
    {
        Dictionary<int, List<int>> stageData = new();
        TextAsset sourcefile = Resources.Load<TextAsset>($"Data/{targetStage}");
        StringReader sr = new StringReader(sourcefile.text);

        // first line of CSV
        string header = sr.ReadLine();

        string line;

        int stage = 0;

        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(',');

            // if empty stage 
            if (data[1] == "")  
                continue;

            List<int> notes = new List<int>();

            for (int i = 1; i < data.Length; ++i)
            {
                if (data[i] == "")
                    break;

                notes.Add(int.Parse(data[i]));
            }

            stageData.Add(int.Parse(data[0]), notes);

            stage++;

        }

        return stageData;
    }

}
