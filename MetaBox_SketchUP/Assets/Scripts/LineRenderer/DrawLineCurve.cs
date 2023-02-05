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
    [SerializeField] TextMeshProUGUI clearText = null;

    [Header("[Color Changed]")]
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Slider colorAlpha = null;

    private Camera mainCam;
    private GameObject currentLine;
    public GameObject startNodeObj = null;
    GameObject endNodeObj = null;

    Touch myTouch;
    private Vector3 touchPos;
    public Vector3 startPos;
    LineRender linerender = null;

    LinePosCDLinkedList circleObj = null;
    LineColorChanged lineColorChanged = null;

    int circleObjCount;
    int nodePosCount;
    int linePosCount;

    GameObject prevObj = null;
    GameObject nextObj = null;

    Stack<GameObject> lineStack;

    public Color startColor;
    public Color newColor;
    public Color tempColor;

    float colorAlphaValue;

    void Awake()
    {
        mainCam = Camera.main;
        clearText.transform.gameObject.SetActive(false);
        lineStack = new Stack<GameObject>();

        float r = 0.1997152f;
        float g = 0.8301887f;
        float b = 0.7776833f;
        colorAlphaValue = 1;
        colorAlpha.value = colorAlphaValue;
        startColor = new Color(r, g, b, colorAlphaValue);

        colorOne.onClick.AddListener(delegate { GetOtherColor(colorOne); });
        colorTwo.onClick.AddListener(delegate { GetOtherColor(colorTwo); });
        colorThree.onClick.AddListener(delegate { GetOtherColor(colorThree); });
    }

    void Start()
    {
        checkObj.TryGetComponent<LinePosCDLinkedList>(out circleObj);
        circleObjCount = circleObj.circlePointArry.Length;
        revertBut.onClick.AddListener(delegate { OnClickRevertBut(); });
        colorAlpha.onValueChanged.AddListener(delegate { SetColorAlpha(); });
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
                    RaycastHit2D hitInfo = RayCheck();
                    if (hitInfo)
                    {
                        TouchBegan(hitInfo);
                    }
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    RaycastHit2D hitInfo = RayCheck();

                    if (hitInfo)
                    {
                        linerender.SetCurvePosition(touchPos);
                        GameObject hitObjCheck = hitInfo.transform.gameObject;

                        string collderCheck = hitInfo.collider.name;
                        if (hitObjCheck == prevObj)
                        {
                            //Debug.Log("prevObj : " + prevObj);
                            Vector3 check = hitInfo.transform.position;
                            linerender.SetPosition(1, check);
                            endNodeObj = prevObj;
                        }
                        else if (hitObjCheck == nextObj)
                        {
                            Vector3 check = hitInfo.transform.position;
                            linerender.SetPosition(1, check);
                            endNodeObj = nextObj;
                        }
                        else
                        {
                            endNodeObj = startNodeObj;
                        }

                    }
                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {
                    #region
                    //Debug.Log("%%endNodeObj (Ended) %%" + endNodeObj);


                    //currentLine.TryGetComponent<LineRender>(out linerender);
                    //if (linerender.GetPosition(0) == new Vector3(0, 0, 0))
                    //{
                    //    Debug.Log(" ## 1 라인 삭제하라우 ~");
                    //    ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                    //    linerender.SetPosition(0, new Vector3(0, 0, 0));
                    //    linerender.SetPosition(1, new Vector3(0, 0, 0));
                    //}
                    //if (linerender.GetPosition(1) == new Vector3(0, 0, 0))
                    //{
                    //    Debug.Log(" ## 0 라인 삭제하라우 ~");
                    //    ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                    //    linerender.SetPosition(0, new Vector3(0, 0, 0));
                    //    linerender.SetPosition(1, new Vector3(0, 0, 0));

                    //}
                    #endregion

                }
                break;
                #endregion
        }
    }

    void TouchBegan(RaycastHit2D hitInfo)
    {

        if (startNodeObj == null)
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
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(endNodeObj);
                startNodeObj = circleObj.cdNode.data.circlePointObj;

                prevObj = circleObj.cdNode.prev.data.circlePointObj;
                nextObj = circleObj.cdNode.next.data.circlePointObj;

                startPos = startNodeObj.transform.position;
                linerender.SetPosition(0, startPos);
                linerender.SetPosition(1, startPos);
            }
            else
            {
                DestroyLine();
            }
        }
    }


    void InstLine() 
    {
        //if (currentLine == null)
        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        lineStack.Push(currentLine);
        // === SetParent 빌드 할때 꼭 빼자 ===
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);
        //linerender.GetLineColor(); // 기존 컬러 받아 오기
        linerender.SetColor(startColor);
        colorAlphaValue = startColor.a;
        //Debug.Log("### colorAlphaValue : " + colorAlphaValue);
    }

    void SetColorAlpha()
    {
        colorAlphaValue = colorAlpha.value;
        startColor.a = colorAlphaValue;
        //Debug.Log("colorAlphaValue : " + colorAlphaValue);
    }

    void GetOtherColor(Button colorNum)
    {
        Image imgageColor = null;
        colorNum.transform.gameObject.TryGetComponent<Image>(out imgageColor);
        startColor = imgageColor.color;
        startColor.a = colorAlpha.value;
    }


    void OnClickRevertBut()
    {
        if (lineStack.Count == 0) return;
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
