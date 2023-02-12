using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectPanelImgData", menuName = "Data/SelectPanelImgData", order = 0)]

public class SelectPanelImgData : ScriptableObject
{
    public Sprite[] LevelOneSelectPanelImg;

    public Sprite[] LevelTwoSelectPanelImg;

    public Sprite[] LevelThreeSelectPanelImg;

    public Sprite[] LevelFourSelectPanelImg;
}
