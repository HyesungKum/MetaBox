using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawLineCurve : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab = null;
    [Header("[Object Prefabs]")]
    [SerializeField] GameObject checkObj = null;
    [SerializeField] Button revertBut = null;
    [SerializeField] Transform lineClonePos = null;

    [Header("[Color Changed]")]
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Slider colorAlpha = null;
    [SerializeField] Slider lineSize = null;

    [SerializeField] int clearCount;
    [SerializeField] GameObject animation = null;
    ClearAnimation clearAnimaition = null;

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
    LineColorChanged lineColorChanged = null;

    int circleObjCount;
    int nodePosCount;
    int linePosCount;

    GameObject prevObj = null;
    GameObject nextObj = null;

    Stack<GameObject> lineStack;

    Color startColor;

    float colorAlphaValue;
    float lineSizeValue;

    void Awake()
    {

        mainCam = Camera.main;
        lineStack = new Stack<GameObject>();

        float r = 0.1997152f;
        float g = 0.8301887f;
        float b = 0.7776833f;
        colorAlphaValue = 1;
        lineSizeValue = 1;
        lineSize.value = lineSizeValue;
        colorAlpha.value = colorAlphaValue;
        startColor = new Color(r, g, b, colorAlphaValue);

        colorOne.onClick.AddListener(delegate { GetOtherColor(colorOne); ColorPosMove(colorOne); /*SoundManager.Inst.ClearSFXPlay(); */});
        colorTwo.onClick.AddListener(delegate { GetOtherColor(colorTwo); ColorPosMove(colorTwo); /* SoundManager.Inst.ClearSFXPlay(); */});
        colorThree.onClick.AddListener(delegate { GetOtherColor(colorThree); ColorPosMove(colorThree); /* SoundManager.Inst.ClearSFXPlay(); */});
    }

    void Start()
    {
        animation.TryGetComponent<ClearAnimation>(out clearAnimaition);
        checkObj.TryGetComponent<LinePosCDLinkedList>(out circleObj);
        circleObjCount = circleObj.circlePointArry.Length;
        revertBut.onClick.AddListener(delegate { OnClickRevertBut(); });
        colorAlpha.onValueChanged.AddListener(delegate { SetColorAlpha(); });
        lineSize.onValueChanged.AddListener(delegate { SetLineSize(); });
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
            if (currentLine == null)
            {
                InstLine();
                startNodeObj = hitInfo.transform.gameObject;
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj);

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
                    //InstLine();
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
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            GameObject hitObjCheck = hitInfo.transform.gameObject;

            if (hitObjCheck == prevObj)
            {
                lineStack.Push(currentLine);
                //Debug.Log("(prevObj) Starck Count : " + lineStack.Count);
            }
            else if (hitObjCheck == nextObj)
            {
                lineStack.Push(currentLine);
                //Debug.Log("nextObj 같아" + lineStack.Count);
            }
        }

        ClearCheck();
    }

    void ClearCheck()
    {
        if (lineStack.Count == clearCount)
        {
            Debug.Log("너가 이겼다 @!!");

            clearAnimaition.StartCoroutine(clearAnimaition.Moving());
        }
        else if (lineStack.Count > clearCount)
        {
            DestroyLine();
        }
    }

    void InstLine()
    {
        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        // === SetParent 빌드 할때 꼭 빼자 ===
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);
        linerender.SetColor(startColor);
        colorAlphaValue = startColor.a;
    }

    void SetColorAlpha()
    {
        colorAlphaValue = colorAlpha.value;
        startColor.a = colorAlphaValue;
    }

    void SetLineSize()
    {
        lineSizeValue = lineSize.value;
        linerender.SetLineSize(lineSizeValue);
    }

    void GetOtherColor(Button colorNum)
    {
        Image imgageColor = null;
        colorNum.transform.gameObject.TryGetComponent<Image>(out imgageColor);
        startColor = imgageColor.color;
        startColor.a = colorAlpha.value;
    }

    void ColorPosMove(Button colorNum)
    {
        Transform rackPos = colorNum.gameObject.transform;
        Transform startPos = rackPos;

        rackPos.localPosition = new Vector3(rackPos.localPosition.x, rackPos.localPosition.y + 60, rackPos.localPosition.z);
        //Debug.Log("rackPos.localPosition : " + rackPos.localPosition);
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