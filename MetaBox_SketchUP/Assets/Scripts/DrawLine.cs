using ObjectPoolCP;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    //===================== referrance object=======================
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] RectTransform instPos = null;
    [SerializeField] Button revertButton = null;
    [SerializeField] Camera mainCam = null;

    //=====================ref vertixes===========================
    [SerializeField] Vertex[] vertices;
    private GameObject handlingLine;

    //====================inner variables==============================

    Touch myTouch;

    private Vector3 startPos;

    public Vertex startVertex = null;

    Stack<GameObject> lineBackStack = null;
    Stack<Vertex> checkVertex = null;

    private int verticesCount = 0;
    //int vCount;

    private void Awake()
    {
        lineBackStack = new Stack<GameObject>();
        checkVertex = new Stack<Vertex>();

        // Revert Button Event
        //revertButton.onClick.AddListener(() => OnClickRevertButton());
    }

    void Start()
    {
        //vCount = 0;
        verticesCount = FindObjectsOfType<Vertex>().Length;
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;

        myTouch = Input.GetTouch(0);

        switch (myTouch.phase)
        {
            #region Began
            case TouchPhase.Began:
                {
                    Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

                    if (hitInfo)
                    {
                        foreach (Vertex vertex in vertices)
                        {
                            if (hitInfo.collider.name == vertex.name)
                            {
                                GameObject instLine = PoolCp.Inst.BringObjectCp(linePrefab);

                                startPos = myTouch.position;
                                instLine.transform.position = startPos;

                                //기타 초기화 구문

                                handlingLine = instLine;
                            }
                        }
                    }
                }
                break;

            #region Move
            case TouchPhase.Moved:
                {
                    //handlingLIne.transform.;//컨트롤
                    //로테이션
                    //handlingLine.
                }
                break;
            #endregion
            #region Ended
            case TouchPhase.Ended:
                {
                    Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

                    if (hitInfo)
                    {
                        //if (hitInfo.collider.name == )

                        //
                        //
                        //


                        startVertex = startVertex.MoveVertex(1);
                        handlingLine = null;//
                                            //스택에다 넣기
                    }
                    else
                    {
                        PoolCp.Inst.DestoryObjectCp(handlingLine); //지우기
                    }
                    break;
                }
                #endregion
        }
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
