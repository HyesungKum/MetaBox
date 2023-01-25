using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataLoader : MonoBehaviour
{
    protected Dictionary<int, List<int>> StageData(string targetStage)
    {
        Dictionary<int, List<int>> stageData = new();
        TextAsset sourcefile = Resources.Load<TextAsset>($"StageData/{targetStage}");
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
            Debug.Log($"{stage}��������! {data[0]} �������� ��ȣ ");

        }

        return stageData;
    }






}
