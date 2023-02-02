using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinePosCDLinkedList : MonoBehaviour
{
    //[SerializeField] List<GameObject> circlePointList = null;
    LineRenderer linePos = null;

    CDLinkedList.CDLinkedListInst cdLinkedList = null;
    CDLinkedList.CDNodeData nodeData = null;
    int linePosCount;

    DelegateTraversalCDNode delegateTraversal;

    void Awake()
    {
        TryGetComponent<LineRenderer>(out linePos);
        linePosCount = linePos.positionCount;
        //circlePointObj = new List<GameObject>();
        nodeData = new CDLinkedList.CDNodeData();
        cdLinkedList = new CDLinkedList.CDLinkedListInst();
    }

    void Start()
    {
        Vector3 nodePos;
        int nodeIndex = 0;

        // 포지션 연결
        for (int i = 0; i < linePosCount; ++i)
        {
            nodePos = linePos.GetPosition(i);
            nodeData.nodePos = nodePos;
            nodeData.index = nodeIndex;

            cdLinkedList.InsertHead(nodeData.nodePos, nodeData.index);
            nodeIndex += 1;
        }

        //cdLinkedList.Search(new Vector3(-0.17f, 3.05f, -10f), 0);
        //cdLinkedList.Search(new Vector3(-1.27f, 2.60f, -10f), 0);
        //cdLinkedList.Search(new Vector3(-1.27f, 2.60f, -10f), 0);
        //cdLinkedList.Search(new Vector3(-2.16f, 1.19f, -10f), 2);
        //cdLinkedList.TraversalForWard(new Vector3(-2.16f, 1.19f, -10f), 2);

        cdLinkedList.TraversalBackWard(new Vector3(-2.16f, 1.19f, -10f), 2);
    }

}
