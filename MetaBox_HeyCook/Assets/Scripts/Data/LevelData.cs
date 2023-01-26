using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelTable
{
    [SerializeField] public int level;
    [SerializeField] public int countDown;
    [SerializeField] public float beltSpeed;

    //
    [SerializeField] public GuestGroup guestGroup;

    //
    //[SerializeField] public

    //
    [SerializeField] public Sprite conveyorBeltImage;
    [SerializeField] public Sprite submissionImage;
    [SerializeField] public Sprite orderImage;

    //
    [SerializeField] public Sprite kitchenImage;
    [SerializeField] public Sprite backGroundImage1;
    [SerializeField] public Sprite backGroundImage2;
}

[CreateAssetMenu(menuName = "ScriptableObj/LevelData", fileName = "LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelTable> levelTables;
}
