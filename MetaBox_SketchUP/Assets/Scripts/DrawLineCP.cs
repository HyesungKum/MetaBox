using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineCP : MonoBehaviour
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

    GameObject newLine = null;

    Stack<GameObject> lineBackStack = new Stack<GameObject>();
    Stack<Vertex> checkVertex = new Stack<Vertex>();

    int winCount = 0;

    public GameObject check = null;
    private int verticesCount = 0;
    int vCount;


    //CP===========================================================
    Stack<GameObject> drawSpot = new Stack<GameObject>();
    Stack<GameObject> drewLine = new Stack<GameObject>();

    Vertex lastTouchedVertex = null;


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

        RaycastHit2D touchedObj = RayCheck(TouchPos);

        Vertex collisionVertex = null;



        //================================================================================================


        // if not touched spot 
        if (touchedObj.collider.name.CompareTo("DrowPoint") != 0)
            return;



        // if touched vertex is not same with last touched vertex
        if (touchedObj.transform.GetComponent<Vertex>() != lastTouchedVertex)
        {
            // do something
        }

        lastTouchedVertex = touchedObj.transform.GetComponent<Vertex>();

        // push draw spot in stack 
        drawSpot.Push(touchedObj.transform.gameObject);


        switch (myTouch.phase)
        {
            case TouchPhase.Began:
                {
                    newLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab); // Object Pool
                    newLine.transform.SetParent(instPos.transform); // Inst Line setParent 

                    Debug.Log("생성됨?? " + newLine);

                    drewLine.Push(newLine);

                    startPos = Input.GetTouch(0).position;
                    newLine.transform.position = startPos;
                }
                break;

            case TouchPhase.Moved:
                {
                    drawLine(newLine);
                }
                break;

            case TouchPhase.Ended:
                {

                    // if alreay has the draw point 
                    if (drawSpot.Contains(touchedObj.transform.gameObject))
                    {
                        // destroy line 
                        destroyDrewLine(drewLine.Pop());
                    }


                }
                break;
        }



        if (touchedObj)
        {
            if (touchedObj.collider.name.CompareTo("DrowPoint") == 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)  // Touch Start
                {
                    //instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab); // Object Pool
                    //instLine.transform.SetParent(instPos.transform); // Inst Line setParent 

                    //Debug.Log("생성됨?? " + instLine);

                    //drewLine.Push(instLine);

                    //startPos = Input.GetTouch(0).position;
                    //instLine.transform.position = startPos;


                    if (touchedObj.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
                    {
                        //Debug.Log(" 들어감 ?");
                        if (startVertex == null)
                        {
                            tempVertex = collisionVertex; // Start DrowPoint Save
                            startVertex = collisionVertex;
                            collisionVertex.StartPointColor();

                            checkVertex.Push(collisionVertex);  // drow point Stack
                            Debug.Log("checkVertex 스택 한개 추가 : " + checkVertex.Count);

                            check = startVertex.transform.parent.gameObject; // parent GameObject 
                                                                             //Debug.Log("parent.name : " + check);
                        }
                        else
                        {
                            if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                            {
                                //Debug.Log("collisionVertex.GetNodeName() :" +collisionVertex.GetNodeName());
                                //Debug.Log("startVertex.GetNodeName() : " + startVertex.GetNodeName());

                                // === 오브젝트 풀링으로 오브젝트 삭제 변경 하기
                                Destroy(newLine.gameObject);
                            }
                            else if (newLine != null)
                            {
                                Debug.Log("## 1 삭제 안됨 ");
                                lineBackStack.Push(newLine);  // linStack Stack Add One Object
                                Debug.Log(" 1) lineBackStack 한개 추가" + lineBackStack.Count);
                            }
                        }
                    }
                }


                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //Vertex collisionVertex = null;

                    if (touchedObj.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                    {
                        if (newLine != null)
                        {
                            for (int i = 0; i < startVertex.GetNodeLength(); i++)
                            {
                                if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                                {
                                    collisionVertex.ColorChange(); // Color Changed 

                                    newLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                                    newLine.transform.SetParent(instPos.transform);


                                    checkVertex.Push(collisionVertex);  // drow point Stack
                                    Debug.Log("checkVertex 스택 한개 추가 : " + checkVertex.Count);

                                    startPos = Input.GetTouch(0).position;
                                    newLine.transform.position = startPos;

                                    Vector3 myPos = Input.GetTouch(0).position;

                                    newLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                                    //Debug.Log("localScale : " + instLine.transform.localScale);

                                    // localScale 이 0이면 Destroy
                                    newLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));

                                    startVertex = collisionVertex;

                                    break;
                                }
                            }
                            vCount += 1;
                        }
                    }
                }


                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (startVertex != null)
                    {
                        if (touchedObj.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                        {
                            for (int i = 0; i < startVertex.GetNodeLength(); i++)
                            {
                                if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                                {
                                    return;
                                }
                                else if (i == startVertex.GetNodeLength() - 1)
                                {
                                    // === 오브젝트 풀링으로 오브젝트 삭제 변경 하기
                                    Destroy(newLine.gameObject);

                                }
                                else if (newLine != null)
                                {
                                    Debug.Log("## 2 삭제 안됨 ");
                                    lineBackStack.Push(newLine);  // linStack Stack Add One Object
                                    Debug.Log(" 2) lineBackStack 한개 추가" + lineBackStack.Count);
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

            if (newLine != null)
            {
                newLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                newLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));
            }
        }


        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (newLine != null)
            {
                // === 오브젝트 풀링으로 오브젝트 삭제 변경 하기
                Destroy(newLine.gameObject);

                //if (instLine != null)
                //{
                //    Debug.Log("## 1 삭제 안됨 ");
                //    lineBackStack.Push(instLine);  // linStack Stack Add One Object
                //    Debug.Log(" 3) lineBackStack 한개 추가" + lineBackStack.Count);
                //}
                #region
                //if (endVertex == tempVertex)
                //{
                //    if (clearImg != null)
                //    {
                //        if (check.transform.name == "StarObj1")
                //        {
                //            winCount += 1;
                //            clearImg.ClearIMgObjTwo();
                //        }
                //        else if (check.transform.name == "Obj3")
                //        {
                //            winCount += 1;
                //            clearImg.ClearImgOne();
                //        }
                //        else if (check.transform.name == "Obj2")
                //        {
                //            winCount += 1;
                //            clearImg.ClearIMgObjThree();
                //        }
                //    }
                //}
                #endregion
            }
        }
    }


    public void OnClickPopStack()
    {
        if (lineBackStack.Count == 0)
        {
            lineBackStack.Clear();
            Debug.Log("lineBackStack 스택 비우기");
            return;
        }

        GameObject deleteObj = lineBackStack.Pop();
        Debug.Log("## lineBackStack에서 하나 뺴기 : " + deleteObj);
        Debug.Log("뺴고 카운트 lineBackStack Count : " + lineBackStack.Count);
        Destroy(deleteObj.gameObject);


        //GameObject ChangedColor = checkBack.Pop();
        //Debug.Log("## checkBack 하나 뺴기 : " + ChangedColor);
        //Debug.Log("빼고난뒤 카운트checkBack Count : " + checkBack.Count);        

        if (checkVertex.Count == 0)
        {
            checkVertex.Clear();
            Debug.Log("checkVertex 스택 비우기");
            return;
        }
        Vertex ChangedColor = checkVertex.Pop();
        Debug.Log("## checkBack 하나 뺴기 : " + ChangedColor);
        Debug.Log("빼고난뒤 카운트checkBack Count : " + checkVertex.Count);
        //ChangedColor = checkBack.Pop();
        ChangedColor.GetComponent<Vertex>().BackOriginalColor();
        //Debug.Log("## 컬러 원래대로 돌려줘");
    }




    //============================================================================================
    void destroyDrewLine(GameObject targetLine)
    {

        targetLine.transform.position = Vector2.zero;
        targetLine.transform.localScale = Vector3.one;
        targetLine.transform.localRotation = Quaternion.identity;

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(targetLine);
    }


    void drawLine(GameObject target)
    {
        Vector3 myPos = Input.GetTouch(0).position;

        target.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
        target.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));
    }


    void createLine()
    {
        newLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab); // Object Pool
        newLine.transform.SetParent(instPos.transform); // Inst Line setParent 

        Debug.Log("생성됨?? " + newLine);

        drewLine.Push(newLine);

        startPos = Input.GetTouch(0).position;
        newLine.transform.position = startPos;
    }



    void checkIfCanDraw(Vertex target)
    {
        for (int i = 0; i < lastTouchedVertex.GetNodeLength(); i++)
        {
            if (target.GetNodeName().CompareTo(lastTouchedVertex.GetNextNodeName(i)) == 0)
            {
                target.ColorChange(); // Color Changed 

                newLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                newLine.transform.SetParent(instPos.transform);


                checkVertex.Push(target);  // drow point Stack
                Debug.Log("checkVertex 스택 한개 추가 : " + checkVertex.Count);

                startPos = Input.GetTouch(0).position;
                newLine.transform.position = startPos;

                Vector3 myPos = Input.GetTouch(0).position;

                newLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                //Debug.Log("localScale : " + instLine.transform.localScale);

                // localScale 이 0이면 Destroy
                newLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));

                lastTouchedVertex = target;

                break;
            }
        }
        vCount += 1;


    }


    // RaycastHit
    RaycastHit2D RayCheck(Vector3 touchPos)
    {
        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 5f);
        return hit;
    }

    //============================================================================================



    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}
