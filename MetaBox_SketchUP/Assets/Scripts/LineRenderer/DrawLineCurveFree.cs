using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineCurveFree : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab = null;

    private Camera mainCam;
    private GameObject currentLine;

    Touch myTouch;
    private Vector3 touchPos;
    LineRender linerender = null;

    void Awake()
    {
        mainCam = Camera.main;
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
                    //RaycastHit2D hitInfo = RayCheck();

                    //if (hitInfo)
                    //{
                        //hitInfo.collider.name
                        //Debug.Log("hitInfo collider.name: " + hitInfo.transform.gameObject);
                        currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                        // ºôµå ÇÒ´ë SetParent »©±â
                        currentLine.transform.SetParent(this.transform);
                    

                   //}
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    //RaycastHit2D hitInfo = RayCheck();

                    //if (hitInfo)
                    //{
                        currentLine.TryGetComponent<LineRender>(out linerender);
                        linerender.SetCurvePosition(touchPos);

                   // }
                }
                break;
                #endregion
        }
    }

    RaycastHit2D RayCheck()
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, 
            Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}