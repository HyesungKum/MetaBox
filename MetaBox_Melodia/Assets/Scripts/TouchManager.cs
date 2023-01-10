using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TouchManager;
using static UnityEngine.GraphicsBuffer;

public class TouchManager : MonoBehaviour
{
    public delegate void DelegateTouchManager(GameObject myPos);
    public static DelegateTouchManager myDelegateTouchManager;



    Vector3 touchedToScreen;
    Touch myTouch;

    bool isDragging = false;

    PlayableNote myNote;
    Vector3 myNoteOriginPos;

    //RaycastHit hitPoint;
    RaycastHit2D hitPoint2D;




    // code for demo test ==================================================
    #region
    [SerializeField] bool isTestMode = false;
    [SerializeField] bool createNormalNote = false;
    [SerializeField] bool deleteNote = false;
    [SerializeField] bool createQuizNote = false;

    [SerializeField] Toggle myToggle4NormalNote;
    [SerializeField] Toggle myToggle4QuizNote;
    [SerializeField] Toggle myToggle4DeleteNote;

    [SerializeField] GameObject NormalNote;
    [SerializeField] GameObject quizNote;

    GameObject newNote;
    #endregion
    // code for demo test ==================================================

    private void Awake()
    {

    }




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

        // code for demo test ==================================================
        #region This codes for test
        //if (isTestMode && Input.GetTouch(0).phase == TouchPhase.Began)
        //{

        //    if (createNormalNote)
        //    {
        //        if (hitPoint.collider.name == "SheetMusic")
        //        {
        //            newNote = ObjectPoolCP.PoolCp.Inst.BringObjectCp(NormalNote);

        //            newNote.transform.position = touchedToScreen;

        //            Debug.Log("Create normal note");
        //        }

        //        return;
        //    }


        //    if (createQuizNote)
        //    {
        //        if (hitPoint.collider.name == "SheetMusic")
        //        {
        //            newNote = ObjectPoolCP.PoolCp.Inst.BringObjectCp(quizNote);

        //            newNote.transform.position = touchedToScreen;

        //            Debug.Log("Create quiz note");
        //        }

        //        return;
        //    }


        //    else if (deleteNote)
        //    {
        //        Debug.Log($"บฮผล? {hitPoint.collider.name}");
        //        if (hitPoint.collider.name == "QNote" || hitPoint.collider.name == "Note")
        //        {
        //            ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(shootRay(touchedToScreen).transform.gameObject);
        //            Debug.Log("Destroy!");
        //        }

        //        return;
        //    }
        //}

        #endregion
        // code for demo test ==================================================


    }



    RaycastHit2D shootRay(Vector2 targetT)
    {
        RaycastHit2D hit;
        Ray2D ray;

        ray = new Ray2D(targetT, Vector2.zero);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit;
    }


    RaycastHit2D shootRay(Vector2 targetT, LayerMask myLayerMask)
    {
        RaycastHit2D hit;
        Ray2D ray;

        ray = new Ray2D(targetT, Vector2.zero);
        hit = Physics2D.Raycast(ray.origin, ray.direction, myLayerMask);

        return hit;
    }


    void isMyNote(RaycastHit2D hitPoint)
    {
        // if ray hits playablenote 
        PlayableNote isMyNote = hitPoint.transform.gameObject.GetComponent<PlayableNote>();

        if (isMyNote != null)
        {
            myNote = isMyNote;

            // disable collider 
            hitPoint2D.collider.enabled = false;
            startDragging();
        }
    }


    // is drag stared? 
    void startDragging()
    {
        myNoteOriginPos = myNote.transform.position;
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

        RaycastHit2D hit = shootRay(myNote.transform.position);


        if (!hit)
        {
            myNote.transform.position = myNoteOriginPos;
            hitPoint2D.collider.enabled = true;
        }

        // layer 7 is SheetMusic
        else if (hit.transform.gameObject.layer == 7)
        {
            myDelegateTouchManager(myNote.gameObject);

        }
    }






    // code for demo test ============================================================
    #region this codes for test

    public void OnClickCreateNormalNote()
    {

        createNormalNote = myToggle4NormalNote.isOn;
        isTestMode = myToggle4NormalNote.isOn;

    }



    public void OnClickCreateQuizNote()
    {

        createQuizNote = myToggle4QuizNote.isOn;
        isTestMode = myToggle4QuizNote.isOn;

    }


    public void OnClickDeleteNote()
    {
        deleteNote = myToggle4DeleteNote.isOn;
        isTestMode = myToggle4DeleteNote.isOn;

    }
    #endregion
    // code for demo test ============================================================


}
