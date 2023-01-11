using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolCP;

public class SheetMusic : MonoBehaviour
{
    [SerializeField] List<GameObject> qNoteList = new List<GameObject>();

    PlayableNote myPlayableNote;



    void Awake()
    {
        // delegate chain
        TouchManager.myDelegateTouchManager = CheckPlayableNotePos;

        checkQNote();
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

                return;
            }
        }

        // if is not touched Qnote
        myPlayableNote.DestroyNote();
    }


    void checkRemainQNote()
    {
        if (qNoteList.Count > 0)
        {
            myPlayableNote.UseNote();
            return;
        }


        GameManager.Inst.UpdateGameStatus(GameStatus.GetAllQNotes);
        Debug.Log("Success!");
    }


    private void OnDisable()
    {
        qNoteList.Clear();
    }

}