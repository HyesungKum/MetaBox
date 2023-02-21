using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalQPrefabData", menuName = "Data/AnimalQPrefabData", order = 0)]

public class AnimalQPrefabData : ScriptableObject
{
    [SerializeField] public GameObject[] Qprefab;
}