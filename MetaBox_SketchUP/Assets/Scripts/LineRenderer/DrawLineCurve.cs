using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.ParticleSystem;

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
    [SerializeField] GameObject clearAnimation = null;
    [SerializeField] private int clearCount;

    [SerializeField] GameObject particles = null;

    GameObject instParticles = null;

    public int ClearCount { get { return clearCount; } set { clearCount = value; } }


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
        InGamePanelSet.Inst.LineColorAndSizeChange(false);
        choiceWordPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        clearAnimation.TryGetComponent<ClearAnimation>(out clearAnimaition);
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
                return;
            }
            if (currentLine == null)
            {
                startNodeObj = hitInfo.transform.gameObject; // 클릭한 걸 받아오기
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj);
                InstLine(); // 라인 만들고 
                currentLine.transform.position = startNodeObj.transform.position;
                //Debug.Log("currentLine.transform.position : " + currentLine.transform.position);

                startNodeObj = circleObj.cdNode.data.circlePointObj;
                Debug.Log("starNodeObj : " + startNodeObj);
                prevObj = circleObj.cdNode.prev.data.circlePointObj;
                Debug.Log("prevObj : " + prevObj);
                nextObj = circleObj.cdNode.next.data.circlePointObj;
                Debug.Log("nextObj : " + nextObj);

                startPos = startNodeObj.transform.position;
                linerender.SetPosition(0, startPos);
                linerender.SetPosition(1, startPos);
            }
            else
            {
                InstLine();

                startNodeObj = hitInfo.transform.gameObject;
                currentLine.transform.position = startNodeObj.transform.position;

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
                //linerender.PositionDown(1);
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

        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            GameObject hitObjCheck = hitInfo.transform.gameObject;

            if (hitObjCheck == prevObj)
            {
                instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles);
                Debug.Log("instParticles :  " + instParticles);
                instParticles.transform.position = hitInfo.transform.position;
                lineStack.Push(currentLine);

                //Debug.Log("(prevObj) Starck Count : " + lineStack.Count);
            }
            else if (hitObjCheck == nextObj)
            {
                lineStack.Push(currentLine);
                //Debug.Log("nextObj 같아" + lineStack.Count);
            }
        }

        //if (linerender.GetPosition(0) == linerender.GetPosition(1))
        //{
        //    Debug.Log(" 삭제 함 ?? ");
        //    DestroyLine();
        //}

        ClearCheck();
    }

    void ClearCheck()
    {
        // ==== 승리 판정 ====
        if (lineStack.Count == ClearCount)
        {
            Debug.Log("너가 이겼다 @!!");
            clearAnimaition.StartCoroutine(clearAnimaition.Moving());
            checkObj.transform.gameObject.SetActive(false);
            revertBut.transform.gameObject.SetActive(false);
            colorPanel.transform.gameObject.SetActive(false);
            lineSizeChange.transform.gameObject.SetActive(false);
            
            // 라인렌더러 다 삭제 하기

            choiceWordPanel.gameObject.SetActive(true);
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

        GetColor();
        SetLineSize();
        linerender.SetColor(startColor);
        linerender.SetLineSize(lineSizeValue);
    }

    void InstPrticle()
    {
        Debug.Log("파티클 만들어졌나?");
        instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles);
        instParticles.transform.SetParent(lineClonePos);
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

    void SetLineSize()
    {
        //Debug.Log(" @@ lineSizeChange.LineSize : " + lineSizeChange.LineSize);

        if (lineSizeValue == 0)
            lineSizeValue = 0.15f;
        if (lineSizeChange.LineSize == 0)
            lineSizeValue = 0.15f;
        else
            lineSizeValue = lineSizeChange.LineSize;
    }

    void GetColor()
    {

        if (colorPanel.GetColor == new Color(0, 0, 0, 0))
        {
            float r = 0.9411765f;
            float g = 0.9019608f;
            float b = 0.2156863f;
            float a = 1f;
            startColor = new Color(r, g, b, a);
            //Debug.Log("## colorPanel.GetColor : " + colorPanel.GetColor);
        }
        else
        {
            startColor = colorPanel.GetColor;
            //Debug.Log("## colorPanel.GetColor : " + colorPanel.GetColor);
        }
    }
}