using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelTable
{
    //game setting data
    [SerializeField] public int level;
    [SerializeField] public int countDown;
    [SerializeField] public float beltSpeed;
    //[SerializeField] public FoodData foodData; ������ �ٸ� ���� ������
    //[SerializeField] public Ingredient ingred; ���Ŀ� �´� ��� ������

    //guest table data
    [SerializeField] public GuestGroup guestGroup;

    //image data
    [SerializeField] public Sprite conveyorBeltImage;
    [SerializeField] public Sprite submissionImage;
    [SerializeField] public Sprite orderImage;
    [SerializeField] public Sprite kitchenImage;
    [SerializeField] public Sprite backGroundImage1;
    [SerializeField] public Sprite backGroundImage2;
}

[CreateAssetMenu(menuName = "ScriptableObj/LevelData", fileName = "LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelTable> levelTables;
}
