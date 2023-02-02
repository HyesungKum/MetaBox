using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.PackageManager;
using UnityEngine;

public class DrawLineCurve : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab = null;
    [Header("[Object Prefabs]")]
    [SerializeField] GameObject checkObj = null;

    private Camera mainCam;
    private GameObject currentLine;
    GameObject startNodeObj = null;
    GameObject tempNodeObj = null;
    GameObject endNodeObj = null;

    Touch myTouch;
    private Vector3 touchPos;
    public Vector3 startPos;
    public Vector3 endPos;
    LineRender linerender = null;

    public GameObject[] nodePosArry;
    LinePosCDLinkedList circleObj = null;

    int circleObjCount;
    int nodePosCount;

    GameObject prevObj = null;
    GameObject nextObj = null;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        checkObj.TryGetComponent<LinePosCDLinkedList>(out circleObj);
        circleObjCount = circleObj.circlePointArry.Length;
    }

    void Update()
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);

        switch (myTouch.phase)
        {
            #region Began
            case TouchPhase.Began:
                {
                    RaycastHit2D hitInfo = RayCheck();
                    if (hitInfo)
                    {
                        if (startNodeObj == null)
                        {
                            currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                            // === SetParent 빌드 할때 꼭 빼자 ===
                            currentLine.transform.SetParent(this.transform);
                            currentLine.TryGetComponent<LineRender>(out linerender);

                            startNodeObj = hitInfo.transform.gameObject;
                            circleObj.cdNode = circleObj.cdLinkedList.SearchObj(startNodeObj);
                            startNodeObj = circleObj.cdNode.data.circlePointObj;
                            prevObj = circleObj.cdNode.prev.data.circlePointObj;
                            nextObj = circleObj.cdNode.next.data.circlePointObj;
                            //Debug.Log("@@ 터치 시작(prevObj):" + prevObj);
                            //Debug.Log("@@ 터치 시작(nextObj) : " + nextObj);

                            startPos = startNodeObj.transform.position;
                            linerender.SetPosition(0, startPos);
                            linerender.SetPosition(1, startPos);
                        }
                        else
                        {
                            currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                            // == SetParent 빌드 할때 꼭 빼자 === 
                            currentLine.transform.SetParent(this.transform);
                            currentLine.TryGetComponent<LineRender>(out linerender);

                            startNodeObj = hitInfo.transform.gameObject;
                            //Debug.Log(" ## 다시 클릭 :" + startNodeObj);

                            Debug.Log("$$$$ endNodeObj :  " + endNodeObj);

                            if (startNodeObj == endNodeObj)
                            {
                                circleObj.cdNode = circleObj.cdLinkedList.SearchObj(endNodeObj);
                                startNodeObj = circleObj.cdNode.data.circlePointObj;

                                Debug.Log("#### startNodeObj :  " + startNodeObj);
                                prevObj = circleObj.cdNode.prev.data.circlePointObj;
                                //Debug.Log("$$$$ prevObj :  " + prevObj);
                                nextObj = circleObj.cdNode.next.data.circlePointObj;
                                //Debug.Log("$$$$ nextObj :  " + nextObj);

                                startPos = startNodeObj.transform.position;
                                linerender.SetPosition(0, startPos);
                                linerender.SetPosition(1, startPos);
                            }
                            else
                            {
                                ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                            }
                        }
                    }
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {

                    RaycastHit2D hitInfo = RayCheck();
                    if (hitInfo)
                    {
                        if (hitInfo.transform.gameObject == prevObj)
                        {
                            //Debug.Log(" prevObj");
                            Vector3 check = hitInfo.transform.position;
                            linerender.SetPosition(1, check);
                            endNodeObj = prevObj;
                            Debug.Log("(endNodeObj)prevObj" + endNodeObj);
                        }
                        else if (hitInfo.transform.gameObject == nextObj)
                        {
                            //Debug.Log(" nextObj");
                            Vector3 check = hitInfo.transform.position;
                            linerender.SetPosition(1, check);
                            endNodeObj = nextObj;
                            Debug.Log("(endNodeObj)nextObj" + endNodeObj);
                        }


                    }
                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {
                    //Debug.Log("%%endNodeObj (Ended) %%" + endNodeObj);


                    //currentLine.TryGetComponent<LineRender>(out linerender);
                    //if (linerender.GetPosition(0) == new Vector3(0, 0, 0))
                    //{
                    //    Debug.Log(" ## 1 라인 삭제하라우 ~");
                    //    ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                    //    linerender.SetPosition(0, new Vector3(0, 0, 0));
                    //    linerender.SetPosition(1, new Vector3(0, 0, 0));
                    //}
                    //if (linerender.GetPosition(1) == new Vector3(0, 0, 0))
                    //{
                    //    Debug.Log(" ## 0 라인 삭제하라우 ~");
                    //    ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                    //    linerender.SetPosition(0, new Vector3(0, 0, 0));
                    //    linerender.SetPosition(1, new Vector3(0, 0, 0));

                    //}
                    if (linerender.GetPosition(0) == linerender.GetPosition(1))
                    {
                        Debug.Log(" ## 0 -1 라인 삭제하라우 ~");
                        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(currentLine);
                        //linerender.SetPosition(0, new Vector3(0, 0, 0));
                        //linerender.SetPosition(1, new Vector3(0, 0, 0));
                    }
                    //Debug.Log("(Ended) endNodeObj : " + endNodeObj);
                }
                break;
                #endregion
        }
    }

    RaycastHit2D RayCheck()
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}
