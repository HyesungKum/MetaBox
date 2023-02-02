using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineStraight : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;

    Camera mainCam = null;
    GameObject instLine = null;

    Touch myTouch;
    private Vector2 startTouchPos;
    private Vector2 touchPos;
    private Vector2 endPos;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount <= 0) return;
        myTouch = Input.GetTouch(0);

        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        if (myTouch.phase == TouchPhase.Began)
        {
            instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
            instLine.transform.SetParent(this.transform);

            startTouchPos = touchPos;
        }

        if (myTouch.phase == TouchPhase.Moved)
        {
            Ray2D ray = new Ray2D(touchPos, Vector2.zero);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

            instLine.GetComponent<LineRender>().SetPosition(0, startTouchPos);
            instLine.GetComponent<LineRender>().SetPosition(1, touchPos);
        }

        if (myTouch.phase == TouchPhase.Ended)
        {

        }
    }

    RaycastHit2D RayCheck(Touch myTouch)
    {
        touchPos = mainCam.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hitInfo;
    }
}
