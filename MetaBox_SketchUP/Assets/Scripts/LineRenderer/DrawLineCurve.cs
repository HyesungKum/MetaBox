using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineCurve : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab = null;
    [SerializeField] GameObject checkObj = null;

    private Camera mainCam;
    private GameObject currentLine;

    Touch myTouch;
    private Vector3 touchPos;
    public Vector3 startPos;
    LineRender linerender = null;

    public List<Vector3> nodePosList;
    public LinkedList<Vector3> nodePosLinkedList;
    public LinkedList<Dictionary<int, Vector3>> nodePosLinkedListDictionary;

    int linerenderPosCount;
    int nodePosCount;

    void Awake()
    {
        mainCam = Camera.main;
        nodePosList = new List<Vector3>();
        nodePosLinkedList = new LinkedList<Vector3>();
        nodePosLinkedListDictionary = new LinkedList<Dictionary<int, Vector3>>();

        nodePosList.Clear();
        nodePosLinkedList.Clear();
    }

    void Start()
    {
        LineRenderer ObjLender = null;
        checkObj.TryGetComponent<LineRenderer>(out ObjLender);
        linerenderPosCount = ObjLender.positionCount;

        Vector3 nodePos;
        for (int i = 0; i < linerenderPosCount; i++)
        {
            nodePos = ObjLender.GetPosition(i);
            nodePosList.Add(nodePos);
            nodePosLinkedList.AddLast(nodePos);
        }

        //Debug.Log(" nodePosLinkedList : " + nodePosLinkedList.Count);
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
                        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                        // == SetParent ºôµå ÇÒ¶§ ²À »©ÀÚ === 
                        currentLine.transform.SetParent(this.transform);

                        startPos = hitInfo.transform.position;
                        //Debug.Log("startPos : " +  startPos);
                        nodePosLinkedList.Find(startPos);
                        
                        //Debug.Log("Find" + nodePosLinkedList.Find(startPos).Value.ToString());
                        //int index = nodePosLinkedList.
                        //Debug.Log("Count : " + startPosCount);
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
                        //startPos = currentLine.transform.position;
                        currentLine.TryGetComponent<LineRender>(out linerender);
                        linerender.SetPosition(0, startPos);
                        linerender.SetPosition(1, touchPos);

                    }
                    else
                    {
                        return;
                    }
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

    RaycastHit2D RayCheck()
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}
