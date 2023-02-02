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
                    currentLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                    // ºôµå ÇÒ´ë SetParent »¾±â
                    currentLine.transform.SetParent(this.transform);
                }
                break;
            #endregion

            #region Move
            case TouchPhase.Moved:
                {
                    currentLine.TryGetComponent<LineRender>(out linerender);
                    linerender.SetCurvePosition(touchPos);
                }
                break;
                #endregion
        }
    }
}