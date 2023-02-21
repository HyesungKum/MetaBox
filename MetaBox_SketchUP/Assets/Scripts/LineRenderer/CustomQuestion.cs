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

    private Vector3 startPos;

    Touch myTouch;
    private Vector3 touchPos;
    private Camera mainCam = null;

    Stack<GameObject> nodeStack;

    int linerenderPosCount;
    LineRender linerender = null;
    EdgeCollider2D edge = null;

    void Awake()
    {
        mainCam = Camera.main;
        nodeStack = new Stack<GameObject>();
        lineRenderPrefab.transform.GetChild(0).TryGetComponent<EdgeCollider2D>(out edge);
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
        }
        else
        {
            TouchInstNode(touchPos);
            addPoint.transform.SetParent(line.transform);
            startPos = addPoint.transform.position;

            line.TryGetComponent<LineRender>(out linerender);
            linerender.SetCurvePosition(startPos);
            linerender.setEdgeCollider();
            linerenderPosCount = linerender.GetPositionCount();
            //nodeList.Add(addPoint);
        }
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
    }

    void TouchInstNode(Vector3 linePos)
    {
        addPoint = ObjectPoolCP.PoolCp.Inst.BringObjectCp(circlePointPrefab);

        addPoint.transform.position = linePos;
        nodeStack.Push(addPoint);
    }

    RaycastHit2D RayCheck(Touch myTouch)
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }

}
