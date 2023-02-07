using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/FoodData", fileName = "FoodData")]
public class FoodData : ScriptableObject
{
    [Header("[Food Settings]")]
    public string foodName;

    [Header("[Food Image Setting]")]
    public Sprite foodImage;
    public Sprite combineImage;

    [Header("[Cooking Touch Type Setting]")]
    public CookType cookType;

    [Header("[Cooking Task Setting]")]
    public int needTask;
    public int Score;

    [Header("[Vfx]")]
    public GameObject cookingVfx;
    public GameObject foodVfx;

    [Header("[Recipe Data]")]
    public IngredData[] needIngred;
}

