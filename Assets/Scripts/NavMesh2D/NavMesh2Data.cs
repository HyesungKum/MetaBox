using System;
using UnityEngine;

[Serializable]
public class NavMesh2Hor
{
    [SerializeField] public NavNode[] customNodes = null;
}

[CreateAssetMenu(fileName = "NavMesh2Data", menuName = "ScriptableObj/NavMesh2Data")]
public class NavMesh2Data : ScriptableObject
{
    [SerializeField] public NavMesh2Hor[] NavMesh2DVer;
}