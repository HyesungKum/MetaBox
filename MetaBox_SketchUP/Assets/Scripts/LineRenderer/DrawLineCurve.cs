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
        colorPanel.gameObject.SetActive(true);
        lineSizeChange.gameObject.SetActive(true);
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
            if (hitInfo.collider.name.Equals("Collider"))
            {
                return;
            }
            if (currentLine == null)
            {
                startNodeObj = hitInfo.transform.gameObject; // Ŭ���� �� ������Ʈ �޾ƿ���
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // ���� ����� ����Ʈ���� ��尡 �ִ��� ã��
                InstLine(); // ���� ����

                startNodeObj = circleObj.cdNode.data.circlePointObj; // ù Ŭ�� ��� �� :4
                //Debug.Log("## ù ��ġ startNodeObj : " + startNodeObj);
                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // ��: 5
                //Debug.Log("## ù ��ġ prevObj : " + prevObj);

                nextObj = circleObj.cdNode.next.data.circlePointObj; // ��: 3
                //Debug.Log("## ù ��ġ nextObj : " + nextObj);

                startPos = startNodeObj.transform.position; // ù ���� Pos �����ֱ�
                linerender.SetPosition(0, startPos); // ���η����� ������ ���� ���ֱ�
                linerender.SetPosition(1, startPos); // ���η����� 2��° �����ǵ� ���� ���ֱ�
            }
            else
            {
                InstLine();
                //currentLine.transform.position = endNodeObj.transform.position;
                startNodeObj = endNodeObj;
                //Debug.Log("## �ٽ� ��ġ �� startNodeObj : " + startNodeObj);
                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj); // ���� ����� ����Ʈ���� ��尡 �ִ��� ã��

                prevObj = circleObj.cdNode.prev.data.circlePointObj;  // ��: 5
                //Debug.Log("## �ٽ� ��ġ �� prevObj : " + prevObj);
                nextObj = circleObj.cdNode.next.data.circlePointObj; // ��: 3
                //Debug.Log("## �ٽ� ��ġ �� nextObj : " + nextObj);

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
            linerender.SetCurvePosition(touchPos); // ���η����� ������ ��� ����ϰ� �׸��� ���ֱ�
            GameObject hitObjCheck = hitInfo.transform.gameObject; // ���� ������ üũ
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
            else if (hitObjCheck != prevObj && hitObjCheck != nextObj)
            {
                if (hitInfo.collider.Equals("Collider"))
                {
                    linerender.SetPosition(1, chekcPos);
                }
                else
                {
                    //DestroyLine();
                    //Debug.Log("���� �� ??");
                }
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
                lineStack.Push(currentLine);
                //Debug.Log("(prevObj)Starck Count : " + lineStack.Count);
                //Debug.Log("ClearCount : " + ClearCount);
                SoundManager.Inst.ConnectLineSFXPlay(); // ȿ����
                instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles); // ����Ʈ ����
                instParticles.transform.position = endNodeObj.transform.position;
                ClearCheck();
                return;
            }
            else if (endNodeObj == nextObj && endNodeObj != prevObj)
            {
                linerender.SetPosition(1, nextObj.transform.position);
                lineStack.Push(currentLine);
                //Debug.Log("(nextObj)Starck Count : " + lineStack.Count);
                //Debug.Log("ClearCount : " + ClearCount);
                SoundManager.Inst.ConnectLineSFXPlay(); // ȿ����
                instParticles = ObjectPoolCP.PoolCp.Inst.BringObjectCp(particles); // ����Ʈ ����
                instParticles.transform.position = endNodeObj.transform.position;
                ClearCheck();
                return;
            }
            else
            {
                DestroyLine();
            }  
        }
    }

    void ClearCheck()
    {
        // ==== �¸� ���� ====
        if (lineStack.Count == ClearCount)
        {
            Debug.Log("�ʰ� �̰�� @!!");
            clearAnimaition.StartCoroutine(clearAnimaition.Moving());
            ObjSetFalse();

            choiceWordPanel.gameObject.SetActive(true);

            int childCount = lineClonePos.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject destoryLine = lineClonePos.transform.GetChild(i).gameObject;
                ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(destoryLine);
            }

            lineStack.Clear(); // ���� ����

            //Debug.Log("lineStack Clear : " + lineStack.Count);
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
        // === SetParent ���� �Ҷ� �� ���� ===
        currentLine.transform.SetParent(lineClonePos);
        currentLine.TryGetComponent<LineRender>(out linerender);

        GetColor();
        SetLineSize();
        linerender.SetColor(startColor);
        linerender.SetLineSize(lineSizeValue);
    }

    void InstPrticle()
    {
        //Debug.Log("��ƼŬ ���������?");
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
        Debug.Log("lineStack Pop : " + lineStack.Count); ;
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