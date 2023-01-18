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

    //==================== inner variables=======================
    GameObject instLine = null;
    GameObject parentObj = null;

    // ==== private 변경 꼭 하기 !!!!!
    private Vertex startVertex = null;
    private Vertex collisionVertex = null;
    private Vertex endVertex = null;

    // ==== Stack ====
    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    Touch myTouch;
    private Vector3 startPos;

    private int verticesCount = 0;
    bool isMoveEnd = false;
   
    // ==== private 변경 꼭 하기 !!!!!
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

    void TouchBeganCheck(out Vertex collisionVertex)
    {
        collisionVertex = null;
        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            InstLine();
            LineTransformReset(instLine);

            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                if (startVertex == null)
                {
                    startVertex = collisionVertex;

                    endVertex = collisionVertex;

                    startVertex.StartPointColor();
                    checkVertex.Push(startVertex);
                    //Debug.Log("checkVertex.Count (## (첫 터치 ) Push )) : " + checkVertex.Count);

                    parentObj = startVertex.transform.parent.gameObject;
                    //Debug.Log("collisionVertex.GetNodeName() :" + collisionVertex.GetNodeName());
                    //Debug.Log("startVertex.GetNodeName() :" + startVertex.GetNodeName());
                }
                else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                {
                    DestroyLineObj();
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
                            //Debug.Log("checkVertex.Cout (Move) :" + checkVertex.Count);

                            InstLine();
                            StrethchLine(instLine);

                            startVertex = collisionVertex;
                            endVertex = collisionVertex;
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
                            DestroyLineObj();
                        }
                    }
                }
            }
            ClearImg();
        }
        else if (isMoveEnd == true)
        {
            if (instLine != null)
            {
                DestroyLineObj();
            }
        }
    }

    void SetPlayAgain(bool clearImgOneSet, bool clearImgTwoSet, bool clearImgThreeSet)
    {
        InGamePanelSet.Inst.ClearPanelSet(true);
        InGamePanelSet.Inst.ClearImgSet(clearImgOneSet, clearImgTwoSet, clearImgThreeSet);

        int Childcount = instLineTransform.childCount;

        for (int i = 0; i < Childcount; i++)
        {
            DestroyLineObj();
        }

        lineBackStack.Clear();
        checkVertex.Clear();
        //Debug.Log("스택 클리어 ??" + lineBackStack.Count);

        startVertex = null;
        endVertex = null;

        StartCoroutine(delayTime());
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(2f);

        InGamePanelSet.Inst.ClearPanelSet(false);
        InGamePanelSet.Inst.SelectPanelSetting(true);
    }

    void ClearImg()
    {
        if (instLine != null)
        {
            // ==== 좀더 정확한 클리어 판정 ====
            if (parentObj.name == objOne.name)
            {
                if (lineBackStack.Count == 3)
                {
                    //Debug.Log("## 1 ) 완료");
                    checkClearImgCount -= 1;
                    objOne.gameObject.SetActive(false);
                    SetPlayAgain(true, false, false);
                    StartCoroutine(delayTime());

                    AllClear();
                }
            }
            if (parentObj.name == objTwo.name)
            {
                if (lineBackStack.Count == 10)
                {
                    //Debug.Log("두번째 완료");
                    checkClearImgCount -= 1;

                    objTwo.gameObject.SetActive(false);
                    SetPlayAgain(false, true, false);
                    StartCoroutine(delayTime());

                    AllClear();
                }
            }

            if (lineBackStack.Count == 8)
            {
                //Debug.Log("세번째 완료");
                checkClearImgCount -= 1;

                objThree.gameObject.SetActive(false);
                SetPlayAgain(false, false, true);
                StartCoroutine(delayTime());

                AllClear();
            }
        }
    }

    void AllClear()
    {
        if (checkClearImgCount == 0)
        {
            //Debug.Log("## All Clear) lineBackStack.Count :" + checkClearImgCount);

            objOne.gameObject.SetActive(false);
            objTwo.gameObject.SetActive(false);
            objThree.gameObject.SetActive(false);

            InGamePanelSet.Inst.ClearPanelSet(true);
            InGamePanelSet.Inst.ClearImgSet(true, true, true);

            InGamePanelSet.Inst.WinPanelSet(true);
        }
    }

    void InstLine()
    {
        instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        instLine.transform.SetParent(instLineTransform);

        lineBackStack.Push(instLine);
        //Debug.Log("lineBackStack.Count (## Push )) : " + lineBackStack.Count);

        startPos = myTouch.position;
        instLine.transform.position = startPos;
    }

    void DestroyLineObj()
    {
        if (lineBackStack.Count == 0) return;

        instLine = lineBackStack.Pop();
        //Debug.Log("lineBackStack.Count (## Pop )) : " + lineBackStack.Count);

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
        collisionVertex = null;
    }

    public void OnClickRevertButton()
    {
        if (lineBackStack.Count == 0 && checkVertex.Count == 0)
        {
            startVertex = null;
            endVertex = null;
            return;
        }
        else
        {
            DestroyLineObj();

            startVertex = checkVertex.Pop();
            startVertex.BackOriginalColor();
            //Debug.Log(" checkVertex.Pop() : " + checkVertex.Count);
        }
    }

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

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}