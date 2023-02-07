using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDatas : MonoBehaviour
{
    private string polarbear = "ºÏ±Ø°õ";
    private string reindeer = "¼ø·Ï";
    private string penguin = "Æë±Ï";
    private string orca = "¹ü°í·¡";
    private string walrus = "¹Ù´ÙÄÚ³¢¸®";
    private string dolphin = "µ¹°í·¡";
    private string giraffe = "±â¸°";
    private string elephant = "ÄÚ³¢¸®";
    private string cheetah = "Ä¡Å¸";
    private string tiger = "È£¶ûÀÌ";
    private string deer = "»ç½¿";
    private string rabbit = "Åä³¢";

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

    public List<string> animalList;

    void Awake()
    {
        animalList = new List<string>();
        // ¸®½ºÆ®¿¡ Ãß°¡
        animalList.Add(Polarbear);  // ºÏ±Ø°õ
        //Debug.Log(animalList.Contains("±â¸°")); ;
        animalList.Add(Reindeer); // ¼ø·Ï
        animalList.Add(Penguin); // Æë±Ï
        animalList.Add(Orca); // ¹ü°í·¡
        animalList.Add(Walrus); // ¹Ù´ÙÄÚ³¢¸®
        animalList.Add(Dolphin); // µ¹°í·¡
        animalList.Add(Giraffe); // ±â¸°
        animalList.Add(Elephant); // ÄÚ³¢¸®
        animalList.Add(Cheetah); // Ä¡Å¸
        animalList.Add(Tiger); // È£¶ûÀÌ
        animalList.Add(Deer); // »ç½¿
        animalList.Add(Rabbit); // Åä³¢

        //Debug.Log("List: " + animalList.ToString());
        //Debug.Log("List COunr: " + animalList.Count);
    }
}
