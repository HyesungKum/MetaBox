using UnityEngine;

[CreateAssetMenu(fileName = "ClearAnimalImgData", menuName = "Data/ClearAnimalImgData", order = 0)]

public class ClearAnimalImgData : ScriptableObject
{
    [Header("[ Level One ]")]
    public Sprite[] LevelOneClearAniamlImg;
    //[SerializeField] public Sprite polarbearClearImg = null;
    //[SerializeField] public Sprite reindeerClearImg = null;
    //[SerializeField] public Sprite penguinClearImg = null;

    [Header("[ Level Two ]")]
    public Sprite[] LevelTwoClearAniamlImg;
    //[SerializeField] public Sprite orcaClearImg = null;
    //[SerializeField] public Sprite walrusClearImg = null;
    //[SerializeField] public Sprite dolphinClearImg = null;

    [Header("[ Level Three ]")]
    public Sprite[] LevelThreeClearAniamlImg;

    //[SerializeField] public Sprite giraffeClearImg = null;
    //[SerializeField] public Sprite elephantClearImg = null;
    //[SerializeField] public Sprite cheetachClearImg = null;

    [Header("[ Level Four ]")]
    public Sprite[] LevelFourClearAniamlImg;
    //[SerializeField] public Sprite tigerClearImg = null;
    //[SerializeField] public Sprite rabbitClearImg = null;
    //[SerializeField] public Sprite deerClearImg = null;
}
