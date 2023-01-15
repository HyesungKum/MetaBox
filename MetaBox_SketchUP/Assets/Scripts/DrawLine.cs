using ObjectPoolCP;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class DrawLine : MonoBehaviour
{
    //===================== referrance object====================
    //[SerializeField] Camera mainCam = null;
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [SerializeField] Button revertButton = null;

    //=====================ref vertixes===========================
    [SerializeField] Vertex[] ObjOneVertiexs = null;
    [SerializeField] Vertex[] ObjTwoVertiexs = null;
    [SerializeField] Vertex[] ObjThreeVertiexs = null;

    //====================inner variables=========================
    GameObject instLine = null;
    Touch myTouch;
    private Vector3 startPos;
    public Vertex startVertex = null;
    public Vertex temp = null;
    public Vertex endVertex = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    bool isMoveEnd = false;
    Vertex collisionVertex = null;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        // Revert Button Event
        revertButton.onClick.AddListener(() => OnClickRevertButton());
    }

    void Start()
    {
        verticesCount = FindObjectsOfType<Vertex>().Length;
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);


        switch (myTouch.phase)
        {
            #region Began
            case TouchPhase.Began:
                {
                    isMoveEnd = false;
                    TouchBeganCheck(out collisionVertex);
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    isMoveEnd = false;
                    MoveLineInHit(out collisionVertex);
                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {
                    LineMoveEnd(out collisionVertex);
                }
                break;
                #endregion

        }
    }

    void MoveLineInHit(out Vertex collisionVertex)
    {
        collisionVertex = null;
        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            if (hitInfo.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
            {
                if (instLine != null)
                {
                    for (int i = 0; i < startVertex.GetNodeLength(); i++)
                    {
                        if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                        {
                            collisionVertex.ColorChange(); // Color Changed 
                            checkVertex.Push(collisionVertex);
                            Debug.Log("## checkVertex) 스택에 하나 추가 :");

                            instLine = InstLine();
                            StrethchLine(instLine);
                            lineBackStack.Push(instLine);
                            Debug.Log("## lineBackStack 스택에 하나 추가 :");

                            startVertex = collisionVertex;
                            isMoveEnd = true;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            StrethchLine(instLine);
            isMoveEnd = true;
        }
    }

    void TouchBeganCheck(out Vertex collisionVertex)
    {
        collisionVertex = null;
        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            // Line Inst
            instLine = InstLine();
            lineBackStack.Push(instLine);
            Debug.Log("## lineBackStack 스택에 하나 추가 :");
            LineTransformReset(instLine);

            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                if (startVertex == null)
                {
                    startVertex = collisionVertex;

                    collisionVertex.StartPointColor();
                    checkVertex.Push(collisionVertex);
                    Debug.Log("## checkVertex) 스택에 하나 추가 :");
                }
                else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                {
                    DestroyLine(instLine);
                    Debug.Log("## 1 Pool 삭제");
                }
            }
        }
    }


    void LineMoveEnd(out Vertex collisionVertex)
    {
        collisionVertex = null;

        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            if (startVertex != null)
            {
                if (hitInfo.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                {
                    for (int i = 0; i < startVertex.GetNodeLength(); i++)
                    {
                        if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                        {
                            return;
                        }
                        else if (i == startVertex.GetNodeLength() - 1)
                        {
                            DestroyLine(instLine);
                        }
                    }
                }
            }
        }
        else if (isMoveEnd == true)
        {
            DestroyLine(instLine);
            //Debug.Log("삭제 됨");
        }
    }

    GameObject InstLine()
    {
        GameObject instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        instLine.transform.SetParent(instLineTransform);

        startPos = myTouch.position;
        instLine.transform.position = startPos;
        //Debug.Log("StartPos : " + startPos);

        return instLine;
    }

    void DestroyLine(GameObject line)
    {
        if (line != null)
        {
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
            instLine = lineBackStack.Pop();
            Debug.Log("## lineBackStack 스택에 하나 삭~~제 :");
            Debug.Log("## lineBackStack.Count :" + lineBackStack.Count);
        }
    }

    // Line Transform Resetting
    public void LineTransformReset(GameObject line)
    {
        if (line != null)
        {
            line.transform.localScale = new Vector3(1f, 1f, 0f);
            line.transform.localRotation = Quaternion.identity;
        }
    }

    public void StrethchLine(GameObject line)
    {
        if (line != null)
        {
            line.transform.localScale = new Vector2(Vector3.Distance(myTouch.position, startPos), 1);
            line.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myTouch.position));
        }
    }

    // RayCastHit
    RaycastHit2D RayCheck(Touch myTouch)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }

    public void OnClickRevertButton()
    {
        if (lineBackStack.Count == 0 && checkVertex.Count == 0) return;

        GameObject deleteLine = instLine;
        deleteLine = lineBackStack.Pop();

        Vertex delete = collisionVertex;
        delete = checkVertex.Pop();
        Debug.Log("checkVertex.Count :" + checkVertex.Count);
        delete.BackOriginalColor();
    }

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }
    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}
