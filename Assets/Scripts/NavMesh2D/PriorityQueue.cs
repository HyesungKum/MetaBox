using System.Collections.Generic;
using UnityEngine;

public class PriorityNode<T>
{
    //value & priority
    public T Value;
    public float Priority;

    //nextNode
    public PriorityNode<T> next;
}

public class PriorityQueue<T>
{
    PriorityNode<T> head = null;
    PriorityNode<T> cur = null;
    PriorityNode<T> prev = null;

    /// <summary>
    /// create node and Enqueue priority Queue
    /// </summary>
    /// <param name="target"> new Node value </param>
    /// <param name="priority">new Node priority </param>
    public void Enqueue(T target, float priority)
    {
        //create Node
        PriorityNode<T> newNode = new()
        {
            Value = target,
            Priority = priority
        };

        //search and input
        SearchingInput(newNode);
        prev = null;
    }

    /// <summary>
    /// out value and unlinked node
    /// </summary>
    /// <param name="value">reference out value</param>
    /// <returns>return false when queue is null</returns>
    public bool Dequeue(out T value)
    {
        if (head == null)//no more data in queue
        {
            value = default;
            return false;
        }

        //getting value
        value = head.Value;

        if (head.next != null)//head setting to next
        {
            PriorityNode<T> tempNode = head.next;
            head.next = null;
            head = tempNode;
        }
        else//last is out queue be null
        {
            head = null;
        }

        return true;
    }

    /// <summary>
    /// finding correct position to priority queue
    /// </summary>
    /// <param name="targetNode"> target Node </param>
    private void SearchingInput(PriorityNode<T> targetNode)
    {
        //empty queue head is null
        if (head == null)
        {
            head = targetNode;
            return;
        }

        //Set Searching Cursor
        cur = head;

        //comparison target and cur priority
        while (true)
        {
            if (targetNode.Priority < cur.Priority)//finded correct position
            {
                if (prev == null)//input poistion is forefront
                {
                    targetNode.next = cur;
                    head = targetNode;
                    break;
                }
                else//input position is middle somewhere
                {
                    prev.next = targetNode;
                    targetNode.next = cur;
                    break;
                }
            }
            else//no match
            {
                if (cur.next == null)//cur is last behind
                {
                    cur.next = targetNode;
                    break;
                }
                else//cursor change to next
                {
                    prev = cur;
                    cur = cur.next;
                }
            }
        }
    }

    /// <summary>
    /// find matching object
    /// </summary>
    /// <param name="target">target object</param>
    /// <returns> if target in queue return true</returns>
    public bool Contains(T target)
    {
        cur = head;

        while (cur != null)
        {
            if (cur.Value.Equals(target)) return true;
            else cur = cur.next;
        }

        return false;
    }

    /// <summary>
    /// unchain each node and apply null
    /// </summary>
    public void Clear()
    {
        cur = head;
        while (cur != null)
        {
            PriorityNode<T> temp = cur;
            cur = cur.next;

            temp.next = null;
            temp = null;
        }
    }
}
