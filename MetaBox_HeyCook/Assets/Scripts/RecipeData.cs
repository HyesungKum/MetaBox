using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/RecipeData", fileName = "RecipeData")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public IngredData[] needIngred;

    public Sprite cooked;
    public Sprite raw;

    public bool RightCombine;
    public int TaskCount;
}

