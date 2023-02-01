using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomQuestion : MonoBehaviour
{
    [SerializeField] Button RevertBut = null;
    [SerializeField] GameObject lineRenderPrefab = null;
    [SerializeField] GameObject circlePointPrefab = null;

    GameObject addPoint = null;
    GameObject line = null;
    BoxCollider2D collider = null;

    [SerializeField] private Vector3 pointPos;

    private Vector3 startPos;

    Touch myTouch;
    private Vector3 touchPos;
    private Camera mainCam = null;

    //GameObject creatPrefab = null;
    public List<GameObject> nodeList;
    Stack<GameObject> nodeStack;

    int linerenderPosCount;
    LineRender linerender = null;

    void Awake()
    {
        mainCam = Camera.main;
        //creatPrefab = new GameObject("CreatPrefab");
        nodeList = new List<GameObject>();
        nodeStack = new Stack<GameObject>();

        nodeList.Clear();
        lineRenderPrefab.transform.GetChild(0).TryGetComponent<BoxCollider2D>(out collider);

        RevertBut.onClick.AddListener(delegate { OnClickRevertBut(); });
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);

        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        switch (myTouch.phase)
        {
            #region Began
            case TouchPhase.Began:
                {
                    RaycastHit2D hitInfo = RayCheck(myTouch);
                    if (hitInfo)
                    {
                        //Debug.Log("collider name : " + hitInfo.collider.name);
                        TouchBegan();
                    }
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {


                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {

                }
                break;
                #endregion
        }
    }

    void TouchBegan()
    {
        if (addPoint == null)
        {
            TouchInstNode(touchPos);

            startPos = addPoint.transform.position;
            line = ObjectPoolCP.PoolCp.Inst.BringObjectCp(lineRenderPrefab);
            //line.transform.SetParent(creatPrefab.transform);
        }
        else
        {
            TouchInstNode(touchPos);
            addPoint.transform.SetParent(line.transform);
            startPos = addPoint.transform.position;

            line.TryGetComponent<LineRender>(out linerender);
            linerender.SetCurvePosition(startPos);
            linerenderPosCount = linerender.GetPositionCount();
            nodeList.Add(addPoint);
        }

        Debug.Log("nodeList Count : " + nodeList.Count);
    }

    void OnClickRevertBut()
    {
        linerenderPosCount = linerender.SetPositionCountDown();
        if (linerenderPosCount < 0) return;

        DestroyNode();
    }

    void DestroyNode()
    {
        if (nodeStack.Count == 0) return;
        else
        {
            addPoint = nodeStack.Pop();
        }

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(addPoint);
        nodeList.RemoveAt(nodeList.Count - 1);
    }

    void TouchInstNode(Vector3 linePos)
    {
        addPoint = ObjectPoolCP.PoolCp.Inst.BringObjectCp(circlePointPrefab);
        //addPoint.transform.SetParent(creatPrefab.transform);
        //addPoint.transform.SetParent(line.transform);

        addPoint.transform.position = linePos;
        nodeStack.Push(addPoint);
        //nodeList.Add(addPoint);
    }
    RaycastHit2D RayCheck(Touch myTouch)
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}
