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

    void Awake()
    {
        TryGetComponent<LineRenderer>(out linePos);
        linePosCount = linePos.positionCount;

        nodeData = new CDLinkedList.CDNodeData();
        cdNode = new CDLinkedList.CDNode();
        cdLinkedList = new CDLinkedList.CDLinkedListInst();
    }

    void Start()
    {
        CDLinkedListInsets();
    }

    public CDLinkedList.CDLinkedListInst CDLinkedListInsets()
    {
        Vector3 nodePos;
        int nodeIndex = 0;

        // 포지션 연결
        for (int i = 0; i < linePosCount - 1 ; ++i)
        {
            nodePos = linePos.GetPosition(i);
            nodeData.nodePos = nodePos;
            nodeData.index = nodeIndex;
            nodeData.circlePointObj = circlePointArry[i];

            cdLinkedList.InsertHead(nodeData.nodePos, nodeData.index, nodeData.circlePointObj);
            nodeIndex += 1;
        }

        return cdLinkedList;
    }
}
