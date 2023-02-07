using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Data/AnimalData", order = 0)]

public class ChoicePanelData : ScriptableObject
{
    [Header("[Level Choice Panel Animal String]")]
    [SerializeField] private string polarbear = "�ϱذ�";
    [SerializeField] private string reindeer = "����";
    [SerializeField] private string penguin = "���";
    [SerializeField] private string orca = "����";
    [SerializeField] private string walrus = "�ٴ��ڳ���";
    [SerializeField] private string dolphin = "����";
    [SerializeField] private string giraffe = "�⸰";
    [SerializeField] private string elephant = "�ڳ���";
    [SerializeField] private string cheetah = "ġŸ";
    [SerializeField] private string tiger = "ȣ����";
    [SerializeField] private string deer = "�罿";
    [SerializeField] private string rabbit = "�䳢";

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