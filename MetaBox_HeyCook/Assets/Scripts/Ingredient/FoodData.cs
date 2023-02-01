using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/FoodData", fileName = "FoodData")]
public class FoodData : ScriptableObject
{
    [Header("[Food Settings]")]
    public string foodName;
    public Sprite foodImage;
    public Sprite combineImage;
    public CookType cookType;
    public int needTask;
    public int Score;

    [Header("[Vfx]")]
    public GameObject foodVfx;

    [Header("Recipe Data")]
    public IngredData[] needIngred;
}

