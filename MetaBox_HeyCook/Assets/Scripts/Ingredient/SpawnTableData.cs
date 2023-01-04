using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/SpawnTableData", fileName = "SpawnTableData")]
public class SpawnTableData : ScriptableObject
{
    public List<GameObject> SpawnTable = new();
}