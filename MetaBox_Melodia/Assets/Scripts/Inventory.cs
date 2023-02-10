
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> playableNoteList = new List<GameObject>();
    [SerializeField] List<GameObject> usedNoteList = new List<GameObject>();
    [SerializeField] GameObject playableNote;


    int PlayableNoteCount = 10;


    private void Start()
    {
        // observe game status 
        GameManager.myDelegateGameStatus += curGameStatus;

    }


    void curGameStatus(GameStatus curStatus)
    {
        switch (curStatus)
        {
            case GameStatus.Idle:
                {
                    ReadyGame();
                }
                break;
        }
    }




    void ReadyGame()
    {

        if (playableNoteList.Count > 0)
        {
            for (int i = 0; i < playableNoteList.Count; ++i)
            {
                PoolCp.Inst.DestoryObjectCp(playableNoteList[i]);
            }

            playableNoteList.Clear();
        }


        if (usedNoteList.Count > 0)
        {
            for (int i = 0; i < usedNoteList.Count; ++i)
            {
                PoolCp.Inst.DestoryObjectCp(usedNoteList[i]);
            }

            usedNoteList.Clear();
        }

        PlayableNote.myDelegatePlayableNote = null;
        generatePlayableNote();
    }


    private void OnDisable()
    {
        playableNoteList.Clear();
        usedNoteList.Clear();
    }


    void generatePlayableNote()
    {
        float xPos = -2f * (PlayableNoteCount / 2);


        for (int i = 0; i < PlayableNoteCount; ++i)
        {
            GameObject newNote = PoolCp.Inst.BringObjectCp(playableNote);

            newNote.transform.position = new Vector2(xPos, this.transform.position.y - 0.2f);
            PlayableNote.myDelegatePlayableNote += StatusPlayableNote;
            playableNoteList.Add(newNote);

            xPos += 2f;
        }
    }


    public void StatusPlayableNote(GameObject note, bool destory)
    {
        playableNoteList.Remove(note);
        usedNoteList.Add(note);

        if(destory)PoolCp.Inst.DestoryObjectCp(note);


        if (playableNoteList.Count <= 0)
        {
            GameManager.Inst.UpdateCurProcess(GameStatus.NoMorePlayableNote);
        }
    }
}
