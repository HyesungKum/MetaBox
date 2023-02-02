using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : DataLoader
{
    static Dictionary<int, List<int>> myStageData = new();
    public Dictionary<int, List<int>> MyStageData { get { return myStageData; } }

    public void GetStageData(string targetStage)
    {
        myStageData = StageData(targetStage);
    }

    public void BuildStage(int stage)
    {
        

    }








}
