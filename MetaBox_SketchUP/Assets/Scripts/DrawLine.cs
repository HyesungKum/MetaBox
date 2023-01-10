using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] GameObject clearImg = null;

    Touch myTouch;
    private Vector3 startPos;
    private Vector3 firstPos;
    private Vector3 endPos;
    private Vertex startVertex = null;
    private Transform startPosCheck;
    private Transform endPosCheck;


    GameObject instLine = null;
    Clear clearImgs = null;

    void Start()
    {
        clearImgs=  gameObject.GetComponent<Clear>();
    }

    void Update()
    {
        if (Input.touchCount <= 0) return; // 입력이 없으면 return

        myTouch = Input.GetTouch(0);
        Vector3 TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        RaycastHit2D objCheck = RayCheck(TouchPos);

        if (objCheck)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)  // Have Touch
            {
                //Debug.Log("닿은 이름이 뭐니 ?? :" + objCheck.collider.name);
                if (objCheck.collider.name.Equals("DrowPoint"))
                {
                    startPosCheck = objCheck.collider.transform.GetChild(0);
                    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, startPosCheck);
                    startPos = Input.GetTouch(0).position;
                    Debug.Log("startPos : " + startPos);
                    firstPos = startPos;
                    Debug.Log("# 1 FirstPos" + firstPos);

                    instLine.transform.position = startPos;
                    Vertex collisionVertex = null;

                    if(objCheck.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
                    {
                        if (startVertex == null)
                        {
                            startVertex = collisionVertex;
                        }
                    }
                }
            }
            else if (objCheck.collider.name.Equals("DrowPoint"))
            {
                if (objCheck && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    startPosCheck = objCheck.transform.GetChild(0);
                    //Debug.Log("## 터치 땟을 때 : " + testStartPos);
                    endPos = objCheck.transform.GetChild(0).position;
                    Debug.Log("endPos :" + endPos);
                    endPosCheck = objCheck.transform.GetChild(0);
                    Debug.Log("endPoscheck : " + endPosCheck);

                    if(firstPos == endPos)
                    {
                        clearImgs.ClearImgOne();
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vertex collisionVertex = null;
                    if (objCheck.transform.gameObject.TryGetComponent<Vertex>(out collisionVertex))
                    {
                        //Debug.Log(startVertex.GetNodeLength());

                        for (int i = 0; i < startVertex.GetNodeLength(); i++)
                        {
                            //Debug.Log("name");
                            //Debug.Log(startVertex.GetNextNodeName(0));
                            //Debug.Log(startVertex.GetNextNodeName(1));

                            if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNextNodeName(i)) == 0)
                            {
                                startPosCheck = objCheck.collider.transform.GetChild(0);
                                instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, startPosCheck);
                                startPos = Input.GetTouch(0).position;
                                instLine.transform.position = startPos;
                                startVertex = collisionVertex;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Destroy(instLine);
                    }
                }
            }
        }

        else if (Input.GetTouch(0).phase == TouchPhase.Moved)  // Touch Move
        {
            Vector3 myPos = Input.GetTouch(0).position;

            if (instLine != null)
            {
                instLine.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
                instLine.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));
            }
        }
    }

    void CleserOne()
    {
        clearImgs.ClearImgOne();

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
