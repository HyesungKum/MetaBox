using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDatas : MonoBehaviour
{
    private string polarbear = "�ϱذ�";
    private string reindeer = "����";
    private string penguin = "���";
    private string orca = "����";
    private string walrus = "�ٴ��ڳ���";
    private string dolphin = "����";
    private string giraffe = "�⸰";
    private string elephant = "�ڳ���";
    private string cheetah = "ġŸ";
    private string tiger = "ȣ����";
    private string deer = "�罿";
    private string rabbit = "�䳢";

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
        // ����Ʈ�� �߰�
        animalList.Add(Polarbear);  // �ϱذ�
        //Debug.Log(animalList.Contains("�⸰")); ;
        animalList.Add(Reindeer); // ����
        animalList.Add(Penguin); // ���
        animalList.Add(Orca); // ����
        animalList.Add(Walrus); // �ٴ��ڳ���
        animalList.Add(Dolphin); // ����
        animalList.Add(Giraffe); // �⸰
        animalList.Add(Elephant); // �ڳ���
        animalList.Add(Cheetah); // ġŸ
        animalList.Add(Tiger); // ȣ����
        animalList.Add(Deer); // �罿
        animalList.Add(Rabbit); // �䳢

        //Debug.Log("List: " + animalList.ToString());
        //Debug.Log("List COunr: " + animalList.Count);
    }
}
