using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/FoodData", fileName = "FoodData")]
public class FoodData : ScriptableObject
{
    public string foodName;

    [Header("need child SetMenu List")]
    public SetData[] needSet;
    
    [Header("Cooked Food Image")]
    public Sprite FoodImage;

    [Header("Value")]
    public int Score;
}

