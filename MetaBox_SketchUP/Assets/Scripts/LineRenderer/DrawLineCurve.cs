using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLineCurve : MonoBehaviour
{
    [Header("[Draw Line Prefab]")]
    [SerializeField] private GameObject linePrefab = null;

    [Header("[Object Prefabs]")]
    [SerializeField] GameObject checkObj = null;
    [SerializeField] Button revertBut = null;
    [SerializeField] Transform lineClonePos = null;

    [Header("[Clear Draw]")]
    [SerializeField] GameObject choiceWordPanel = null;
    [SerializeField] GameObject clearAnimation = null;

    [Header("[Clear Count]")]
    [SerializeField] private int clearCount;

    [Header("[ObjIndex]")]
    [SerializeField] private int objInex;
    public int ObjIndex { get { return objInex; } set { objInex = value; } }

    [Header("[Particle]")]
    [SerializeField] GameObject particles = null;

    GameObject instParticles = null;

    public int ClearCount { get { return clearCount; } set { clearCount = value; } }

    [Header("[Other]")]
    private Camera mainCam;
    private GameObject currentLine;

    [Header("Only Check")]
    // === Test Ende Changed private
    public GameObject startNodeObj = null;
    public GameObject endNodeObj = null;
    public GameObject prevObj = null;
    public GameObject nextObj = null;

    Touch myTouch;
    private Vector3 touchPos;
    public Vector3 startPos;
    Vector3 startPosCheck;
    LineRender linerender = null;

    LinePosCDLinkedList circleObj = null;
    CDLinkedList.CDNode cdNode = null;

    int circleObjCount;
    int nodePosCount;
    int linePosCount;

    Stack<GameObject> lineStack;

    Color startColor;
    float lineSizeValue;

    void Awake()
    {
        mainCam = Camera.main;
        lineStack = new Stack<GameObject>();
        cdNode = new CDLinkedList.CDNode();
        InGamePanelSet.Inst.LineColorAndSizeChange(true);
        choiceWordPanel.gameObject.SetActive(false); // 선택 판넬 비활성화
        clearAnimation.gameObject.SetActive(false); // 애니메이션 이미지 비활성화
        GetObjIndex();
    }

    void Start()
    {

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
        RaycastHit2D hitInfo = RayCheck();

        if (hitInfo)
        {
            switch (myTouch.phase)
            {
                case TouchPhase.Began:
                    TouchBegan(hitInfo);
                    break;

                case TouchPhase.Moved:
                    TouchMove(hitInfo);
                    break;

                case TouchPhase.Ended:
                    MoveEnd(hitInfo);
                    break;
            }
        }
    }

    void TouchBegan(RaycastHit2D hitInfo)
    {
        if (hitInfo)
        {
            if (hitInfo.collider.name.Equals("Collider")) return;
            if (startNodeObj == null)
            {
                startNodeObj = hitInfo.transform.gameObject; // 클릭한 걸 오브젝트 받아오기
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // 원형 양방향 리스트에서 노드가 있는지 찾기
                InstLine(); // 라인 생성

                startNodeObj = circleObj.cdNode.data.circlePointObj;// 첫 클릭 노드 예 :4
                Debug.Log("#1)Touch Start : " + startNodeObj);
                endNodeObj = startNodeObj;
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
                nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3

                startPos = startNodeObj.transform.position; // 첫 시작 Pos 정해주기
                linerender.SetPosition(0, startPos); // 라인렌더러 포지션 셋팅 해주기
                linerender.SetPosition(1, startPos); // 라인렌더러 2번째 포지션도 셋팅 해주기
            }
            else if (startNodeObj != null)
            {
                InstLine();
                Debug.Log("#2)Touch Start : " + startNodeObj);
                endNodeObj = startNodeObj;
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // 원형 양방향 리스트에서 노드가 있는지 찾기
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
                nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3

                linerender.SetPosition(0, startNodeObj.transform.position);
                linerender.SetPosition(1, startNodeObj.transform.position);
            }
        }
    }

    void TouchMove(RaycastHit2D hitInfo)
    {
        if (startNodeObj == null) return;
        if (hitInfo)
        {
            GameObject hitObjCheck = hitInfo.transform.gameObject; // 다음 포지션 체크
            linerender.SetCurvePosition(touchPos); // 라인렌더러 포지션 고불 고불하게 그리게 해주기

            Debug.Log("## 바뀌기 전 startPos : " + startNodeObj);
            //Debug.Log("hitPos : " + hitObjCheck.transform.position);
            //endNodeObj = startNodeObj;
            if (hitObjCheck == prevObj)
            {
                startNodeObj = hitObjCheck.gameObject;
                Debug.Log("## 바뀌고 나서 startPos : " + startNodeObj);
                Debug.Log("## EndNodeObj : " + endNodeObj);

                linerender.SetPosition(1, hitObjCheck.transform.position);
                prevObj = circleObj.cdNode.prev.prev.data.circlePointObj;
                //Debug.Log("## move startNodeObj )" + startNodeObj);
                //Debug.Log("## move prevObj )" + prevObj);


                InstPrticle(startNodeObj.transform.position);
                Debug.Log("@@ 임팩트 출력 @@");
                SoundManager.Inst.ConnectLineSFXPlay(); // 효과음

                if (linerender.GetPosition(0) == linerender.GetPosition(1))
                {
                    linerender.SetPosition(1, hitObjCheck.transform.position);
                    Debug.Log("## P)두개 포지션이 0과 1이 같으면");
                }
                //linerender.SetPosCountTwo();
                //Debug.Log("SetPosCountTwo : " + linerender.GetPositionCount());
                //Debug.Log("ClearCount : " + ClearCount);
                lineStack.Push(currentLine);
                Debug.Log("$$ P)Starck Count : " + lineStack.Count);
            }
            else if (hitObjCheck == nextObj)
            {
                startNodeObj = hitObjCheck.gameObject;

                linerender.SetPosition(1, hitObjCheck.transform.position);
                nextObj = circleObj.cdNode.next.next.data.circlePointObj;
                // Debug.Log("@@ startNodeObj)" + startNodeObj);
                //Debug.Log("@@ nextObj :" + nextObj);

                if (linerender.GetPosition(0) == linerender.GetPosition(1))
                {
                    linerender.SetPosition(1, hitObjCheck.transform.position);
                    Debug.Log("## N)두개 포지션이 0과 1이 같으면");
                }
                InstPrticle(startNodeObj.transform.position);
                Debug.Log("@@ 임팩트 출력 @@");
                SoundManager.Inst.ConnectLineSFXPlay(); // 효과음
                linerender.SetPosCountTwo();
                //Debug.Log("ClearCount : " + ClearCount);

                lineStack.Push(currentLine);
                Debug.Log("$$ N)Starck Count : " + lineStack.Count);
            }
            else
            {
                //if(linerender)
                Debug.Log("Move Else 일때");
            }
        }
    }

    void MoveEnd(RaycastHit2D hitInfo)
    {
        if (startNodeObj == null) return;
        
        
        // ==== end == start 같은지 체크 해보자
        //if(end)

        //if (hitInfo)
        //{
        //    if (hitInfo.collider.name.Equals("Collider"))
        //    {
        //        Debug.Log("move end 콜라이더였어");
        //        DestroyLine();
        //        StackPop();
        //        linerender.PosReset();
        //        // ==== 포지션 바꿔주기
        //        //startNodeObj = endNodeObj;
        //        Debug.Log(" Moev End : " + startNodeObj);
        //        Debug.Log(" Moev End : " + endNodeObj);
        //    }

        //}

        if (startNodeObj != null)
        {
            //Debug.Log("### linePosCount : " + linerender.GetPositionCount());

            if (startNodeObj == endNodeObj)
            {
                //Debug.Log("## Move End startNode == endNode ## ");
                //linerender.SetPosition(1, startNodeObj.transform.position);
                //linerender.SetPosCountTwo();
                // Debug.Log("!! end StartNodeObj : " + startNodeObj);
            }
            if (linerender.GetPosition(0) == linerender.GetPosition(1))
            {
                //DestroyLine();
                //StackPop();
                //Debug.Log("포지션이 두개가 같아");
            }
            if (linerender.GetPositionCount() > 2)
            {
                //Debug.Log("SSSSSSSSS linePosCount : " + linerender.GetPositionCount());
                //DestroyLine();
                //Debug.Log("포지션이 두개 이상이야");
            }

            Invoke(nameof(ClearCheck), 0.5f);
            //ClearCheck();
        }
    }

    void ClearCheck()
    {
        // ==== 승리 판정 ====
        if (lineStack.Count == ClearCount)
        {

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

    void StackPop()
    {
        if (lineStack.Count == 0) return;
        currentLine = lineStack.Pop(); // 스택 에서 빼기
        Debug.Log("stackPop : " + lineStack.Count);
        currentLine.TryGetComponent<LineRender>(out linerender);
        linerender.PosReset();
    }

    void ObjSetFalse()
    {
        checkObj.transform.gameObject.SetActive(false);
        revertBut.transform.gameObject.SetActive(false);
        InGamePanelSet.Inst.LineColorAndSizeChange(false); // 컬러 판넬 비활성화
    }

    void InstLine()
    {
        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);

        GetColor(); // 컬러 초기 셋팅 및 새로운 컬러 받아오기
        SetLineSize();  // 라인 사이지 셋팅 및 변경
        linerender.SetColor(startColor);
        linerender.SetLineSize(lineSizeValue);
    }

    void DestroyLine()
    {
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
        linerender.PosReset();

        StackCountZeroNull();
    }

    void OnClickRevertBut()
    {
        StackPop();
        DestroyLine();
        StackCountZeroNull();
    }

    public int GetObjIndex()
    {
        if (this.gameObject.active == true)
            return ObjIndex;
        else
            return 0;
    }

    void StackCountZeroNull()
    {
        if (lineStack.Count == 0)
        {
            startNodeObj = null;
            endNodeObj = null;
            prevObj = null;
            nextObj = null;
        }
    }

    void InstPrticle(Vector3 instPos)
    {
        instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles);
        instParticles.transform.position = instPos;
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
        if (InGamePanelSet.Inst.LineSize.LineSize == 0) lineSizeValue = 0.15f;
        else lineSizeValue = InGamePanelSet.Inst.LineSize.LineSize;
    }

    void GetColor()
    {
        if (InGamePanelSet.Inst.ColorPanel.GetColor == new Color(0, 0, 0, 0))
        {
            float r = 0.9411765f;
            float g = 0.9019608f;
            float b = 0.2156863f;
            float a = 1f;
            startColor = new Color(r, g, b, a);
        }
        else
            startColor = InGamePanelSet.Inst.ColorPanel.GetColor;
    }
}