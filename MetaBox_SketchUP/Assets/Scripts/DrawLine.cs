using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;


public class DrawLine : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instPos = null;

    Touch myTouch;
    private Vector3 startPos;
    //private Vertex startVertex = null;
    public Vertex startVertex = null;

    //private Vertex tempVertex = null;
    public Vertex tempVertex = null;

    //private Vertex endVertex = null;
    public Vertex endVertex = null;

    private Clear clearImg = null;

    GameObject instLine = null;

    Stack<GameObject> lineBackStack = new Stack<GameObject>();
    Stack<GameObject> checkBack = new Stack<GameObject>();

    int winCount = 0;

    public GameObject check = null;
    private int verticesCount = 0;
    int vCount;

    void Start()
    {
        vCount = 0;
        verticesCount = FindObjectsOfType<Vertex>().Length;
        clearImg = FindObjectOfType<Clear>();
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
                    //checkBack.Clear();
                    checkBack.Push(objCheck.transform.gameObject);  // drow point Stack

                    instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab); // Object Pool
                    instLine.transform.SetParent(instPos.transform); // Inst Line setParent 

                    lineBackStack.Push(instLine);  // linStack Stack Add One Object

                    startPos = Input.GetTouch(0).position;
                    instLine.transform.position = startPos;

                    if (objCheck.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
                    {
                        if (startVertex == null)
                        {
                            collisionVertex.StartPointColor();
                            tempVertex = collisionVertex; // Start DrowPoint Save
                            startVertex = collisionVertex;

                            check = startVertex.transform.parent.gameObject; // parent GameObject 
                            //Debug.Log("parent.name : " + check);
                        }
                        else
                        {
                            if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                            {
                                Destroy(instLine.gameObject);
                            }
                        }
                    }
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //Vertex collisionVertex = null;

                    if (objCheck.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                    {
                        if (instLine != null)
                        {
                            for (int i = 0; i < startVertex.GetNodeLength(); i++)
                            {
                                if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                                {
                                    //Debug.Log("## check name : " + collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)));
                                    //Debug.Log("collisionVertex :" + collisionVertex);

                                    checkBack.Push(objCheck.transform.gameObject);  // drow point Stack

                                    collisionVertex.ColorChange(); // Color Changed 

                                    instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                                    instLine.transform.SetParent(instPos.transform);

                                    lineBackStack.Push(instLine); // linStack Stack Add One Object

                                    startPos = Input.GetTouch(0).position;
                                    instLine.transform.position = startPos;

                                    Vector3 myPos = Input.GetTouch(0).position;

                                    instLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                                    instLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));

                                    startVertex = collisionVertex;
                                    break;
                                }
                            }
                            vCount++;
                            //Debug.Log(vCount + " count!");
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
                                else if (i == startVertex.GetNodeLength() - 1)
                                {
                                    Destroy(instLine.gameObject);
                                }
                            }
                            endVertex = collisionVertex;
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
                Destroy(instLine.gameObject);

                if (endVertex == tempVertex)
                {
                    if (clearImg != null)
                    {
                        if (check.transform.name == "StarObj1")
                        {
                            winCount += 1;
                            clearImg.ClearIMgObjTwo();
                        }
                        else if(check.transform.name == "Obj3")
                        {
                            winCount += 1;
                            clearImg.ClearImgOne();
                        }
                        else if (check.transform.name == "Obj2")
                        {
                            winCount += 1;
                            clearImg.ClearIMgObjThree();
                        }
                    }
                }
            }
        }
    }

    //void CheckFindsh(GameObject obj, string parentName)
    //{
    //    if (obj.transform.name == parentName)
    //    {
    //        clearImg.ClearIMgObjTwo();
    //    }
    //}

    void CheckObjFinsh()
    {

    }


    public void OnClickPopStack()
    {
        if (lineBackStack.Count == 0)
        {
            return;
        }

        GameObject deleteObj = lineBackStack.Pop();
        //Debug.Log("## 스택에서 하나 뺴기 : " + deleteObj);

        Destroy(deleteObj);

        GameObject ChangedColor = checkBack.Pop();
        //ChangedColor = checkBack.Pop();
        ChangedColor.GetComponent<Vertex>().BackOriginalColor();
        //Debug.Log("## 컬러 원래대로 돌려줘");
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
