using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawLineCurve : MonoBehaviour
{
    [Header("[Draw Line Prefab]")]
    [SerializeField] private GameObject linePrefab = null;

    [Header("[Object Prefabs]")]
    [SerializeField] GameObject checkObj = null;
    [SerializeField] Button revertBut = null;
    [SerializeField] Transform lineClonePos = null;

    [Header("[Line Change]")]
    [SerializeField] LineColorChanged colorPanel = null;
    [SerializeField] LineSizeChange lineSizeChange = null;

    [Header("[Clear Draw]")]
    [SerializeField] GameObject choiceWordPanel = null;
    [SerializeField] GameObject animation = null;
    [SerializeField] int clearCount;

    [Header("[Other]")]
    private Camera mainCam;
    private GameObject currentLine;
    public GameObject startNodeObj = null;
    public GameObject endNodeObj = null;

    Touch myTouch;
    private Vector3 touchPos;
    public Vector3 startPos;
    Vector3 startPosCheck;
    LineRender linerender = null;

    LinePosCDLinkedList circleObj = null;

    int circleObjCount;
    int nodePosCount;
    int linePosCount;

    GameObject prevObj = null;
    GameObject nextObj = null;

    Stack<GameObject> lineStack;
    ClearAnimation clearAnimaition = null;

    public Color startColor;

    float lineSizeValue;

    void Awake()
    {
        mainCam = Camera.main;
        lineStack = new Stack<GameObject>();
    }

    void Start()
    {
        animation.TryGetComponent<ClearAnimation>(out clearAnimaition);
        checkObj.TryGetComponent<LinePosCDLinkedList>(out circleObj);
        circleObjCount = circleObj.circlePointArry.Length;
        revertBut.onClick.AddListener(delegate { OnClickRevertBut(); });
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);

        switch (myTouch.phase)
        {
            case TouchPhase.Began:
                {
                    TouchBegan();
                }
                break;

            case TouchPhase.Moved:
                {
                    TouchMove();
                }
                break;

            case TouchPhase.Ended:
                {
                    MoveEnd();
                }
                break;
        }
    }

    void TouchBegan()
    {
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            if (hitInfo.collider.name == "Collider")
            {
                //Debug.Log("콜라이더 부디쳤니?") ;
                return;
            }
            if (currentLine == null)
            {
                startNodeObj = hitInfo.transform.gameObject; // 클릭한 걸 받아오기
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj);
                InstLine(); // 라인 만들고 
                //Debug.Log("circleObj.cdNode : " + circleObj.cdNode);

                startNodeObj = circleObj.cdNode.data.circlePointObj;
                prevObj = circleObj.cdNode.prev.data.circlePointObj;
                nextObj = circleObj.cdNode.next.data.circlePointObj;

                startPos = startNodeObj.transform.position;
                linerender.SetPosition(0, startPos);
                linerender.SetPosition(1, startPos);
            }
            else
            {
                InstLine();

                startNodeObj = hitInfo.transform.gameObject;
                linerender.SetPosition(0, startNodeObj.transform.position);
                linerender.SetPosition(1, startNodeObj.transform.position);


                if (startNodeObj == endNodeObj)
                {
                    circleObj.cdNode = circleObj.cdLinkedList.SearchObj(endNodeObj);
                    startNodeObj = circleObj.cdNode.data.circlePointObj;

                    prevObj = circleObj.cdNode.prev.data.circlePointObj;
                    nextObj = circleObj.cdNode.next.data.circlePointObj;

                    startPos = startNodeObj.transform.position;
                    linerender.SetPosition(0, startPos);
                    linerender.SetPosition(1, startPos);
                }
            }
        }
    }

    void TouchMove()
    {
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            linerender.SetCurvePosition(touchPos);
            GameObject hitObjCheck = hitInfo.transform.gameObject;
            Vector3 chekcPos = hitObjCheck.transform.position;

            if (hitObjCheck == prevObj)
            {
                linerender.SetPosition(1, chekcPos);

                //SoundManager.Inst.ButtonSFXPlay(); //// 효과음
                endNodeObj = prevObj;
                //Debug.Log("(prevObj) endNodeObj  : " + endNodeObj);
            }
            else if (hitObjCheck == nextObj)
            {
                linerender.SetPosition(1, chekcPos);
                //SoundManager.Inst.ButtonSFXPlay(); //// 효과음
                endNodeObj = nextObj;
                //Debug.Log("(nextObj) endNodeObj  : " + nextObj);

            }
            else
            {
                //linerender.SetPosition(1, chekcPos);
                endNodeObj = hitObjCheck;
            }
        }
    }

    void MoveEnd()
    {
        //Debug.Log("stackCount : " + lineStack.Count);

        //RaycastHit2D hitInfo = RayCheck();

        //if (hitInfo)
        //{
        //    GameObject hitObjCheck = hitInfo.transform.gameObject;

        //    if (hitObjCheck == prevObj)
        //    {
        //        lineStack.Push(currentLine);
        //        //Debug.Log("(prevObj) Starck Count : " + lineStack.Count);
        //    }
        //    else if (hitObjCheck == nextObj)
        //    {
        //        lineStack.Push(currentLine);
        //        //Debug.Log("nextObj 같아" + lineStack.Count);
        //    }
        //}

        ClearCheck();
    }

    void ClearCheck()
    {
        // ==== 승리 판정 ====
        //if (lineStack.Count == clearCount)
        //{
        //    Debug.Log("너가 이겼다 @!!");

        //    clearAnimaition.StartCoroutine(clearAnimaition.Moving());
        //}
        //else if (lineStack.Count > clearCount)
        //{
        //    DestroyLine();
        //}
    }

    void InstLine()
    {
        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        // === SetParent 빌드 할때 꼭 빼자 ===
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);

        GetColor();
        SetLineSize();
        linerender.SetColor(startColor);
    }


    void SetLineSize()
    {
        if (lineSizeValue == 0)
        {
            lineSizeValue = 0.15f;
        }
        else
        {
            lineSizeValue = lineSizeChange.LineSize;
            Debug.Log("lineSizeValue : " + lineSizeValue);
        }

        linerender.SetLineSize(lineSizeValue);
    }

    void GetColor()
    {
        //Debug.Log("lineColorChanged.getColor : " + lineColorChanged.getColor);
        if (colorPanel.getColor == new Color(0, 0, 0, 0))
        {
            //Debug.Log("들어옴 ??");
            float r = 0.1997152f;
            float g = 0.8301887f;
            float b = 0.7776833f;
            float a = 1f;
            startColor = new Color(r, g, b, a);
        }
        else
        {
            startColor = colorPanel.getColor;
        }

    }

    void OnClickRevertBut()
    {
        if (lineStack.Count == 0)
        {
            startNodeObj = null;
            return;
        }
        currentLine = lineStack.Pop();

        DestroyLine();
    }

    void DestroyLine()
    {
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
    }

    RaycastHit2D RayCheck()
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}