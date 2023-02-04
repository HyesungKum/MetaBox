using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelegateTraversalCDNode(CDLinkedList.CDNode node);

public class CDLinkedList
{
    public class CDNodeData
    {
        public int index;
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
        public void InsertHead(Vector3 nodePos, int index, GameObject cirPointObj)
        {
            CDNodeData newData = new CDNodeData();
            newData.nodePos = nodePos;
            newData.index = index;
            newData.circlePointObj = cirPointObj;
            //Debug.Log("newData.nodePos :  " + newData.nodePos);
            //Debug.Log("newData.index :  " + newData.index);
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
                    //Debug.Log("temp SearchObj 1: " + temp.data.nodePos);
                    //Debug.Log("temp SearchObj 2: " + temp.data.index);
                    //Debug.Log("temp SearchObj 2: " + temp.data.circlePointObj.name);

                    ///Debug.Log("temp check (next) 1: " + temp.next.data.nodePos);
                    ///Debug.Log("temp check (next) 2: " + temp.next.data.index);  
                    ///Debug.Log("temp check (next) 2: " + temp.next.data.circlePointObj.name);
                    ///
                    #endregion
                    //Debug.Log("SearchObj : " + temp.data.nodePos);
                    //Debug.Log("SearchObj : " + temp.data.index);
                    //Debug.Log("SearchObj : " + temp.data.circlePointObj.name);
                    //Debug.Log("SearchObj (next) : " + temp.next.data.nodePos);
                    //Debug.Log("SearchObj (next) : " + temp.next.data.index);
                    //Debug.Log("SearchObj (next) : " + temp.next.data.circlePointObj.name);
                    //Debug.Log("SearchObj (prev) : " + temp.prev.data.nodePos);
                    //Debug.Log("SearchObj (prev) : " + temp.prev.data.index);
                    //Debug.Log("SearchObj (prev) : " + temp.prev.data.circlePointObj.name);
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

        private CDNode Search(Vector3 nodePos, int index)
        {
            CDNodeData nodeData = new CDNodeData();
            nodeData.nodePos = nodePos;
            nodeData.index = index;
            //Debug.Log("## �ֳ� ? newData.nodePos :  " + nodeData.nodePos);
            //Debug.Log("## �ִ� ? newData.index :  " + nodeData.index);
            CDNode temp = head;

            do
            {
                if (temp.data.index.CompareTo(nodeData.index) == 0)
                {
                    #region Debug
                    //Debug.Log("temp check 1: " + temp.data.nodePos);
                    //Debug.Log("temp check 2: " + temp.data.index);
                    //Debug.Log("temp check (next) 1: " + temp.next.data.nodePos);
                    //Debug.Log("temp check (next) 2: " + temp.next.data.index);  
                    //Debug.Log("temp check (prev) 1: " + temp.prev.data.nodePos);
                    //Debug.Log("temp check (prev) 2: " + temp.prev.data.index);
                    #endregion
                    return temp;
                }
                temp = temp.next;
                //Debug.Log("temp check 1: " + temp.next.data.nodePos);
                //Debug.Log("temp check 2: " + temp.next.data.index);
            } while (temp != head);

            return null;
        }

        // �����͸� ���������� Ž���ϱ�
        public void TraversalForWard(GameObject cirPointObj)
        {
            CDNode startNode = SearchObj(cirPointObj);
            #region
            //Debug.Log("temp.Pos 00 ##  " + startNode.data.nodePos);
            //Debug.Log("temp.index 00 ## " + startNode.data.index);
            #endregion
            CDNode temp = startNode.next;
            #region
            //Debug.Log("temp.Pos 0##  " + temp.data.nodePos);
            //Debug.Log("temp.index 0## " + temp.data.index);
            //Debug.Log("temp.index 0## " + temp.data.index);
            #endregion

            while (temp != startNode)
            {
                #region
                //Debug.Log("temp.Pos 1 ## " + temp.data.nodePos);
                //Debug.Log("temp.index 1 ## " + temp.data.index);
                //Debug.Log("temp.index 1 ## " + temp.data.circlePointObj);
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
               //Debug.Log("temp.Pos 1 ## " + temp.data.nodePos);
               //Debug.Log("temp.index 1 ## " + temp.data.index);
               //Debug.Log("temp.index 1 ## " + temp.data.circlePointObj);
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
                //Debug.Log("temp.Pos (ForWard) start -> end ## " + temp.data.index);
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
                Debug.Log("temp.Pos (Backward) start -> end ## " + temp.data.index);
                Debug.Log("temp.Pos (Backward) start -> end ##" + temp.data.circlePointObj);


                temp = temp.prev;
            }
        }

        // �����͸� ���������� Ž���ϱ�
        private void TraversalBackward(CDNode startNode, CDNode endNode, DelegateTraversalCDNode doSomething)
        {
            CDNode temp = startNode;
            while (temp != null)
            {
                doSomething(temp);
                if (temp == endNode)
                    break;
                temp = temp.prev;
            }
        }



        // �����͸� ������ ��� ���������� Ž���ϱ�
        private void TraversalForward(CDNode startNode, CDNode endNode, DelegateTraversalCDNode doSomething)
        {
            CDNode temp = startNode;

            while (temp != null)
            {
                doSomething(temp);

                if (temp == endNode)
                    break;

                temp = temp.next;
            }
        }


        // �����͸� ���������� Ž���ϱ�
        private void TraversalForwarld(DelegateTraversalCDNode doSomething)
        {
            CDNode temp = head.next;

            while (temp != tail)
            {
                doSomething(temp);

                temp = temp.next;
            }
        }

        private void TraversalForward(CDNode startNode, DelegateTraversalCDNode doSomething)
        {
            CDNode temp = startNode.next;
            while (temp != startNode)
            {
                doSomething(temp);
                temp = temp.next;
            }
        }


        // �����͸� ���������� Ž���ϱ�
        private void TraversalBackward(DelegateTraversalCDNode doSomething)
        {
            CDNode temp = tail.prev;
            while (temp != head)
            {
                doSomething(temp);
                temp = temp.prev;
            }
        }

        // �����͸� ���������� Ž���ϱ�
        private void TraversalBackward(CDNode startNode, DelegateTraversalCDNode doSomething)
        {
            CDNode temp = startNode.prev;
            while (temp != startNode)
            {
                doSomething(temp);
                temp = temp.prev;
            }
        }


    }
}
