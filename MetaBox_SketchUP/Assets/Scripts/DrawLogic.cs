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
    [SerializeField] Transform[] ObjOne = new Transform[0];
    [SerializeField] Transform[] ObjTwo = new Transform[0];
    [SerializeField] Transform[] ObjThree = new Transform[0];

    Stack<Node> stack = new Stack<Node>();

    void checkPath(Transform pos)
    {
        for(int i = 0; i < ObjOne.Length; i++) 
        {

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
