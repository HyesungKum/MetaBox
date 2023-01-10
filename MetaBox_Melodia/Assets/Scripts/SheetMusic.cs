using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolCP;
using static SheetMusic;
using static TouchManager;

public class SheetMusic : MonoBehaviour
{

    [SerializeField] List<GameObject> qNoteList = new List<GameObject>();

    PlayableNote myPlayableNote;

    void Awake()
    {
        checkQNote();
        myDelegateTouchManager = CheckPlayableNotePos;
    }

    void checkQNote()
    {
        // get all child 
        foreach (Transform note in this.transform.GetComponentInChildren<Transform>())
        {
            if (note.name == "QNote")
            {
                qNoteList.Add(note.gameObject);
            }
        }
    }



    //new logic ======================================================================
    public void CheckPlayableNotePos(GameObject target)
    {
        myPlayableNote = target.GetComponent<PlayableNote>();

        foreach (GameObject note in qNoteList)
        {

            if (Vector2.Distance(note.transform.position, target.transform.position) < 0.25f)
            {
                myPlayableNote.MoveNote(note.transform.position, 3f);

                qNoteList.Remove(note);
                checkRemainQNote();

                myPlayableNote.UseNote();

                return;
            }
        }

        // if is not touched Qnote
        myPlayableNote.DestroyNote();
    }


    void checkRemainQNote()
    {
        if (qNoteList.Count > 0)
            return;

        GameManager.Inst.UpdateGameStatus(GameStatus.GetAllQNotes);
        Debug.Log("Success!");
    }



    private void OnDisable()
    {
        qNoteList.Clear();
    }

}