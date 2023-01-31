using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomQuestion : MonoBehaviour
{
    [SerializeField] Button addPointBut = null;
    [SerializeField] GameObject lineRenderPrefab = null;
    [SerializeField] GameObject circlePointPrefab = null;

    GameObject addPoint = null;
    GameObject line = null;
    BoxCollider2D collider = null;

    [SerializeField] private Vector3 pointPos;

    [Header("[Circle Size Range]")]
    [SerializeField] bool ramdomCreate = false;
    [SerializeField][Range(0f, 5f)] private float radius = 2f;

    [Header("[Point Random Range]")]
    [SerializeField] bool circleCreate = false;
    [SerializeField][Range(-5f, 0f)] private float ramdomMin = -4.5f;
    [SerializeField][Range(0f, 5f)] private float ramdomMax = 4.5f;

    [SerializeField] private Vector3 startPos;
    Vector3 endPos;

    Touch myTouch;
    private Vector3 touchPos;
    private Camera mainCam = null;
    bool isHaveNewNode = false;

    GameObject creatPrefab = null;
    List<GameObject> nodeList;

    void Awake()
    {
        mainCam = Camera.main;
        creatPrefab = new GameObject("CreatPrefab");
        nodeList = new List<GameObject>();
        nodeList.Clear();
        lineRenderPrefab.transform.GetChild(0).TryGetComponent<BoxCollider2D>(out collider);
        //addPointBut.onClick.AddListener(delegate { OnClickAddPoint(); });
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
                    if (addPoint == null)
                    {
                        addPoint = TouchInstNode(touchPos);
                        nodeList.Add(addPoint);
                        startPos = addPoint.transform.position;
                        line = ObjectPoolCP.PoolCp.Inst.BringObjectCp(lineRenderPrefab);
                        line.transform.SetParent(creatPrefab.transform);
                    }
                    else
                    {
                        addPoint = TouchInstNode(touchPos);
                        nodeList.Add(addPoint);
                        startPos = addPoint.transform.position;

                        LineRender linerender = null;
                        line.TryGetComponent<LineRender>(out linerender);
                        linerender.SetCurvePosition(startPos);

                        Debug.Log("NodeList Count : " + nodeList.Count);
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

    void OnClickAddPoint()
    {
        if (addPoint == null)
        {
            //addPoint = InstRandomNode();
            //line = ObjectPoolCP.PoolCp.Inst.BringObjectCp(lineRenderPrefab);
        }
        else if (addPoint != null)
        {
            startPos = addPoint.transform.position;
            addPoint = InstRandomNode();
            endPos = addPoint.transform.position;

            InsetOneLine(endPos);

            //InsetOneLine(endPos);

            // === 콜라이도 사이즈 조절 ===
            //collider.size = new Vector3(5f, 5f, Vector3.Distance(check, checkPos));
            //Debug.Log(collider.size + " : collider.size");

        }
    }

    GameObject TouchInstNode(Vector3 linePos)
    {
        GameObject instNode = null;
        instNode = ObjectPoolCP.PoolCp.Inst.BringObjectCp(circlePointPrefab);
        instNode.transform.SetParent(creatPrefab.transform);

        instNode.transform.position = linePos;
        return instNode;
    }


    void InstLineRender(Vector3 startPos, Vector3 endPos)
    {
        line = ObjectPoolCP.PoolCp.Inst.BringObjectCp(lineRenderPrefab);

        LineRender linerender = null;
        line.TryGetComponent<LineRender>(out linerender);
        linerender.SetPosition(0, startPos);
        linerender.SetPosition(1, endPos);
    }

    void InsetOneLine(Vector3 endPos)
    {
        LineRender linerender = null;
        line.TryGetComponent<LineRender>(out linerender);
        linerender.SetCurvePosition(endPos);
    }


    void LineCollider()
    {

    }


    GameObject InstRandomNode()
    {
        float randomX = Random.Range(ramdomMin, ramdomMax);
        float randomY = Random.Range(ramdomMin, ramdomMax);

        GameObject instNode = null;
        instNode = ObjectPoolCP.PoolCp.Inst.BringObjectCp(circlePointPrefab);
        pointPos = new Vector3(randomX, randomY, 0);

        instNode.transform.position = pointPos;

        return instNode;
    }

    GameObject CircleInst()
    {
        pointPos.x = radius * Mathf.Cos(Time.time * 360 * Mathf.Deg2Rad);
        pointPos.y = radius * Mathf.Sin(Time.time * 360 * Mathf.Deg2Rad);
        pointPos.z = 0;

        GameObject instNode = null;
        instNode = ObjectPoolCP.PoolCp.Inst.BringObjectCp(circlePointPrefab);
        instNode.transform.position = pointPos;
        return instNode;
    }
}
