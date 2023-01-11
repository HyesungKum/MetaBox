using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    bool isGameOver = false;

    bool isMoving = false;
    bool isInventoryOpened = false;

    [SerializeField] Vector2 openPos = new Vector2(7, 0.5f);
    [SerializeField] float movingSpeed = 10f;
    [SerializeField] List<GameObject> playableNoteList = new List<GameObject>();

    Vector2 originPos;
    Vector2 targetPos;

    private void Awake()
    {
        originPos = transform.position;
        GameManager.myDelegateIsGameOver += gameIsOver;
        bringAllNotes();
    }


    private void Update()
    {

        if (isGameOver)
        {
            return;
        }


        if (isMoving)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Time.deltaTime * movingSpeed);

            if (Vector2.Distance(this.transform.position, targetPos) <= 0)
            {
                isMoving = false;
            }
        }

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
            isGameOver = true;
        }
    }



    //Open or close inventory 
    public void OnClickInventory()
    {
        if (isInventoryOpened == false)
        {
            targetPos = openPos;
            isMoving = true;
            isInventoryOpened = true;
        }

        else if (isInventoryOpened == true)
        {

            targetPos = originPos;
            isMoving = true;
            isInventoryOpened = false; ;
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
        note.transform.SetParent(null);
        playableNoteList.Remove(note);

        UiManager.myDelegateUiManager("잘했어요!");
        CheckHowManyNotes();
    }


    void gameIsOver(bool isOver)
    {
        isGameOver = isOver;
    }


    private void OnDisable()
    {
        playableNoteList.Clear();
    }

}
