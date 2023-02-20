using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/IngredientData", fileName = "IngredientData")]
public class IngredData : ScriptableObject
{
    public string ingredName;

    [Header("Settings")]
    public float lifeTime;
    public Sprite ingredientImage;
    [Tooltip("default = 1, 0 is collider size zero, that value multiplication to collider size")]
    public float interDistance = 1;

    [Header("Delete Vfx")]
    public GameObject delVfx; 
}
