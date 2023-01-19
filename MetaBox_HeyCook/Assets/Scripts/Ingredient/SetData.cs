using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/SetData", fileName = "SetData")]
public class SetData : ScriptableObject
{
    public string setName;

    [Header("need child Ingredient List")]
    public IngredData[] needIngred;

    [Header("Ingreding Combine Recipe Image")]
    public Sprite CombineImage;

    [Header("Cooked Particle")]
    public GameObject particle;

    [Header("Cooking Task Count")]
    [Tooltip("ex) TaskCount =10 -> need 10 times touch for make Food")]
    public int TaskCount;

    [Header("Value")]
    public int Score;
}
