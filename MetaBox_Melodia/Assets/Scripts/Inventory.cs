using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : ObjectPool<PlayableNote>
{
    [SerializeField] List<PlayableNote> playableNoteList = new List<PlayableNote>();
    [SerializeField] List<PlayableNote> usedNoteList = new List<PlayableNote>();
    [SerializeField] PlayableNote playableNotePref;

    [SerializeField] TextMeshProUGUI noteNumber;

    int PlayableNoteCount;

    private void Awake()
    {
        // observe game status 
        GameManager.Inst.myDelegateGameStatus += curGameStatus;
    }

    public override PlayableNote CreatePool()
    {
        if (playableNotePref == null) playableNotePref = Resources.Load<PlayableNote>(nameof(PlayableNote));
        return playableNotePref;
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
                Release(playableNoteList[i]);
            }

            playableNoteList.Clear();
        }


        if (usedNoteList.Count > 0)
        {
            for (int i = 0; i < usedNoteList.Count; ++i)
            {
                Release(usedNoteList[i]);
            }

            usedNoteList.Clear();
        }

        generatePlayableNote();
    }

    void generatePlayableNote()
    {
        float xPos = 0f;
        //float distance = 25f / (PlayableNoteCount - 1);

        for (int i = 0; i < 5; ++i)
        {
            PlayableNote playableNote = Get();

            playableNote.transform.position = new Vector2(xPos, -8.2f);
            playableNote.myDelegatePlayableNote = StatusPlayableNote;
            playableNoteList.Add(playableNote);

            xPos += 3;
        }

        PlayableNoteCount -= 5;
        noteNumber.text = (PlayableNoteCount + playableNoteList.Count).ToString();
    }


    public void StatusPlayableNote(PlayableNote note, bool destory)
    {
        playableNoteList.Remove(note);

        if(destory) Release(note);
        else usedNoteList.Add(note);

        noteNumber.text = (PlayableNoteCount + playableNoteList.Count).ToString();

        if (playableNoteList.Count <= 0)
        {
            if(PlayableNoteCount >= 5) generatePlayableNote();
            else if(!GameManager.Inst.CurStatus.Equals(GameStatus.GetAllQNotes)) GameManager.Inst.UpdateCurProcess(GameStatus.Fail);
        }
    }

}
