using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> playableNoteList = new List<GameObject>();


    private void Awake()
    {
        bringAllNotes();
    }


    void bringAllNotes()
    {
        // get all child 
        foreach (Transform note in this.transform.GetComponentInChildren<Transform>())
        {
            if (note.name == "PlayableNote")
            {
                playableNoteList.Add(note.gameObject);

                note.GetComponent<Collider2D>().enabled = true;
            }
        }
    }


    public void CheckHowManyNotes()
    {
        if (playableNoteList.Count <= 0)
        {
            UiManager.myDelegateUiManager("끝났어");
            GameManager.Inst.UpdateGameStatus(GameStatus.NoMorePlayableNote);
        }
    }


    public void DestoyedPlayableNote(GameObject note)
    {
        playableNoteList.Remove(note);

        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(note);
       
        UiManager.myDelegateUiManager("다시 생각해봐요");

        CheckHowManyNotes();
        note.transform.SetParent(null);
    }

    public void UseNote(GameObject note)
    {
        playableNoteList.Remove(note);

        UiManager.myDelegateUiManager("잘했어요!");

        CheckHowManyNotes();
    }


    private void OnDisable()
    {
        playableNoteList.Clear();
    }

}
