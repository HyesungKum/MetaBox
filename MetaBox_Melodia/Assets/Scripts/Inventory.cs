
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> playableNoteList = new List<GameObject>();
    [SerializeField] List<GameObject> usedNoteList = new List<GameObject>();
    [SerializeField] GameObject playableNote;

    int PlayableNoteCount;

    private void Awake()
    {
        // observe game status 
        GameManager.Inst.myDelegateGameStatus += curGameStatus;
    }

    void curGameStatus(GameStatus curStatus)
    {
        if(curStatus == GameStatus.Idle) ReadyGame();
    }


    void ReadyGame()
    {
        PlayableNoteCount = GameManager.Inst.MelodiaData.invenNote;

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

    void generatePlayableNote()
    {
        float xPos = -11f;
 
        float distance = 25f / (PlayableNoteCount - 1);


        for (int i = 0; i < PlayableNoteCount; ++i)
        {
            GameObject newNote = PoolCp.Inst.BringObjectCp(playableNote);

            newNote.transform.position = new Vector2(xPos, this.transform.position.y - 0.2f);
            PlayableNote.myDelegatePlayableNote += StatusPlayableNote;
            playableNoteList.Add(newNote);

            xPos += distance;
        }
    }


    public void StatusPlayableNote(GameObject note, bool destory)
    {
        playableNoteList.Remove(note);
        usedNoteList.Add(note);

        if(destory)PoolCp.Inst.DestoryObjectCp(note);


        if (playableNoteList.Count <= 0)
        {
            GameManager.Inst.UpdateCurProcess(GameStatus.Fail);
        }
    }
}
