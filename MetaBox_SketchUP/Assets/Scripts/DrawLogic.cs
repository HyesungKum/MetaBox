using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Node start = null;

    public Node next = null;
    public Node end = null;

}

public class DrawLogic : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;

    [SerializeField] RectTransform objOneLinePos = null;
    [SerializeField] RectTransform objTwoLinePos = null;
    [SerializeField] RectTransform objThreeLinePos = null;

    Stack<Node> stack = new Stack<Node>();

    int objOneFinishCount = 5;
    int objTwoFinishCount = 5;
    int objThreeFinishCount = 9;


    public void InstObjOneLine()
    {
        GameObject line = null;
        if (objOneFinishCount > 0)
        {
            line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, objOneLinePos);
        }
        else if (objOneFinishCount <= 0)
        {
            return;
        }
    }

    void InstLine()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

}
