using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DrawLine : MonoBehaviour
{
    //===================== referance object ====================
    [SerializeField] Camera mainCam = null;
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [SerializeField] Button revertButton = null;

    //===================== referance questions obj =============
    [SerializeField] QuestionCheck[] objs = null;
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    //===================== ImgDrawCount (int) ==================
    public int objOneDrawCount = 0;
    public int objTwoDrawCount = 0;
    public int objThreeDrawCount = 0;

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

    public int checkClearImgCount = 3;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        //objs = FindObjectsOfType<QuestionCheck>();

        // Revert Button Event
        revertButton.onClick.AddListener(() => OnClickRevertButton());

        // Draw Count Setting
        //objOneDrawCount = objs[0].ObjOneDrawCount;
        //objTwoDrawCount = objs[1].ObjTwoDrawCount;
        //objThreeDrawCount = objs[2].ObjThreeDrawCount;
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

                    //if(instLine != null) 
                    //{
                    //    ClearImg();
                    //}
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
            //CountUp();

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
                    //CountDown();
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

                            // ===============
                            //CountUp();

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
        if (parentObj.name == "IsoscelesOneBrush")
        {
            if (tempStartVertex == endVertex)
            {
                Debug.Log("완료");
                checkClearImgCount -= 1;
                clearImg.ClearImgOne();
            }
        }
        else if(parentObj.name == "StarPolygonOneBrush")
        {
            if (tempStartVertex == endVertex)
            {
                Debug.Log("두번째 완료");
                checkClearImgCount -= 1;
                clearImg.ClearImgTwo();
            }
        }
        else if(parentObj.name == "HouseXOneBrush")
        {
            Debug.Log("세번째 완료");
            checkClearImgCount -= 1;
        }
        else if(checkClearImgCount == 0)
        {
            clearImg.ClearAll();
        }
    }

    void CountDown()
    {
        if (parentObj != null)
        {
            switch (parentObj.name)
            {
                case "IsoscelesOneBrush":
                    {
                        objOneDrawCount -= 1;
                        Debug.Log("objOneDrawCount (-)  :" + objOneDrawCount);

                        if (objOneDrawCount == 0) return;
                    }
                    break;

                case "StarPolygonOneBrush":
                    {
                        objTwoDrawCount -= 1;
                        Debug.Log("objTwoDrawCount (-)  :" + objTwoDrawCount);

                        if (objTwoDrawCount == 0) return;
                    }
                    break;

                case "HouseXOneBrush":
                    {
                        objThreeDrawCount -= 1;
                        Debug.Log("objThreeDrawCount (-) :" + objThreeDrawCount);

                        if (objThreeDrawCount == 0) return;
                    }
                    break;
            }
        }
    }

    void CountUp()
    {
        if (parentObj != null)
        {
            switch (parentObj.name)
            {
                case "IsoscelesOneBrush":
                    {
                        objOneDrawCount += 1;
                        Debug.Log("objOneDrawCount (+) :" + objOneDrawCount);

                        //if (objOneDrawCount <= 3)
                        //{
                        //    Debug.Log(" 1번째 완성");
                        //    clearImg.ClearImgOne();
                        //    checkClearImgCount -= 1;
                        //    Debug.Log("## 1 ) checkClearImgCount" + checkClearImgCount);
                        //}
                    }
                    break;

                case "StarPolygonOneBrush":
                    {
                        objTwoDrawCount += 1;
                        Debug.Log("objTwoDrawCount (+)  :" + objTwoDrawCount);

                        if (objTwoDrawCount <= 10)
                        {
                            Debug.Log(" 2번째 완성");
                            clearImg.ClearImgObjTwo();
                            checkClearImgCount -= 1;
                            Debug.Log("## 2 ) checkClearImgCount" + checkClearImgCount);

                            //ClearAllImg();
                        }
                    }
                    break;

                case "HouseXOneBrush":
                    {
                        objThreeDrawCount += 1;
                        Debug.Log("objThreeDrawCount (+)  :" + objThreeDrawCount);

                        if (objThreeDrawCount <= 8)
                        {
                            Debug.Log(" 3번째 완성");
                            clearImg.ClearImgObjThree();
                            checkClearImgCount -= 1;
                            Debug.Log("## 3 ) checkClearImgCount" + checkClearImgCount);

                            //ClearAllImg();
                        }
                    }
                    break;
            }
        }
    }

    void ClearCheck()
    {

        if (parentObj != null)
        {
            switch (parentObj.name)
            {
                case "IsoscelesOneBrush":
                    {
                        if (objOneDrawCount == 0)
                        {
                            Debug.Log(" 1번째 완성");
                            clearImg.ClearImgOne();
                            checkClearImgCount -= 1;
                            //ClearAllImg();
                        }
                    }
                    break;

                case "StarPolygonOneBrush":
                    {
                        if (objTwoDrawCount == 0)
                        {
                            Debug.Log(" 2번째 완성");
                            clearImg.ClearImgObjTwo();
                            checkClearImgCount -= 1;
                            ClearAllImg();
                        }
                    }
                    break;

                case "HouseXOneBrush":
                    {
                        if (objThreeDrawCount == 0)
                        {
                            Debug.Log(" 3번째 완성");
                            clearImg.ClearImgObjThree();
                            checkClearImgCount -= 1;
                            ClearAllImg();
                        }
                    }
                    break;
            }
        }

    }

    void ClearAllImg()
    {
        if (checkClearImgCount > 0) return;

        if (checkClearImgCount == 0)
        {
            clearImg.ClearAll();
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
