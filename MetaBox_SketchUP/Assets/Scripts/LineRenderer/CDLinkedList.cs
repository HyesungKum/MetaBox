using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelegateTraversalCDNode(CDLinkedList.CDNode node);

public class CDLinkedList
{
    public class CDNodeData
    {
        public Vector3 nodePos;
        public GameObject circlePointObj;
    }

    public class CDNode
    {
        public CDNodeData data = null;

        public CDNode prev = null;
        public CDNode next = null;
    }

    public class CDLinkedListInst
    {
        CDNode head = null;
        CDNode tail = null;

        // Head ������ �߰�
        public void InsertHead(Vector3 nodePos, GameObject cirPointObj)
        {
            CDNodeData newData = new CDNodeData();
            newData.nodePos = nodePos;
            newData.circlePointObj = cirPointObj;
            //Debug.Log("newData.nodePos :  " + newData.nodePos);
            //Debug.Log("newData.index :  " + newData.circlePointObj.name);

            // �� �����͸� ��� �� ���
            CDNode newNode = new CDNode();
            newNode.data = newData;

            // ������ ù �߰�
            if (head == null)
            {
                newNode.prev = newNode; // ����
                newNode.next = newNode; // ����
                head = tail = newNode;
            }
            else
            {
                // �� ����� �������
                newNode.prev = head.prev;
                newNode.next = head;

                // �� ��尡 �߰��Ǿ����� head�� �� ��带 �����Ѿ� �Ѵ�.
                head.prev = newNode;
                head = newNode;
                tail.next = newNode;
            }
        }

        // tail �տ� �߰�
        public void InsertTail(CDNodeData newData)
        {
            // �� �����͸� ��� �� ���
            CDNode newNode = new CDNode();
            newNode.data = newData;

            if (tail == null)
            {
                newNode.prev = newNode;
                newNode.next = newNode;
                head = tail = newNode;
            }
            else
            {
                // �� ����� �������
                newNode.prev = tail;
                newNode.next = tail.next;

                // �� ��尡 �߰��Ǿ����� tail�� �� ��带 �����Ѿ��Ѵ�.
                tail.next = newNode;
                tail = newNode;
                head.prev = newNode;
            }
        }

        public CDNode SearchObj(GameObject cirPointObj)
        {
            CDNodeData nodeData = new CDNodeData();
            nodeData.circlePointObj = cirPointObj;

            CDNode temp = head;

            do
            {
                if (temp.data.circlePointObj.name.CompareTo(nodeData.circlePointObj.name) == 0)
                {
                    #region Debug
                    //Debug.Log("SearchObj : " + temp.data.nodePos);
                    //Debug.Log("SearchObj : " + temp.data.circlePointObj.name);
                    //Debug.Log("SearchObj (next) : " + temp.next.data.nodePos);
                    //Debug.Log("SearchObj (next) : " + temp.next.data.circlePointObj.name);
                    //Debug.Log("SearchObj (prev) : " + temp.prev.data.nodePos);
                    //Debug.Log("SearchObj (prev) : " + temp.prev.data.circlePointObj.name);
                    #endregion
                    return temp;
                }

                temp = temp.next;

                #region
                //Debug.Log("temp check 1: " + temp.next.data.nodePos);
                //Debug.Log("temp check 2: " + temp.next.data.index);
                //Debug.Log("temp check 2: " + temp.next.data.circlePointObj.name);
                #endregion

            } while (temp != head);

            return null;
        }

        // �����͸� ���������� Ž���ϱ�
        public void TraversalForWard(GameObject cirPointObj)
        {
            CDNode startNode = SearchObj(cirPointObj);
            #region
            Debug.Log("SearchObj  ##  " + startNode.data.nodePos);
            Debug.Log("SearchObj 00 ## " + startNode.data.circlePointObj);
            #endregion
            CDNode temp = startNode.next;
            #region
            //Debug.Log("temp.Pos ##  " + temp.data.nodePos);
            //Debug.Log("temp.circlePointObj ## " + temp.data.circlePointObj);
            #endregion

            while (temp != startNode)
            {
                #region
                Debug.Log("temp.Pos  ## " + temp.data.nodePos);
                Debug.Log("temp.index ## " + temp.data.circlePointObj);
                #endregion
                temp = temp.next;
            }
        }

        public void TraversalBackWard(GameObject cirPointObj)
        {
            CDNode startNode = SearchObj(cirPointObj);

            #region
            //Debug.Log("temp.Pos 00 ##  " + startNode.data.nodePos);
            //Debug.Log("temp.index 00 ## " + startNode.data.index);
            #endregion
            CDNode temp = startNode.prev;
            #region
            //Debug.Log("temp.Pos 0##  " + temp.data.nodePos);
            //Debug.Log("temp.index 0## " + temp.data.index);
            #endregion

            while (temp != startNode)
            {
                #region
                Debug.Log("temp.Pos 1 ## " + temp.data.nodePos);
                Debug.Log("temp.index 1 ## " + temp.data.circlePointObj);
                #endregion
                temp = temp.prev;
            }
        }

        // �����͸� ������ ��� ���������� Ž���ϱ�
        public void TraversalForward(GameObject startNodeObj, GameObject endNodeObj)
        {
            CDNode startNode = SearchObj(startNodeObj);
            CDNode endNode = SearchObj(endNodeObj);

            CDNode temp = startNode;

            while (temp != null)
            {
                if (temp == endNode)
                    break;

                //Debug.Log("temp.Pos (ForWard) start -> end ## " + temp.data.nodePos);
                //Debug.Log("temp.Pos (ForWard) start -> end ##" + temp.data.circlePointObj);

                temp = temp.next;
            }

        }

        // �����͸� ���������� Ž���ϱ�
        public void TraversalBackward(GameObject startNodeObj, GameObject endNodeObj)
        {
            CDNode startNode = SearchObj(startNodeObj);
            CDNode endNode = SearchObj(endNodeObj);

            CDNode temp = startNode;
            while (temp != null)
            {
                if (temp == endNode)
                    break;

                Debug.Log("temp.Pos (Backward) start -> end ## " + temp.data.nodePos);
                Debug.Log("temp.Pos (Backward) start -> end ##" + temp.data.circlePointObj);
                temp = temp.prev;
            }
        }
    }
}
