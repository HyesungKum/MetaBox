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
    // ==== private 변경 꼭 하기 !!!!!
    public Vertex startVertex = null;
    public Vertex collisionVertex = null;

    Stack<GameObject> lineBackStack = null;
    Stack<GameObject> ObjTwoBackStack = null;
    Stack<GameObject> ObjThreeBackStack = null;

    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    bool isMoveEnd = false;

    public int checkClearImgCount = 3;
    public int checklineStackCount = 0;

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
            instLine = InstLine();

            LineTransformReset(instLine);

            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                if (startVertex == null)
                {
                    startVertex = collisionVertex;

                    collisionVertex.StartPointColor();
                    checkVertex.Push(collisionVertex);
                    parentObj = startVertex.transform.parent.gameObject;
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

                            instLine = InstLine();
                            StrethchLine(instLine);

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
            ClearImg();
        }
        else if (isMoveEnd == true)
        {
            if (instLine != null)
            {
                DestroyLine(instLine);
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
            DestroyLine(instLineTransform.GetChild(i).gameObject);
        }

        lineBackStack.Clear();
        Debug.Log("스택 클리어 ??" + lineBackStack.Count);

        startVertex = null;
        //collisionVertex = null;

        StartCoroutine(delayTime());
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(2f);

        InGamePanelSet.Inst.ClearPanelSet(false);
        InGamePanelSet.Inst.SelectPanelSet(true);
    }

    void ClearImg()
    {
        if (instLine != null)
        {
            if (lineBackStack.Count == 3)
            {
                Debug.Log("## 1 ) 완료");
                checkClearImgCount -= 1;

                objOne.gameObject.SetActive(false);
                SetPlayAgain(true, false, false);
                StartCoroutine(delayTime());

                AllClear();

                return;
            }
            if (lineBackStack.Count == 10)
            {
                Debug.Log("두번째 완료");
                checkClearImgCount -= 1;

                objTwo.gameObject.SetActive(false);
                SetPlayAgain(false, true, false);
                StartCoroutine(delayTime());

                AllClear();

                return;
            }
            if (lineBackStack.Count == 8)
            {
                Debug.Log("세번째 완료");
                checkClearImgCount -= 1;

                objThree.gameObject.SetActive(false);
                SetPlayAgain(false, false, true);
                StartCoroutine(delayTime());

                AllClear();

                return;
            }
        }
    }

    void AllClear()
    {
        if (checkClearImgCount == 0)
        {
            Debug.Log("## All Clear) lineBackStack.Count :" + checkClearImgCount);

            objOne.gameObject.SetActive(false);
            objTwo.gameObject.SetActive(false);
            objThree.gameObject.SetActive(false);

            InGamePanelSet.Inst.ClearPanelSet(true);
            InGamePanelSet.Inst.ClearImgSet(true, true, true);

            InGamePanelSet.Inst.WinPanelSet(true);
        }
    }

    GameObject InstLine()
    {
        GameObject instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        instLine.transform.SetParent(instLineTransform);

        lineBackStack.Push(instLine);
        Debug.Log("lineBackStack.Count (## Push )) : " + lineBackStack.Count);

        startPos = myTouch.position;
        instLine.transform.position = startPos;

        return instLine;
    }

    void DestroyLine(GameObject line)
    {
        if (line != null)
        {
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);

            if (lineBackStack.Count == 0) return;
            instLine = lineBackStack.Pop();
            lineBackStackPopCount(lineBackStack.Count);
            Debug.Log("lineBackStack.Count (## Pop )) : " + lineBackStack.Count);
        }
    }

    void lineBackStackPopCount(int Count)
    {
        checklineStackCount = Count;
        Debug.Log("check 리턴 값 :" + checklineStackCount);
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