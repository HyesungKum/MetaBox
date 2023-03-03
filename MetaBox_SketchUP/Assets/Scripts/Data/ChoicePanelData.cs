using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalNameData", menuName = "Data/AnimalNameData", order = 0)]

public class ChoicePanelData : ScriptableObject
{
    [Header("[Level Choice Panel Animal String]")]
    [SerializeField] public string Polarbear = "�ϱذ�"; //�ϱذ�
    [SerializeField] public string Reindeer = "����";   //����
    [SerializeField] public string Penguin = "���";     //���
                     
    [SerializeField] public string Orca = "����";           //����
    [SerializeField] public string Walrus = "�ٴ��ڳ���";       //�ٴ��ڳ���
    [SerializeField] public string Dolphin = "����";     //����
            
    [SerializeField] public string Giraffe = "�⸰";     //�⸰
    [SerializeField] public string Elephant = "�ڳ���";   //�ڳ���
    [SerializeField] public string Cheetah = "ġŸ";     //ġŸ
              
    [SerializeField] public string Tiger = "ȣ����";         //ȣ����
    [SerializeField] public string Deer = "�罿";           //�罿
    [SerializeField] public string Rabbit = "�䳢";       //�䳢
}