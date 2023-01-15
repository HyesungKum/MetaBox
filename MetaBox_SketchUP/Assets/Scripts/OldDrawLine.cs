using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectPoolCP;

public class OldDrawLine : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instPos = null;
    [SerializeField] Button revertButton = null;

    Touch myTouch;

    private Vector3 startPos;
    //private Vertex startVertex = null;
    public Vertex startVertex = null;


    //private Vertex tempVertex = null;
    public Vertex tempVertex = null;

    //private Vertex endVertex = null;
    public Vertex endVertex = null;

    GameObject instLine = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    int winCount = 0;
    public int checkInstLine = 0;
    public GameObject check = null;
    private int verticesCount = 0;
    int vCount;

    bool isMoveEnd = false;

    bool isInstLine = false;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        // Revert Button Event
        revertButton.onClick.AddListener(() => OnClickRevertButton());
    }

    void Start()
    {
        vCount = 0;
        verticesCount = FindObjectsOfType<Vertex>().Length;
    }

    void Update()
    {
        if (Input.touchCount <= 0) return; // �Է��� ������ return

        myTouch = Input.GetTouch(0);
        Vector3 TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        RaycastHit2D objCheck = RayCheck(TouchPos);

        Vertex collisionVertex = null;

        if (objCheck)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)  // Touch Start
            {
                instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                checkInstLine += 1;
                // Inst Again Local Position ReSetting
                instLine.transform.localScale = new Vector3(1f, 1f, 0f);
                instLine.transform.localRotation = Quaternion.identity;

                // Setting Parent Pos
                instLine.transform.SetParent(instPos.transform); // Inst Line setParent 

                startPos = Input.GetTouch(0).position;
                instLine.transform.position = startPos;

                if (objCheck.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
                {
                    if (startVertex == null)
                    {
                        tempVertex = collisionVertex; // Start DrowPoint Save
                        startVertex = collisionVertex;
                        collisionVertex.StartPointColor();

                        checkVertex.Push(collisionVertex);  // drow point Stack
                                                            //Debug.Log("checkVertex ���� �Ѱ� �߰� : " + checkVertex.Count);

                        check = startVertex.transform.parent.gameObject; // parent GameObject
                    }
                    else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                    {
                        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                        Debug.Log("## 1 Pool ����");
                        checkInstLine -= 1;
                    }
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (objCheck.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                {
                    if (instLine != null)
                    {
                        for (int i = 0; i < startVertex.GetNodeLength(); i++)
                        {
                            if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                            {
                                collisionVertex.ColorChange(); // Color Changed 

                                instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                                instLine.transform.SetParent(instPos.transform);
                                checkInstLine += 1;

                                checkVertex.Push(collisionVertex);  // drow point Stack

                                startPos = Input.GetTouch(0).position;
                                instLine.transform.position = startPos;

                                Vector3 myPos = Input.GetTouch(0).position;

                                instLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                                instLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));

                                startVertex = collisionVertex;
                                continue;
                            }
                        }

                        vCount += 1;
                        // === ��ġ üũ === (��� �ȺҸ���)
                        //Debug.Log("vCount : " + vCount);
                    }
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (startVertex != null)
                {
                    if (objCheck.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                    {
                        for (int i = 0; i < startVertex.GetNodeLength(); i++)
                        {
                            if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                            {
                                return;
                            }
                            else if (i == startVertex.GetNodeLength() - 1 && lineBackStack.Count != 0)
                            {
                                ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                                Debug.Log("## 2 Pool ����");
                                checkInstLine -= 1;

                            }
                        }
                        //endVertex = collisionVertex;
                    }
                }
            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved)  // Touch Move
        {
            Vector3 myPos = Input.GetTouch(0).position;

            if (instLine != null)
            {
                instLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                instLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));
            }
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (instLine != null)
            {
                ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                Debug.Log("## 3 Pool ����");

                if (checkInstLine == 0) return;

                if (instLine.active == false && checkInstLine > 0)
                {
                    checkInstLine -= 1;
                }
            }
        }
    }

    public void OnClickPopStack()
    {
        if (lineBackStack.Count == 0)
            return;

        startVertex = checkVertex.Pop().GetComponent<Vertex>();

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(lineBackStack.Pop());

        Vertex ChangedColor = startVertex;

        ChangedColor.BackOriginalColor();
    }

    public void OnClickRevertButton()
    {
        if (checkVertex.Count == 0) return;
        //if (lineBackStack.Count == 0) return;

        //GameObject delect = lineBackStack.Pop();

        Vertex Changed = checkVertex.Pop();

        // === �ٽ� ��ŸƮ ������ �Ķ������� ������

        Changed.BackOriginalColor();
    }

    // RaycastHit
    RaycastHit2D RayCheck(Vector3 touchPos)
    {
        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 5f);
        return hit;
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
