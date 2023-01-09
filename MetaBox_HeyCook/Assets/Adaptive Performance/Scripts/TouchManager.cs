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
    RaycastHit2D hitPoint;

    bool isGameOver = false;
    bool isStarted = false;


    // code for demo test ==================================================
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

    // code for demo test ==================================================



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

        else
        {

            // touch ended => drop object 
            if (isDragging && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                dropObject();
                return;
            }

            touchedToScreen = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

            // ray hit resptect to world space point of touch point
            hitPoint = shootRay(touchedToScreen);

            if (hitPoint == false)
                return;


            // code for demo test ==================================================

            if (isTestMode && Input.GetTouch(0).phase == TouchPhase.Began)
            {

                if (createNormalNote)
                {
                    if (hitPoint.collider.name == "SheetMusic")
                    {
                        newNote = ObjectPoolCP.PoolCp.Inst.BringObjectCp(NormalNote);

                        newNote.transform.position = touchedToScreen;

                        Debug.Log("Create normal note");
                    }

                    return;
                }


                if (createQuizNote)
                {
                    if (hitPoint.collider.name == "SheetMusic")
                    {
                        newNote = ObjectPoolCP.PoolCp.Inst.BringObjectCp(quizNote);

                        newNote.transform.position = touchedToScreen;

                        Debug.Log("Create quiz note");
                    }

                    return;
                }


                else if (deleteNote)
                {
                    Debug.Log($"บฮผล? {hitPoint.collider.name}");
                    if (hitPoint.collider.name == "QNote" || hitPoint.collider.name == "Note")
                    {
                        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(shootRay(touchedToScreen).transform.gameObject);
                        Debug.Log("Destroy!");
                    }

                    return;
                }
            }


            // code for demo test ==================================================


            if (isDragging)
            {
                dragObject();
                return;
            }


            isMyNote(touchedToScreen);
        }
    }




    // raycast where touched 
    RaycastHit2D shootRay(Vector3 targetT)
    {
        RaycastHit2D hit;
        Ray2D ray;

        ray = new Ray2D(targetT, Vector2.zero);
        hit = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        return hit;
    }



    void isMyNote(Vector3 touchPos)
    {

        if (hitPoint)
        {
            PlayableNote isMyNote = hitPoint.transform.gameObject.GetComponent<PlayableNote>();
            if (isMyNote != null)
            {
                myNote = isMyNote;

                isMyNote.SetOriginPos();

                startDragging();
            }
        }
    }

    void startDragging()
    {
        isDragging = true;
    }


    void dragObject()
    {
        myNote.transform.position = touchedToScreen;
    }

    void dropObject()
    {
        isDragging = false;
        myNote.Landed();
    }





    // code for demo test ============================================================


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

}
