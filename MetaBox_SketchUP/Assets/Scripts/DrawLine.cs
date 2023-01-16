using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    //===================== referance object ====================
    [SerializeField] Camera mainCam = null;
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [SerializeField] Button revertButton = null;

    //===================== referance questions obj =============
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    //==================== inner variables========================
    GameObject instLine = null;
    GameObject parentObj = null;

    Touch myTouch;
    private Vector3 startPos;
    private Vertex startVertex = null;
    private Vertex tempStartVertex = null;
    private Vertex endVertex = null;

    Vertex collisionVertex = null;
    Clear clearImg = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    bool isMoveEnd = false;

    public int checkClearImgCount = 3;

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
                        ClearImg();
                    }

                    //Debug.Log("lineBackStack.Count :" + lineBackStack.Count);
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
            // ===============
            //objOneDrawCount = lineBackStack.Count;
            //Debug.Log("## 1) objOneDrawCount : " + objOneDrawCount);

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
                            //CountDown();
                        }
                    }
                }
                endVertex = collisionVertex;
            }
        }
        else if (isMoveEnd == true)
        {
            DestroyLine(instLine);
            //CountDown();
            //CountUp();
        }
    }

    void ClearImg()
    {
        if (parentObj.name == objOne.name)
        {
            if (tempStartVertex == endVertex || lineBackStack.Count == 3)
            {
                Debug.Log("완료");
                checkClearImgCount -= 1;
                clearImg.ClearImgOne();
                StartCoroutine(DelayTime(1));
                
                //instLine.transform.parent = null;
            }
        }
        else if (parentObj.name == objTwo.name)
        {
            if (lineBackStack.Count == 10)
            {
                Debug.Log("두번째 완료");
                checkClearImgCount -= 1;
                clearImg.ClearImgObjTwo();

                StartCoroutine(DelayTime(2));
            }
        }
        else if (parentObj.name == objThree.name)
        {
            if (lineBackStack.Count == 8)
            {
                Debug.Log("세번째 완료");
                checkClearImgCount -= 1;
                clearImg.ClearImgObjThree();

                StartCoroutine(DelayTime(3));
            }
        }
        else if (checkClearImgCount == 0)
        {
            clearImg.ClearAll();
        }
    }

    IEnumerator DelayTime(int count)
    {
        yield return new WaitForSeconds(2f);
        LevelManager.Inst.SelectAgain(count);
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