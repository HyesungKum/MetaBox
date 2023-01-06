using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StargeNameData", menuName = "Data/GameData", order = 2)]

public class StageNameData : ScriptableObject
{
    public string[] stageName = new string[10];   
}
