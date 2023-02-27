using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh2Agent : MonoBehaviour
{
    //=========================================Nav Mesh Data=============================================
    private NavMesh2D navMesh2D = null;

    //==============================================Node=================================================
    private NavNode StartNode = null;
    private NavNode EndNode = null;

    private NavNode pathNode = null;

    //======================================path finding Queue===========================================
    PriorityQueue<NavNode> openQueue = new();
    List<NavNode> closedList = new();

    Stack<Vector3> pathStack = new();

    //detail path
    Vector3 detailPos = Vector3.zero;

    //===========================================State Flag==============================================
    [SerializeField] float agentSpeed = 1f;
    public bool IsMove = false;
    public bool IsLookRight = true;
    [SerializeField] bool MoveOrder = false;
    [field: SerializeField] public bool OnField { get; private set; }

    //============================================delegate=============================================== 
    public delegate void CallBack();

    private CallBack nodeClear = null;
    public CallBack moveEvent = null;
    public CallBack stopEvent = null;

    private void CallNodeClear() => nodeClear?.Invoke();
    private void CallMoveEvent() => moveEvent?.Invoke();
    private void CallStopEvent() => stopEvent?.Invoke();

    private void Awake()
    {
        //find nav mesh object
        navMesh2D = FindObjectOfType<NavMesh2D>();
        if (navMesh2D == null) Debug.Log("## NavMeshAgent Error : null Reference exception, plz ref nav mesh 2d");

        //delegate chain
        navMesh2D.connectFinish += Initializing;
        EventReceiver.gameIn += StopAgentMove;
    }

    private void OnEnable()
    {
        if (navMesh2D != null) Initializing();
    }

    private void OnDisable()
    {
        navMesh2D.connectFinish -= Initializing;
        EventReceiver.gameIn -= StopAgentMove;
    }

    //========================================Find Mesh Data & Position Init========================================
    private void Initializing()
    {
        //set position for coll bound
        Vector3 ThisPos = this.transform.position;
        this.transform.position = new Vector3(ThisPos.x, ThisPos.y, navMesh2D.transform.position.z);

        //check this object on mesh
        StartCoroutine(nameof(FieldCheck));
    }

    //==============================================Check Object On Mesh============================================
    IEnumerator FieldCheck()
    {
        while (this.gameObject.activeSelf)
        {
            OnField = navMesh2D.CheckPosOnMesh(this.transform.position);
            yield return null;
        }
    }

    //========================================Set Agent Moving Target Position======================================
    public void SetDestination(Vector3 position)
    {
        //targetting detail position
        detailPos = new Vector3(position.x, position.y, this.transform.position.z);

        //initailizing
        IsMove = true;
        CallMoveEvent();

        StartNode = null;
        EndNode = null;
        pathNode = null;

        openQueue.Clear();
        closedList = new();
        pathStack = new();

        StopCoroutine(nameof(AgentNodeMove));
        StopCoroutine(nameof(AgentDetailMove));
        MoveOrder = true;

        //clean value
        CallNodeClear();
        nodeClear = null;

        //set start, end
        StartNode = navMesh2D.PosToNode(this.transform.position);
        EndNode = navMesh2D.PosToNode(position);

        if (StartNode == null || EndNode == null || EndNode.obstacle)
        {
            #if UNITY_EDITOR
            Debug.Log("##NavMesh2Agent Error : cannot found path");
            #endif
            IsMove = false;
            CallStopEvent();
            return;
        }

        //a star path finding
        AstarPathFinding();
    }

    //=================================================StopAgentMoving==================================================
    private void StopAgentMove(string game)
    {
        StopCoroutine(nameof(AgentNodeMove));
        StopCoroutine(nameof(AgentDetailMove));
        IsMove = false;
        CallStopEvent();
    }

    //===================================================PathFinding====================================================
    private void AstarPathFinding()
    {
        //set cur Node for calculating huristic score
        NavNode curNode = StartNode;

        //obstacle place hsocre max -> return 
        //if (curNode.obstacle) return;

        //set score
        curNode.GScore = 0;
        curNode.HScore = EndNode.CalH(curNode);
        nodeClear += curNode.Clear;

        //finding path
        while (true)
        {
            if (curNode == null) break;

            //input curNode to checkedQueue
            closedList.Add(curNode);

            //check neibhor node
            if (CheckNeibhorNode(ref curNode, ref curNode.EastNode     , false)) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.EastSouthNode, true )) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.SouthNode    , false)) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.SouthWestNode, true )) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.WestNode     , false)) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.WestNorthNode, true )) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.NorthNode    , false)) break;
            if (CheckNeibhorNode(ref curNode, ref curNode.NorthEastNode, true )) break;

            //curNode Change to monitoring Node
            while(curNode != null)
            {
                openQueue.Dequeue(out curNode);

                //handling overlap node
                if (!closedList.Contains(curNode)) break;
            }
        }

        //move in Node
        if (pathNode == null)
        {
            StartCoroutine(nameof(AgentDetailMove));
            return;
        }

        //setting Path
        while(pathNode.parentNode != null)
        {
            pathStack.Push(pathNode.position);
            pathNode = pathNode.parentNode;
        }

        //move
        StartCoroutine(nameof(AgentNodeMove));
    }
    private bool CheckNeibhorNode(ref NavNode curNode, ref NavNode tarNode, bool diagonal)
    {
        //target node is null when edge
        if (tarNode == null) return false;

        //target node is obstcle 
        if (tarNode.obstacle) return false;

        //target node is already checked
        if (closedList.Contains(tarNode)) return false;

        //set gScore
        int tempG = 0;
        if (diagonal) tempG = curNode.GScore + 14; 
        else tempG = curNode.GScore + 10;

        if (openQueue.Contains(tarNode))//targetNode is already in monitoring
        {
            if (tempG < tarNode.GScore)//Low Gscore
            {
                //renewal Gscore and new parent
                tarNode.GScore = tempG;
                tarNode.parentNode = curNode;

                //clean chain
                nodeClear += tarNode.Clear;

                //input openQueue
                openQueue.Enqueue(tarNode, tarNode.FScore);
            }
        }
        else//targetNode is not already in monitoring
        {
            if (tarNode.parentNode == null)//dont have parent 
            {
                //calculating gscore & hsocre set parent
                tarNode.GScore = tempG;
                tarNode.HScore = EndNode.CalH(tarNode);
                tarNode.parentNode = curNode;

                //clean chain
                nodeClear += tarNode.Clear;

                //arrived target
                if (tarNode.HScore == 0)
                {
                    pathNode = tarNode;
                    return true;
                }

                //input monitoring
                openQueue.Enqueue(tarNode, tarNode.FScore);
            }
        }

        return false;
    }

    //===============================================Agent Object Moving================================================
    /// <summary>
    /// agent move node to node
    /// </summary>
    /// <returns></returns>
    private IEnumerator AgentNodeMove()
    {
        MoveOrder = false;

        if (pathStack.Count <= 1)
        {
            IsMove = false;
            CallStopEvent();
            yield break;
        }

        Vector3 targetPos = pathStack.Pop();
        Vector3 dir = targetPos - this.transform.position;

        //node move
        while (true)
        {
            if (dir.normalized.x > 0) IsLookRight = true;
            else IsLookRight = false;

            //new moving order
            if (MoveOrder == true) yield break;

            this.transform.position += agentSpeed * Time.deltaTime * dir.normalized;

            //move position
            if (Vector3.Distance(this.transform.position, targetPos) < 0.2f)
            {
                if (pathStack.Count == 0) break;
                targetPos = pathStack.Pop();
                dir = targetPos - this.transform.position;
            }

            yield return null;
        }

        yield return AgentDetailMove();
    }

    /// <summary>
    /// agent move detail moving
    /// </summary>
    /// <returns></returns>
    private IEnumerator AgentDetailMove()
    {
        //looking position
        Vector3 dir = detailPos - this.transform.position;
        if (dir.normalized.x > 0) IsLookRight = true;
        else IsLookRight = false;

        //detail move
        while (true)
        {
            if (Vector3.Distance(this.transform.position, detailPos) < 0.2f)
            {
                IsMove = false;
                CallStopEvent();
                yield break;
            }

            this.transform.position += agentSpeed * Time.deltaTime * dir.normalized;

            yield return null;
        }
    }
}
