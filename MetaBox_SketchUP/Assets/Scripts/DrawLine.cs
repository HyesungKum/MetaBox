using ObjectPoolCP;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class DrawLine : MonoBehaviour
{
    //===================== referrance object====================
    //[SerializeField] Camera mainCam = null;
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instLineTransform = null;
    [SerializeField] Button revertButton = null;

    //=====================ref vertixes===========================
    [SerializeField] Vertex[] ObjOneVertiexs = null;
    [SerializeField] Vertex[] ObjTwoVertiexs = null;
    [SerializeField] Vertex[] ObjThreeVertiexs = null;

    private GameObject handlingLine;

    //====================inner variables=========================
    GameObject instLine = null;
    Touch myTouch;
    private Vector3 startPos;
    public Vertex startVertex = null;
    public Vertex temp = null;
    public Vertex endVertex = null;


    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;

    bool isMoveEnd = false;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        // Revert Button Event
        //revertButton.onClick.AddListener(() => OnClickRevertButton());
    }

    void Start()
    {
        verticesCount = FindObjectsOfType<Vertex>().Length;
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);

        Vertex collisionVertex = null;


        switch (myTouch.phase)
        {
            #region Began
            case TouchPhase.Began:
                {
                    isMoveEnd = false;

                    TouchBeganCheck(out collisionVertex);
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    isMoveEnd = false;

                    RaycastHit2D hitInfo = RayCheck(myTouch);

                    if (hitInfo)
                    {
                        StrethchLine(instLine);
                        isMoveEnd = true;
                    }
                    else
                    {
                        StrethchLine(instLine);
                        isMoveEnd = true;
                    }
                }
                break;
            #endregion

            #region Ended
            case TouchPhase.Ended:
                {
                    RaycastHit2D hitInfo = RayCheck(myTouch);

                    if (hitInfo)
                    {

                    }
                    else if (isMoveEnd)
                    {
                        DestroyLine(instLine);
                        Debug.Log("ªË¡¶ µ ");
                    }
                    break;
                    #endregion
                }

        }
    }

    void TouchBeganCheck(out Vertex collisionVertex)
    {
        collisionVertex = null;

        RaycastHit2D hitInfo = RayCheck(myTouch);

        if (hitInfo)
        {
            instLine = InstLine();
            LineTransformReset(instLine);

            if (hitInfo.transform.TryGetComponent<Vertex>(out collisionVertex) == true)
            {
                if (startVertex == null)
                {
                    startVertex = collisionVertex;
                    
                }
                else if (collisionVertex.GetNodeName().CompareTo(startVertex.GetNodeName()) != 0)
                {
                    //ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
                    //DestroyLine(instLine);
                    Debug.Log("## 1 Pool ªË¡¶");
                }
            }
        }
    }



    GameObject InstLine()
    {
        GameObject instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
        instLine.transform.SetParent(instLineTransform);

        startPos = myTouch.position;
        instLine.transform.position = startPos;
        //Debug.Log("StartPos : " + startPos);

        return instLine;
    }

    void DestroyLine(GameObject line)
    {
        if (line != null)
        {
            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(instLine);
        }
    }

    // Line Transform Resetting
    public void LineTransformReset(GameObject line)
    {
        if (line != null)
        {
            line.transform.localScale = new Vector3(1f, 1f, 0f);
            line.transform.localRotation = Quaternion.identity;
        }
    }

    public void StrethchLine(GameObject line)
    {
        if (line != null)
        {
            line.transform.localScale = new Vector2(Vector3.Distance(myTouch.position, startPos), 1);
            line.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myTouch.position));
        }
    }

    // RayCastHit
    RaycastHit2D RayCheck(Touch myTouch)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }

    public void OnClickRevertButton()
    {
        if (checkVertex.Count == 0) return;

        Vertex Changed = checkVertex.Pop();
        Changed.BackOriginalColor();
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
