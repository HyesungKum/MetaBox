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
        InGamePanelSet.Inst.LineColorAndSizeChange(true);
        colorPanel.gameObject.SetActive(true);
        lineSizeChange.gameObject.SetActive(true);
        choiceWordPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        clearAnimation.TryGetComponent<ClearAnimation>(out clearAnimaition);
        checkObj.TryGetComponent<LinePosCDLinkedList>(out circleObj);
        circleObjCount = circleObj.circlePointArry.Length;
        revertBut.onClick.AddListener(delegate
        {
            OnClickRevertBut();
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(revertBut.transform.position);
        });
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
            if (hitInfo.collider.name.Equals("Collider"))
            {
                return;
            }
            if (currentLine == null)
            {
                startNodeObj = hitInfo.transform.gameObject; // 클릭한 걸 오브젝트 받아오기
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // 원형 양방향 리스트에서 노드가 있는지 찾기
                InstLine(); // 라인 생성

                startNodeObj = circleObj.cdNode.data.circlePointObj; // 첫 클릭 노드 예 :4
                //Debug.Log("## 첫 터치 startNodeObj : " + startNodeObj);
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
                //Debug.Log("## 첫 터치 prevObj : " + prevObj);

                nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3
                //Debug.Log("## 첫 터치 nextObj : " + nextObj);

                startPos = startNodeObj.transform.position; // 첫 시작 Pos 정해주기
                linerender.SetPosition(0, startPos); // 라인렌더러 포지션 셋팅 해주기
                linerender.SetPosition(1, startPos); // 라인렌더러 2번째 포지션도 셋팅 해주기
            }
            else if (startNodeObj != null && endNodeObj != null)
            {
                InstLine();
                //currentLine.transform.position = endNodeObj.transform.position;
                startNodeObj = endNodeObj;
                //Debug.Log("## 다시 터치 시 startNodeObj : " + startNodeObj);
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // 원형 양방향 리스트에서 노드가 있는지 찾기

                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
                //Debug.Log("## 다시 터치 시 prevObj : " + prevObj);
                nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3
                //Debug.Log("## 다시 터치 시 nextObj : " + nextObj);

                linerender.SetPosition(0, endNodeObj.transform.position);
                linerender.SetPosition(1, endNodeObj.transform.position);
            }
        }
    }

    void TouchMove()
    {
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            linerender.SetCurvePosition(touchPos); // 라인렌더러 포지션 고불 고불하게 그리게 해주기
            GameObject hitObjCheck = hitInfo.transform.gameObject; // 다음 포지션 체크
            //Debug.Log("hitObjCheck : " + hitObjCheck);

            Vector3 chekcPos = hitObjCheck.transform.position;

            if (hitObjCheck == prevObj && hitObjCheck != nextObj)
            {
                linerender.SetPosition(1, chekcPos);
                endNodeObj = prevObj;
            }
            else if (hitObjCheck == nextObj && hitObjCheck != prevObj)
            {
                linerender.SetPosition(1, chekcPos);
                endNodeObj = nextObj;
            }
            else if (hitInfo.collider.Equals("Collider"))
            {
                linerender.SetPosition(1, touchPos);
                //Debug.Log("collider.Equals : Collider ");
            }
            else if (hitObjCheck != prevObj && hitObjCheck != nextObj)
            {
                //Debug.Log("hitObjCheck 다 같지 않을 경우 (Move)");
            }
            else
            {
                endNodeObj = startNodeObj;
            }
        }
    }

    void MoveEnd()
    {
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            if (endNodeObj == prevObj && endNodeObj != nextObj)
            {
                linerender.SetPosition(1, prevObj.transform.position);
                InstPrticle(endNodeObj.transform.position);
                lineStack.Push(currentLine);
                //Debug.Log("(prevObj)Starck Count : " + lineStack.Count);
                //Debug.Log("ClearCount : " + ClearCount);
                SoundManager.Inst.ConnectLineSFXPlay(); // 효과음
                ClearCheck();
                return;
            }
            if (endNodeObj == nextObj && endNodeObj != prevObj)
            {
                linerender.SetPosition(1, nextObj.transform.position);
                InstPrticle(endNodeObj.transform.position);
                lineStack.Push(currentLine);
                //Debug.Log("(nextObj)Starck Count : " + lineStack.Count);
                //Debug.Log("ClearCount : " + ClearCount);
                SoundManager.Inst.ConnectLineSFXPlay(); // 효과음
                ClearCheck();
                return;
            }
            if (endNodeObj != nextObj && endNodeObj != prevObj)
            {
                //Debug.Log("@@ 여기 불림 ??");
                DestroyLine();
            }
        }
        if(linerender.GetPositionCount() == 2)
        {
            //Debug.Log("포지션이 2개 야??");
            lineStack.Push(currentLine);
            InstPrticle(endNodeObj.transform.position);
            //Debug.Log("(포지션이 2개)Starck Count : " + lineStack.Count);
        }
        if (linerender.GetPosition(0) == linerender.GetPosition(1))
        {
            //Debug.Log("## 0 여기 불림 ??");
            DestroyLine();
        }
        if(linerender.GetPositionCount() > 2)
        {
            //Debug.Log("## 포지션이 많아 ??");
            DestroyLine();
        }
    }

    void ClearCheck()
    {
        // ==== 승리 판정 ====
        if (lineStack.Count == ClearCount)
        {
            //Debug.Log("너가 이겼다 @!!");
            clearAnimaition.StartCoroutine(clearAnimaition.Moving());
            ObjSetFalse();

            choiceWordPanel.gameObject.SetActive(true);

            int childCount = lineClonePos.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject destoryLine = lineClonePos.transform.GetChild(i).gameObject;
                ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(destoryLine);
            }

            lineStack.Clear(); // 스택 비우기
        }
        else if (lineStack.Count > clearCount)
        {
            DestroyLine();
        }
    }

    void ObjSetFalse()
    {
        checkObj.transform.gameObject.SetActive(false);
        revertBut.transform.gameObject.SetActive(false);
        colorPanel.transform.gameObject.SetActive(false);
        lineSizeChange.transform.gameObject.SetActive(false);
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

    void InstPrticle(Vector3 instPos)
    {
        instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles);
        instParticles.transform.position = instPos;
    }

    void OnClickRevertBut()
    {
        if (lineStack.Count == 0)
        {
            startNodeObj = null;
            return;
        }
        currentLine = lineStack.Pop();
        //Debug.Log("lineStack Pop : " + lineStack.Count); ;
        endNodeObj = startNodeObj;
        DestroyLine();
    }

    void DestroyLine()
    {
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
        linerender.PosReset();
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
        if (lineSizeValue == 0) lineSizeValue = 0.15f;
        if (lineSizeChange.LineSize == 0) lineSizeValue = 0.15f;
        else lineSizeValue = lineSizeChange.LineSize;
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
        }
        else
            startColor = colorPanel.GetColor;
    }
}