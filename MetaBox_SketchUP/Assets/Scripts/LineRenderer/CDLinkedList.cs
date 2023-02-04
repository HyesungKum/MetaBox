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

        // Head 다음에 추가
        public void InsertHead(Vector3 nodePos, int index, GameObject cirPointObj)
        {
            CDNodeData newData = new CDNodeData();
            newData.nodePos = nodePos;
            newData.index = index;
            newData.circlePointObj = cirPointObj;
            //Debug.Log("newData.nodePos :  " + newData.nodePos);
            //Debug.Log("newData.index :  " + newData.index);
            //Debug.Log("newData.index :  " + newData.circlePointObj.name);

            // 새 데이터를 담는 새 노드
            CDNode newNode = new CDNode();
            newNode.data = newData;

            // 데이터 첫 추가
            if (head == null)
            {
                newNode.prev = newNode; // 원형
                newNode.next = newNode; // 원형
                head = tail = newNode;
            }
            else
            {
                // 새 노드의 연결관계
                newNode.prev = head.prev;
                newNode.next = head;

                // 새 노드가 추가되었으니 head는 새 노드를 가리켜야 한다.
                head.prev = newNode;
                head = newNode;
                tail.next = newNode;
            }
        }

        // tail 앞에 추가
        public void InsertTail(CDNodeData newData)
        {
            // 새 데이터를 담는 새 노드
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
                // 새 노드의 연결관계
                newNode.prev = tail;
                newNode.next = tail.next;

                // 새 노드가 추가되었으니 tail은 새 노드를 가리켜야한다.
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
            //Debug.Log("## 있나 ? newData.nodePos :  " + nodeData.nodePos);
            //Debug.Log("## 있니 ? newData.index :  " + nodeData.index);
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

        // 데이터를 순방향으로 탐색하기
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

        // 데이터를 지정된 노드 순방향으로 탐색하기
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

        // 데이터를 역방향으로 탐색하기
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

        // 데이터를 역방향으로 탐색하기
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



        // 데이터를 지정된 노드 순방향으로 탐색하기
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


        // 데이터를 순방향으로 탐색하기
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


        // 데이터를 역방향으로 탐색하기
        private void TraversalBackward(DelegateTraversalCDNode doSomething)
        {
            CDNode temp = tail.prev;
            while (temp != head)
            {
                doSomething(temp);
                temp = temp.prev;
            }
        }

        // 데이터를 역방향으로 탐색하기
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
