using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolCP;
using static SheetMusic;

public class SheetMusic : MonoBehaviour
{
    //public delegate void DelegateSheetMusic(GameStatus myStatus);
    //public static DelegateSheetMusic myDelegateSheetMusic;


    List<GameObject> qNoteList = new List<GameObject>();

    bool isGameOver = false;

    void Start()
    {
        checkQNote();

    }


    private void Update()
    {
        if (qNoteList.Count > 0)
            return;

        if (!isGameOver)
        {
            correctedAllNote();
        }
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


    public void QNoteDisabled(GameObject qNote)
    {
        qNoteList.Remove(qNote);
        Debug.Log("Correct!!");
    }



    void correctedAllNote()
    {
        GameManager.Inst.UpdateGameStatus(GameStatus.GetAllQNotes);
        Debug.Log("Success!");
        isGameOver = true;
    }



}