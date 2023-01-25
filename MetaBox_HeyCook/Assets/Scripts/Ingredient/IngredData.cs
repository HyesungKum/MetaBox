using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/IngredientData", fileName = "IngredientData")]
public class IngredData : ScriptableObject
{
    public string ingredName;

    public float lifeTime;

    public Sprite ingredientImage;

    public GameObject delVfx;
}
