using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelTable
{
    [SerializeField] public int level;
    //game setting data
    [Header("Level Value Settings")]
    [SerializeField] public int countDown;
    [SerializeField] public float beltSpeed;
    [SerializeField] public float spawnTime;
    [Space]
    [SerializeField] public int ImmeTime;
    [SerializeField] public float ImmeBeltSpeed;
    [SerializeField] public float ImmeSpawnTime;

    [Header("Data Reference Sttings")]
    [SerializeField] public FoodGroup foodDataGroup;
    [SerializeField] public IngredGroup ingredGroup;
    [SerializeField] public GuestGroup guestGroup;

    //image data
    [Header("Image Settings")]
    [SerializeField] public Sprite orderImage;
    [SerializeField] public Sprite kitchenImage;
    [SerializeField] public Sprite backGroundImage;
}

[CreateAssetMenu(menuName = "ScriptableObj/LevelData", fileName = "LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelTable> levelTables;
}
