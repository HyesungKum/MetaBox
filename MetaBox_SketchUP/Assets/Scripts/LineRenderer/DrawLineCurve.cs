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
        choiceWordPanel.gameObject.SetActive(false); // ���� �ǳ� ��Ȱ��ȭ
        clearAnimation.gameObject.SetActive(false); // �ִϸ��̼� �̹��� ��Ȱ��ȭ
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
                startNodeObj = hitInfo.transform.gameObject; // Ŭ���� �� ������Ʈ �޾ƿ���
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // ���� ����� ����Ʈ���� ��尡 �ִ��� ã��
                InstLine(); // ���� ����

                startNodeObj = circleObj.cdNode.data.circlePointObj;// ù Ŭ�� ��� �� :4
                Debug.Log("#1)Touch Start : " + startNodeObj);
                endNodeObj = startNodeObj;
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // ��: 5
                nextObj = circleObj.cdNode.next.data.circlePointObj; // ��: 3

                startPos = startNodeObj.transform.position; // ù ���� Pos �����ֱ�
                linerender.SetPosition(0, startPos); // ���η����� ������ ���� ���ֱ�
                linerender.SetPosition(1, startPos); // ���η����� 2��° �����ǵ� ���� ���ֱ�
            }
            else if (startNodeObj != null)
            {
                InstLine();
                Debug.Log("#2)Touch Start : " + startNodeObj);
                endNodeObj = startNodeObj;
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // ���� ����� ����Ʈ���� ��尡 �ִ��� ã��
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // ��: 5
                nextObj = circleObj.cdNode.next.data.circlePointObj; // ��: 3

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
            GameObject hitObjCheck = hitInfo.transform.gameObject; // ���� ������ üũ
            linerender.SetCurvePosition(touchPos); // ���η����� ������ ��� ����ϰ� �׸��� ���ֱ�

            Debug.Log("## �ٲ�� �� startPos : " + startNodeObj);
            //Debug.Log("hitPos : " + hitObjCheck.transform.position);
            //endNodeObj = startNodeObj;
            if (hitObjCheck == prevObj)
            {
                startNodeObj = hitObjCheck.gameObject;
                Debug.Log("## �ٲ�� ���� startPos : " + startNodeObj);
                Debug.Log("## EndNodeObj : " + endNodeObj);

                linerender.SetPosition(1, hitObjCheck.transform.position);
                prevObj = circleObj.cdNode.prev.prev.data.circlePointObj;
                //Debug.Log("## move startNodeObj )" + startNodeObj);
                //Debug.Log("## move prevObj )" + prevObj);


                InstPrticle(startNodeObj.transform.position);
                Debug.Log("@@ ����Ʈ ��� @@");
                SoundManager.Inst.ConnectLineSFXPlay(); // ȿ����

                if (linerender.GetPosition(0) == linerender.GetPosition(1))
                {
                    linerender.SetPosition(1, hitObjCheck.transform.position);
                    Debug.Log("## P)�ΰ� �������� 0�� 1�� ������");
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
                    Debug.Log("## N)�ΰ� �������� 0�� 1�� ������");
                }
                InstPrticle(startNodeObj.transform.position);
                Debug.Log("@@ ����Ʈ ��� @@");
                SoundManager.Inst.ConnectLineSFXPlay(); // ȿ����
                linerender.SetPosCountTwo();
                //Debug.Log("ClearCount : " + ClearCount);

                lineStack.Push(currentLine);
                Debug.Log("$$ N)Starck Count : " + lineStack.Count);
            }
            else
            {
                //if(linerender)
                Debug.Log("Move Else �϶�");
            }
        }
    }

    void MoveEnd(RaycastHit2D hitInfo)
    {
        if (startNodeObj == null) return;
        
        
        // ==== end == start ������ üũ �غ���
        //if(end)

        //if (hitInfo)
        //{
        //    if (hitInfo.collider.name.Equals("Collider"))
        //    {
        //        Debug.Log("move end �ݶ��̴�����");
        //        DestroyLine();
        //        StackPop();
        //        linerender.PosReset();
        //        // ==== ������ �ٲ��ֱ�
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
                //Debug.Log("�������� �ΰ��� ����");
            }
            if (linerender.GetPositionCount() > 2)
            {
                //Debug.Log("SSSSSSSSS linePosCount : " + linerender.GetPositionCount());
                //DestroyLine();
                //Debug.Log("�������� �ΰ� �̻��̾�");
            }

            Invoke(nameof(ClearCheck), 0.5f);
            //ClearCheck();
        }
    }

    void ClearCheck()
    {
        // ==== �¸� ���� ====
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

            lineStack.Clear(); // ���� ����
        }
        else if (lineStack.Count > clearCount)
        {
            DestroyLine();
        }
    }

    void StackPop()
    {
        if (lineStack.Count == 0) return;
        currentLine = lineStack.Pop(); // ���� ���� ����
        Debug.Log("stackPop : " + lineStack.Count);
        currentLine.TryGetComponent<LineRender>(out linerender);
        linerender.PosReset();
    }

    void ObjSetFalse()
    {
        checkObj.transform.gameObject.SetActive(false);
        revertBut.transform.gameObject.SetActive(false);
        InGamePanelSet.Inst.LineColorAndSizeChange(false); // �÷� �ǳ� ��Ȱ��ȭ
    }

    void InstLine()
    {
        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);

        GetColor(); // �÷� �ʱ� ���� �� ���ο� �÷� �޾ƿ���
        SetLineSize();  // ���� ������ ���� �� ����
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