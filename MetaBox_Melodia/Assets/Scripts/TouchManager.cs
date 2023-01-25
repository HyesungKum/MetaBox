using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    Vector3 touchedToScreen;
    Touch myTouch;

    bool isDragging = false;

    PlayableNote myNote;
    Vector3 myNoteOriginPos;

    RaycastHit2D hitPoint2D;


    private void Update()
    {
        if (Input.touchCount <= 0)
            return;

        myTouch = Input.GetTouch(0);

        // if touched UI, no action required
        if (EventSystem.current.IsPointerOverGameObject(myTouch.fingerId))
        {
            return;
        }


        touchedToScreen = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));


        // touch ended => drop object 
        if (isDragging && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            dropObject();
            return;
        }

        if (isDragging)
        {
            dragObject();
            return;
        }

        // ray hit resptect to world space point of touch point
        hitPoint2D = shootRay(touchedToScreen);

        if (!hitPoint2D)
            return;

        isMyNote(hitPoint2D);

    }



    RaycastHit2D shootRay(Vector2 targetT)
    {
        RaycastHit2D hit;
        Ray2D ray;

        ray = new Ray2D(targetT, Vector2.zero);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit;
    }


    void isMyNote(RaycastHit2D hitPoint)
    {
        // if ray hits playablenote 
        hitPoint.transform.gameObject.TryGetComponent(out PlayableNote target);

        if (target != null)
        {
            myNote = target;

            // disable collider 
            hitPoint2D.collider.enabled = false;
            startDragging();
        }
    }


    // is drag stared? 
    void startDragging()
    {
        //myNoteOriginPos = myNote.transform.position;
        myNote.StartToMove();
        isDragging = true;
    }


    // is dragging object? 
    void dragObject()
    {
        myNote.transform.position = touchedToScreen;
    }

    // is object dropped 
    void dropObject()
    {
        isDragging = false;

        myNote.Dropped();
    }

}
