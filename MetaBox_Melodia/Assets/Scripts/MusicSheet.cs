using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;


public class MusicSheet : MonoBehaviour
{
    //  Quiz Note List 
    [SerializeField] List<GameObject> qNoteList = new();

    // Normal Note List 
    [SerializeField] List<GameObject> noteList = new();

    // Sound Line List 
    [SerializeField] List<Transform> mySoundLines = new();

    [SerializeField] GameObject qNotePrefab;
    [SerializeField] GameObject normalNotePrefab;

    Dictionary<int, List<int>> myStageData = new();
    public Dictionary<int, List<int>> MyStageData { get { return myStageData; } set { myStageData = value; } }



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
                    // clear lists 
                    ReadyGame();
                }
                break;

            case GameStatus.Ready:
                {
                    // ask game manager for current stage number
                    int curStage = GameManager.Inst.CurStage;

                    // get stage data from game manager 
                    MyStageData = GameManager.Inst.CurStageInfo();

                    // generate notes 
                    buildStage(curStage);

                }
                break;

        }
    }




    public void ReadyGame()
    {

        if (noteList.Count > 0)
        {
            for (int i = 0; i < noteList.Count; ++i)
            {
                PoolCp.Inst.DestoryObjectCp(noteList[i]);
            }
        }

        noteList.Clear();

        if (qNoteList.Count > 0)
        {
            for (int i = 0; i < qNoteList.Count; ++i)
            {
                if (qNoteList.Count > 0)
                {
                    PoolCp.Inst.DestoryObjectCp(qNoteList[i]);
                }
            }
        }

        qNoteList.Clear();

    }


    private void OnDisable()
    {
        noteList.Clear();
        qNoteList.Clear();
    }


    void buildStage(int stage)
    {
        // total length of music sheet == 27 (-12.5 ~ 15.5)

        // A number of notes of current stage  
        int noteIdx = myStageData[stage].Count;

        // how many empty note ? <= should get from data sheet 
        int emptyNote = stage / 3 + 2;

        // note generate from this position 
        float xPos = -12.5f;

        // get average distance between notes 
        float distance = 27f / noteIdx;

        List<int> emptyNoteIdx = new();


        // get random index of note to hide to use as quiz note 
        for (int i = 0; i < emptyNote; ++i)
        {
            int temp;

            // prevent duplication 
            do
            {
                temp = Random.Range(1, noteIdx);

            } while (emptyNoteIdx.Contains(temp));

            emptyNoteIdx.Add(temp);
        }

        // generate notes 
        for (int idx = 0; idx < noteIdx; ++idx)
        {
            GameObject newNote;
            GameObject prefab = normalNotePrefab;

            // if it's qnote index, change prefab 
            if (emptyNoteIdx.Contains(idx))
            {
                prefab = qNotePrefab;
            }

            int note = myStageData[stage][idx] % 100;

            newNote = PoolCp.Inst.BringObjectCp(prefab);

            newNote.transform.position = new Vector2(xPos, mySoundLines[note].position.y);

            // set note info
            newNote.TryGetComponent<QNote>(out QNote targetQNote);
            targetQNote.MyPitchNum = myStageData[stage][idx];

            // if code runs in unity editor, ===========================
#if UNITY_EDITOR

            newNote.transform.SetParent(this.transform);
#endif
            // if code runs in unity editor, ===========================


            if (emptyNoteIdx.Contains(idx))
            {
                qNoteList.Add(newNote);
            }

            else
            {
                noteList.Add(newNote);
            }


            xPos += distance;
        }

    }



    // check if playable note is match with Qnote
    public void CheckPlayableNotePos(Transform target)
    {
        //// get note's script
        target.TryGetComponent<PlayableNote>(out PlayableNote myPlayableNote);


        // check if playable note touched Qnote
        foreach (GameObject note in qNoteList)
        {
            // check position and compare with QNote position 
            if (Mathf.Abs(note.transform.position.x - target.transform.position.x) < 1f)
            {
                if (Mathf.Abs(note.transform.position.y - target.transform.position.y) < 0.3f)
                {

                    // move playable note torwards to Qnote
                    myPlayableNote.MoveNote(note.transform.position, 3f);

                    // remove Qnote from list
                    qNoteList.Remove(note);
                    noteList.Add(note);

                    UiManager.myDelegateUiManager("잘했어요!");

                    note.TryGetComponent<QNote>(out QNote myQNote);

                    SoundManager.Inst.PlayNote(myQNote.MyPitchNum, 1);

                    // check how many Qnotes are left
                    if (qNoteList.Count == 0)
                    {
                        getAllQNotes();
                        return;
                    }

                    // if Qnote remains more than 0
                    myPlayableNote.UseNote();

                    return;
                }
            }
        }


        UiManager.myDelegateUiManager("다시 생각해봐요");

        // no this is not an answer note 
        // check if toucched soundline
        isThisTouchedSoundLine(target);


        // is not touched Qnote, destroy it. 
        myPlayableNote.DestroyNote();
    }


    void isThisTouchedSoundLine(Transform target)
    {
        float closestDistance = float.MaxValue;
        Transform closestSoundLine = null;

        foreach (Transform line in mySoundLines)
        {
            float tempDistance = Mathf.Abs(line.transform.position.y - target.transform.position.y);

            if (tempDistance < 0.8f)
            {

                if (tempDistance < closestDistance)
                {
                    closestDistance = tempDistance;     // closest distance 
                    closestSoundLine = line;            // cloesest line
                }
            }
        }

        if (closestSoundLine == null)
        { return; }

        closestSoundLine.TryGetComponent<SoundLine>(out SoundLine targetLine);

        SoundManager.Inst.PlayNote(targetLine.MyPitchNum, 1);
    }



    void getAllQNotes()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.GetAllQNotes);
    }

}