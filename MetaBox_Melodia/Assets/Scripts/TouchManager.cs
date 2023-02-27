using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
    [SerializeField] Camera myCam = null;
    Vector3 touchedToScreen;
    Touch myTouch;
    RaycastHit2D hitPoint2D;
    PlayableNote myNote;

    bool isDragging = false;

    void Update()
    {
        if (Input.touchCount <= 0) return;

        myTouch = Input.GetTouch(0);

        // if touched UI, no action required
        if (EventSystem.current.IsPointerOverGameObject(myTouch.fingerId)) return;

        touchedToScreen = myCam.ScreenToWorldPoint(myTouch.position);

        // touch ended => drop object 
        if (isDragging && myTouch.phase == TouchPhase.Ended)
        {
            dropObject();
            return;
        }

        if (isDragging)
        {
            dragObject();
            return;
        }

        if(myTouch.phase == TouchPhase.Began)
        {
            // ray hit resptect to world space point of touch point
            hitPoint2D = shootRay(touchedToScreen);

            if (!hitPoint2D) return;

            isMyNote(hitPoint2D);
        }
    }



    RaycastHit2D shootRay(Vector2 targetT)
    {
        Ray2D ray = new Ray2D(targetT, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit;
    }


    void isMyNote(RaycastHit2D hitPoint)
    {
        if(hitPoint.transform.gameObject.layer == 6)
        {
            PlayableNote target = null;

            // if ray hits playablenote 
            hitPoint.transform.TryGetComponent(out target);

            if (target != null)
            {
                myNote = target;

                // disable collider 
                hitPoint2D.collider.enabled = false;
                startDragging();
                SoundManager.Inst.SFXPlay(SFX.Flower);
            }
        }
        else
        {
            QNote note = null;

            hitPoint.transform.TryGetComponent(out note);
            if(note != null) SoundManager.Inst.PlayNote(note.MyPitchNum);
        }
    }


    // is drag stared? 
    void startDragging()
    {
        isDragging = true;
        myNote.StartToMove();
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
