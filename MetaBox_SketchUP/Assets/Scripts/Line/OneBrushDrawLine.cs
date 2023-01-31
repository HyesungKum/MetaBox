using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OneBrushDrawLine : MonoBehaviour
{
    //===================== referance object ====================
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [Header("[Button]")]
    [SerializeField] Button revertBut = null;

    //==================== inner variables=======================
    private Camera mainCam = null;
    GameObject instLine = null;

    // ==== private 변경 꼭 하기 !!!!!
    [Header("[Vertex Check]")]
    public Vertex startVertex = null;
    public Vertex tempVertex = null;
    public Vertex collisionVertex = null;

    // ==== Stack ====
    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    Touch myTouch;
    private Vector3 startPos;
    private Vector3 touchPos;

    private int verticesCount = 0;
    bool isMovedEnd = false;

    void Awake()
    {
        mainCam = Camera.main; 
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        // Revert Button Event
        revertBut.onClick.AddListener(() => OnClickRevertButton());
    }

    void OnDisable()
    {
        if (instLine == null)
        {
            LineTransformReset(instLine);
        }
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
                    TouchBeganCheck();
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    MoveLineInHit();
                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {
                    LineMoveEnd();
                }
                break;
                #endregion
        }
    }

    void TouchBeganCheck()
    {
        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                InstLine(collisionVertex.transform.position);
                LineTransformReset(instLine);

                if (startVertex == null)
                {
                    startVertex = collisionVertex;
                    tempVertex = startVertex;

                    checkVertex.Push(collisionVertex);
                    startVertex.StartPointColor();
                }
                else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                {
                    Debug.Log(startVertex.GetNodeName());
                    DestroyLineObj();
                }
            }
            isMovedEnd = false;
        }
        else
        {
            isMovedEnd = false;
            return;
        }
    }

    void MoveLineInHit()
    {
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
                            startVertex = collisionVertex;

                            InstLine(collisionVertex.transform.position);
                            StrethchLine(instLine);

                            checkVertex.Push(collisionVertex);
                            collisionVertex.ColorChange();

                            break;
                        }
                    }
                }
            }
            isMovedEnd = true;
        }
        else
        {
            StrethchLine(instLine);
        }
    }

    void LineMoveEnd()
    {
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
                            Debug.Log(" ## 3 삭제 ");
                        }
                    }
                }
            }
        }
        else if (isMovedEnd)
        {

            if (instLine != null)
            {
                DestroyLineObj();
                Debug.Log(" ## 4 삭제 ");
            }
            isMovedEnd = false;
        }
    }

    void InstLine(Vector3 linsPos)
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
        else
            instLine = lineBackStack.Pop();
        //Debug.Log("lineBackStack.Count (## Pop )) : " + lineBackStack.Count);

        LineTransformReset(instLine);
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);

        collisionVertex = null;
    }

    public void OnClickRevertButton()
    {
        if (lineBackStack.Count == 0 && checkVertex.Count == 0)
        {
            return;
        }
        else
        {
            DestroyLineObj();

            Vertex check = checkVertex.Pop();
            //Debug.Log("## 1) check.Count (## Pop )) : " + checkVertex.Count);
            check.BackOriginalColor();

            if (checkVertex.Count == 1)
            {
                check = checkVertex.Pop();
                //Debug.Log("## 2) check.Count (## Pop )) : " + checkVertex.Count);
                tempVertex.BackOriginalColor();

                if (checkVertex.Count == 0)
                {
                    return;
                }
            }

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
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

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