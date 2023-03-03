using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalNameData", menuName = "Data/AnimalNameData", order = 0)]

public class ChoicePanelData : ScriptableObject
{
    [Header("[Level Choice Panel Animal String]")]
    [SerializeField] public string Polarbear = "ºÏ±Ø°õ"; //ºÏ±Ø°õ
    [SerializeField] public string Reindeer = "¼ø·Ï";   //¼ø·Ï
    [SerializeField] public string Penguin = "Æë±Ï";     //Æë±Ï
                     
    [SerializeField] public string Orca = "¹ü°í·¡";           //¹ü°í·¡
    [SerializeField] public string Walrus = "¹Ù´ÙÄÚ³¢¸®";       //¹Ù´ÙÄÚ³¢¸®
    [SerializeField] public string Dolphin = "µ¹°í·¡";     //µ¹°í·¡
            
    [SerializeField] public string Giraffe = "±â¸°";     //±â¸°
    [SerializeField] public string Elephant = "ÄÚ³¢¸®";   //ÄÚ³¢¸®
    [SerializeField] public string Cheetah = "Ä¡Å¸";     //Ä¡Å¸
              
    [SerializeField] public string Tiger = "È£¶ûÀÌ";         //È£¶ûÀÌ
    [SerializeField] public string Deer = "»ç½¿";           //»ç½¿
    [SerializeField] public string Rabbit = "Åä³¢";       //Åä³¢
}