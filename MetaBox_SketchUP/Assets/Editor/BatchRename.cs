using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BatchRename : ScriptableWizard
{
    // �⺻ �̸�
    public string BaseName = "Square";

    // ���� ����
    public int StartNumber = 0;

    // ����ġ
    public int Increment = 1;

    [MenuItem("Edit/Batch Rename")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
    }

    // â�� ó�� ��Ÿ�� �� ȣ��ȴ�
    void OnEnable()
    {
        UpdateSelectionHelper();
    }

    // ������ ���� ������ ����� �� ȣ��Ǵ� �Լ�
    void OnSelectionChange()
    {
        UpdateSelectionHelper();
    }

    // ���õ� ������ ������Ʈ�Ѵ�
    void UpdateSelectionHelper()
    {
        string helpString = "";

        if (Selection.objects != null)
            helpString = "Number of objects selected: " + Selection.objects.Length;

    }

    // �̸� ���� 
    void OnWizardCreate()
    {
        // ���õ� ���� ������ �����Ѵ�
        if (Selection.objects == null)
            return;

        // ���� ����ġ
        int PostFix = StartNumber;

        // ��ȸ�ϸ� �̸��� �����Ѵ�
        foreach(Object O in Selection.objects)
        {
            O.name = BaseName + PostFix;
            PostFix += Increment;
        }
    }

}
