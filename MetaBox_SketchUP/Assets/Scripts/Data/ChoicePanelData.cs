using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Data/AnimalData", order = 0)]

public class ChoicePanelData : ScriptableObject
{
    [Header("[Level Choice Panel Animal String]")]
    [SerializeField] private string polarbear = "polarbear"; //ºÏ±Ø°õ
    [SerializeField] private string reindeer = "reindeer";   //¼ø·Ï
    [SerializeField] private string penguin = "penguin";     //Æë±Ï

    [SerializeField] private string orca = "orca";           //¹ü°í·¡
    [SerializeField] private string walrus = "walrus";       //¹Ù´ÙÄÚ³¢¸®
    [SerializeField] private string dolphin = "dolphin";     //µ¹°í·¡

    [SerializeField] private string giraffe = "giraffe";     //±â¸°
    [SerializeField] private string elephant = "elephant";   //ÄÚ³¢¸®
    [SerializeField] private string cheetah = "cheetah";     //Ä¡Å¸

    [SerializeField] private string tiger = "tiger";         //È£¶ûÀÌ
    [SerializeField] private string deer = "deer";           //»ç½¿
    [SerializeField] private string rabbit = "rabbit";       //Åä³¢

    #region Property
    public string Polarbear { get { return polarbear; } set { polarbear = value; } }
    public string Reindeer { get { return reindeer; } set { reindeer = value; } }
    public string Penguin { get { return penguin; } set { penguin = value; } }
    public string Orca { get { return orca; } set { orca = value; } }
    public string Walrus { get { return walrus; } set { walrus = value; } }
    public string Dolphin { get { return dolphin; } set { dolphin = value; } }
    public string Giraffe { get { return giraffe; } set { giraffe = value; } }
    public string Elephant { get { return elephant; } set { elephant = value; } }
    public string Cheetah { get { return cheetah; } set { cheetah = value; } }
    public string Tiger { get { return tiger; } set { tiger = value; } }
    public string Deer { get { return deer; } set { deer = value; } }
    public string Rabbit { get { return rabbit; } set { rabbit = value; } }
    #endregion
}