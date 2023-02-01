using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/IngredGroup", fileName = "IngredGroup")]
public class IngredGroup : ScriptableObject
{
    public IngredData[] ingreds;
}
