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
            SoundManager.Inst.ButtonSFXPlay();
            SoundManager.Inst.ButtonEffect(revertBut.transform.position);
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
            }
            // startNodeObj 가 null 아닌 것은 이전에 그었던 마지막 점인거다.
            else if (startNodeObj != null)
            {
                // 마지막 긋던 점이므로 이번에 터치한 것과 다르면 안된다.
                if (startNodeObj != hitInfo.transform.gameObject)
                    return;
            }

            // 터치한 점을 기준으로 라인을 생성한다.
            circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // 원형 양방향 리스트에서 노드가 있는지 찾기
            InstLine(); // 라인 생성

            startNodeObj = circleObj.cdNode.data.circlePointObj;// 첫 클릭 노드 예 :4
            endNodeObj = startNodeObj;
            prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
            nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3

            startPos = startNodeObj.transform.position; // 첫 시작 Pos 정해주기
            linerender.SetPosition(0, startPos); // 라인렌더러 포지션 셋팅 해주기
            linerender.SetPosition(1, startPos); // 라인렌더러 2번째 포지션도 셋팅 해주기
        }
    }

    void TouchMove(RaycastHit2D hitInfo)
    {
        if (startNodeObj == null) return;
        if (hitInfo)
        {
            GameObject hitObjCheck = hitInfo.transform.gameObject; // 다음 포지션 체크
            linerender.SetCurvePosition(touchPos); // 라인렌더러 포지션 고불 고불하게 그리게 해주기

            if (hitObjCheck == prevObj || hitObjCheck == nextObj)
            {
                linerender.SetPosition(1, hitObjCheck.transform.position);
                // 정상적으로 그어진 것이니 스택에 추가
                lineStack.Push(currentLine);
                // 효과 출력
                InstPrticle(hitObjCheck.gameObject.transform.position); // 임펙트
                SoundManager.Inst.ConnectLineSFXPlay(); // 효과음

                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(hitObjCheck); // 원형 양방향 리스트에서 노드가 있는지 찾기

                startNodeObj = circleObj.cdNode.data.circlePointObj;// 첫 클릭 노드 예 :4
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // 예: 5
                nextObj = circleObj.cdNode.next.data.circlePointObj; // 예: 3

                startPos = startNodeObj.transform.position; // 첫 시작 Pos 정해주기
                InstLine(); // 라인 생성
                linerender.SetPosition(0, startPos); // 라인렌더러 포지션 셋팅 해주기
                linerender.SetPosition(1, startPos); // 라인렌더러 2번째 포지션도 셋팅 해주기
                //lineStack.Push(currentLine);

            }
            // 이웃점을 터치한 것이 아니므로 취소
            else
            {
                // 이웃점은 아니지만 다른 점을 터치했다는 얘기
                // 다른 점을 터치했으므로 취소처리
                if (startNodeObj != hitObjCheck &&
                    circleObj.cdLinkedList.SearchObj(hitObjCheck) != null)
                    MoveEnd(hitInfo);
            }
        }
    }

    void MoveEnd(RaycastHit2D hitInfo)
    {
        //Debug.Log("StackCount : " + lineStack.Count);
        //Debug.Log("line Stack에 있니 ? " + lineStack.Contains(currentLine));
        //Debug.Log("End LineCount : " + linerender.GetPositionCount());
        if (startNodeObj == null) return;

        // 그어지고 있는 라인은 취소
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
        linerender.PosReset();
        //startNodeObj = null; // startNodeObj 가 null 아니면 선을 긋던 마지막 점인거다. 여기서 null을 하면 안된다.
        endNodeObj = null;
        prevObj = null;
        nextObj = null;

        Invoke(nameof(ClearCheck), 0.2f);
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