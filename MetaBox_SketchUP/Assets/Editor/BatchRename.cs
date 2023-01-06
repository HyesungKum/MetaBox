using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BatchRename : ScriptableWizard
{
    // 기본 이름
    public string BaseName = "Square";

    // 시작 숫자
    public int StartNumber = 0;

    // 증가치
    public int Increment = 1;

    [MenuItem("Edit/Batch Rename")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
    }

    // 창이 처음 나타날 때 호출된다
    void OnEnable()
    {
        UpdateSelectionHelper();
    }

    // 씬에서 선택 영역이 변경될 때 호출되는 함수
    void OnSelectionChange()
    {
        UpdateSelectionHelper();
    }

    // 선택된 개수를 업데이트한다
    void UpdateSelectionHelper()
    {
        string helpString = "";

        if (Selection.objects != null)
            helpString = "Number of objects selected: " + Selection.objects.Length;

    }

    // 이름 변경 
    void OnWizardCreate()
    {
        // 선택된 것이 없으면 종료한다
        if (Selection.objects == null)
            return;

        // 현재 증가치
        int PostFix = StartNumber;

        // 순회하며 이름을 변경한다
        foreach(Object O in Selection.objects)
        {
            O.name = BaseName + PostFix;
            PostFix += Increment;
        }
    }

}
