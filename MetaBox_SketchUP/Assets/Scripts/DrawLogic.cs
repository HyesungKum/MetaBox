using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectPoolCP;

public class DrawLogic : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instPos = null;
    [SerializeField] Button revertButton = null;

    Touch myTouch;
    private Vector3 startPos;
    private Vertex startVertex = null;

    GameObject instLine = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    int vCount;

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
        if (Input.touchCount <= 0) return; // 입력이 없으면 return

        myTouch = Input.GetTouch(0);
        Vector3 TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        RaycastHit2D objCheck = RayCheck(TouchPos);
        Vertex collisionVertex = null;

        if (objCheck)
        {
            if (objCheck.collider.name.Equals("DrowPoint"))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)  // Touch Start
                {
                    instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                    //Debug.Log("첫 생성");
                    instLine.transform.SetParent(instPos); // Inst Line setParent 

                    instLine.transform.localScale = new Vector3(1f, 1f, 0f);
                    instLine.transform.localRotation = Quaternion.identity;

                    startPos = Input.GetTouch(0).position;
                    instLine.transform.position = startPos;

                    if (objCheck.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
                    {
                        if (startVertex == null)
                        {
                            startVertex = collisionVertex;
                            collisionVertex.StartPointColor();

                            checkVertex.Push(collisionVertex);  // drow point Stack
                        }
                        else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                        {
                            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                            Debug.Log("1)삭제 됨 ??");

                            //instLine = lineBackStack.Pop();
                        }
                    }
                }

                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
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
                                    //Debug.Log("두번째 생성");
                                    //lineBackStack.Push(instLine);

                                    instLine.transform.SetParent(instPos.transform);
                                    checkVertex.Push(collisionVertex);  // drow point Stack

                                    startPos = Input.GetTouch(0).position;
                                    instLine.transform.position = startPos;

                                    Vector3 myPos = Input.GetTouch(0).position;

                                    instLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                                    instLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));

                                    startVertex = collisionVertex;

                                    break;
                                }
                            }
                            vCount += 1;
                        }
                    }
                }

                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
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
                                else if (i == startVertex.GetNodeLength() - 1)
                                {
                                    //Debug.Log("들어옴 ??");
                                    ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                                    //Debug.Log("2)삭제 됨 ??");
                                    //instLine = lineBackStack.Pop();
                                }
                            }
                        }
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
                Debug.Log("3)삭제 됨 ??");
            }
        }
    }

    #region 전 클릭 이벤트
    public void OnClickPopStack()
    {
        if (lineBackStack.Count == 0)
            return;

        startVertex = checkVertex.Pop().GetComponent<Vertex>();

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(lineBackStack.Pop());

        Vertex ChangedColor = startVertex;

        ChangedColor.BackOriginalColor();
    }
    #endregion

    public void RevertEvent(GameObject revertObj)
    {
        if (checkVertex.Count == 0 && lineBackStack.Count == 0) return;
        Vertex Changed = checkVertex.Pop();
        Changed.BackOriginalColor();

        revertObj = lineBackStack.Pop();
    }

    public void OnClickRevertButton()
    {
        if (checkVertex.Count == 0) return;
        //if (lineBackStack.Count == 0) return;
        Vertex Changed = checkVertex.Pop();

        // === 다시 스타트 포스를 파란색으로 돌리기
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
