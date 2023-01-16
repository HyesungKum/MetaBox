using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    //===================== referance object ====================
    [SerializeField] Camera mainCam = null;
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [SerializeField] Button revertButton = null;

    //===================== referance vertixes ==================
    //[SerializeField] Vertex[] ObjOneVertiexs = null;
    // [SerializeField] Vertex[] ObjTwoVertiexs = null;
    // [SerializeField] Vertex[] ObjThreeVertiexs = null;

    //===================== referance questions obj =============
    [SerializeField] QuestionCheck[] objs = null;
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    //====================inner variables========================
    GameObject instLine = null;
    GameObject parentObj = null;

    Touch myTouch;
    private Vector3 startPos;
    private Vertex startVertex = null;
    // ===== private 로 추구 꼭 변경 하기
    public Vertex tempStartVertex = null;
    public Vertex endVertex = null;

    Vertex collisionVertex = null;
    Clear clearImg = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    bool isMoveEnd = false;

    int checkClearImgCount = 3;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        //objs = FindObjectsOfType<QuestionCheck>();

        // Revert Button Event
        revertButton.onClick.AddListener(() => OnClickRevertButton());
    }

    void Start()
    {
        verticesCount = FindObjectsOfType<Vertex>().Length;
        clearImg = FindObjectOfType<Clear>();
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
                    if (instLine != null)
                    {
                        ClearCheck();
                    }
                }
                break;
                #endregion

        }
    }

    void TouchBeganCheck(out Vertex collisionVertex)
    {
        collisionVertex = null;
        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            instLine = InstLine();
            lineBackStack.Push(instLine);
            //Debug.Log("## lineBackStack 스택에 하나 추가 :");
            LineTransformReset(instLine);

            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                if (startVertex == null)
                {
                    tempStartVertex = collisionVertex;
                    startVertex = collisionVertex;

                    collisionVertex.StartPointColor();
                    checkVertex.Push(collisionVertex);
                    //Debug.Log("## checkVertex) 스택에 하나 추가 :" + checkVertex.Count);
                }
                else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                {
                    DestroyLine(instLine);
                }
            }
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
                            //Debug.Log("## checkVertex) 스택에 하나 추가 :" + checkVertex.Count);

                            instLine = InstLine();
                            StrethchLine(instLine);

                            lineBackStack.Push(instLine);
                            //Debug.Log("## lineBackStack 스택에 하나 추가 :");
                            startVertex = collisionVertex;

                            parentObj = startVertex.transform.parent.gameObject;

                            for (int j = 0; j < objs.Length; ++j)
                            {
                                objs[j].ObjName(parentObj.name);
                            }

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
                endVertex = collisionVertex;
            }
        }
        else if (isMoveEnd == true)
        {
            DestroyLine(instLine);
        }
    }

    void CountDown()
    {
        if (parentObj != null)
        {
            if (parentObj.name.Equals("Obj1"))
            {
                objs[0].ObjOneDrawCount -= 1;
            }
            else if (parentObj.name.Equals("Obj2"))
            {
                objs[1].ObjTwoDrawCount -= 1;
            }
            else if (parentObj.name.Equals("Obj3"))
            {
                objs[1].ObjThreeDrawCount -= 1;
            }
        }
    }

    void CountUp()
    {
        if (parentObj != null)
        {
            if (parentObj.name.Equals("Obj1"))
            {
                objs[0].ObjOneDrawCount += 1;
            }
            else if (parentObj.name.Equals("Obj2"))
            {
                objs[1].ObjTwoDrawCount += 1;
            }
            else if (parentObj.name.Equals("Obj3"))
            {
                objs[1].ObjThreeDrawCount += 1;
            }
        }
    }

    void ClearCheck()
    {
        if (parentObj != null)
        {
            if (parentObj.name.Equals("Obj1"))
            {
                //if (tempStartVertex.name == endVertex.name)
                if (objs[0].ObjOneDrawCount == 0)
                {
                    clearImg.ClearImgOne();
                    Debug.Log("## ClearImg 보여줘라 !!");
                    checkClearImgCount -= 1;
                }
            }
            else if (parentObj.name.Equals("Obj2"))
            {
                if (objs[1].ObjTwoDrawCount == 0)
                {
                    Debug.Log("Obj2 Clear");
                    clearImg.ClearImgObjTwo();
                    checkClearImgCount -= 1;
                }
            }
            else if (parentObj.name.Equals("Obj3"))
            {
                if (objs[2].ObjThreeDrawCount == 0)
                {
                    Debug.Log("Obj3 Clear");
                    clearImg.ClearImgObjThree();
                    checkClearImgCount -= 1;
                }
            }
            else if (checkClearImgCount == 0)
            {
                Debug.Log("Obj Clear All");
                clearImg.ClearAll();
            }
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
            if (lineBackStack.Count == 0) return;
            instLine = lineBackStack.Pop();
            //Debug.Log("## lineBackStack.Count :" + lineBackStack.Count);
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

    RaycastHit2D RayCheck(Touch myTouch)
    {
        Vector3 touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }

    public void OnClickRevertButton()
    {
        if (lineBackStack.Count == 0 && checkVertex.Count == 0) return;

        //lineBackStack.Pop();
        DestroyLine(instLine);

        Vertex delete = collisionVertex;
        delete = checkVertex.Pop();
        //Debug.Log("checkVertex.Count :" + checkVertex.Count);
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
