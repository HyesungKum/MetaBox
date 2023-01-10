using System.Collections;
using System.Collections.Generic;
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
    }

    private void Start()
    {
        checkQNote();
    }



    private void Update()
    {

        if (isGameOver)
            return;


        if (playableNoteList.Count <= 0)
        {
            GameManager.Inst.UpdateGameStatus(GameStatus.NoMorePlayableNote);
            isGameOver = true;
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




    void checkQNote()
    {
        // get all child 
        foreach (Transform note in this.transform.GetComponentInChildren<Transform>())
        {
            if (note.name == "PlayableNote")
            {
                playableNoteList.Add(note.gameObject);
            }
        }
    }



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


    public void DestoyedPlayableNote(GameObject Note)
    {
        playableNoteList.Remove(Note);
        Note.transform.parent = null;
        Debug.Log("Good bye!!");
    }




}
