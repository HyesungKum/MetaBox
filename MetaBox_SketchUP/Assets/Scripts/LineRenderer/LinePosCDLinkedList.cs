using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class LinePosCDLinkedList : MonoBehaviour
{
    [SerializeField] public GameObject[] circlePointArry = null;

    LineRenderer linePos = null;

    public CDLinkedList.CDLinkedListInst cdLinkedList = null;
    public CDLinkedList.CDNodeData nodeData = null;
    public CDLinkedList.CDNode cdNode = null;
    int linePosCount;
    int arryLength;
    void Awake()
    {
        TryGetComponent<LineRenderer>(out linePos);

        nodeData = new CDLinkedList.CDNodeData();
        cdNode = new CDLinkedList.CDNode();
        cdLinkedList = new CDLinkedList.CDLinkedListInst();
        linePosCount = linePos.positionCount;
        arryLength = circlePointArry.Length;

        if (arryLength != linePosCount) return;
        else
            CDLinkedListInsets();
    }

    public CDLinkedList.CDLinkedListInst CDLinkedListInsets()
    {
        Vector3 nodePos;

        // 포지션 연결
        for (int i = 0; i < linePosCount; ++i)
        {
            nodePos = linePos.GetPosition(i);
            nodeData.nodePos = nodePos;
            nodeData.circlePointObj = circlePointArry[i];

            cdLinkedList.InsertHead(nodeData.nodePos, nodeData.circlePointObj);
        }

        return cdLinkedList;
    }
}