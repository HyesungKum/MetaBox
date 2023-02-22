using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;


public class MusicSheet : MonoBehaviour
{
    [SerializeField] GameObject qNotePrefab;
    [SerializeField] GameObject nNotePrefab;

    //  Quiz Note List 
    [SerializeField] List<GameObject> qNoteList = new();

    // Normal Note List 
    [SerializeField] List<GameObject> nNoteList = new();

    // Sound Line List 
    [SerializeField] List<Transform> mySoundLines = new();

    Dictionary<int, List<int>> myStageData = new();

    private void Awake()
    {
        // observe game status 
        GameManager.Inst.myDelegateGameStatus += curGameStatus;
    }
    

    void curGameStatus(GameStatus curStatus)
    {
        if (curStatus == GameStatus.Idle)
        {
            // clear lists 
            ReadyGame();

            // get stage data from game manager 
            myStageData = GameManager.Inst.MyStageData;

            // generate notes 
            buildStage();
        }
    }


    public void ReadyGame()
    {

        if (nNoteList.Count > 0)
        {
            for (int i = 0; i < nNoteList.Count; ++i)
            {
                PoolCp.Inst.DestoryObjectCp(nNoteList[i]);
            }
        }

        nNoteList.Clear();

        if (qNoteList.Count > 0)
        {
            for (int i = 0; i < qNoteList.Count; ++i)
            {
                PoolCp.Inst.DestoryObjectCp(qNoteList[i]);
            }
        }

        qNoteList.Clear();

    }


    void buildStage()
    {
        // ask game manager for current stage number
        int stage = GameManager.Inst.CurStage;

        // A number of notes of current stage  
        int noteIdx = myStageData[stage].Count;

        // note generate from this position
        // total length of music sheet == 26 (-13 ~ 13)
        float xPos = -13f;

        // get average distance between notes 
        float distance = 26f / (noteIdx -1);

        // how many empty note ? <= should get from data sheet 
        int emptyNote = GameManager.Inst.StageDatas[stage - 1].emptyNote;


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
            GameObject prefab;

            if (emptyNoteIdx.Contains(idx)) prefab = qNotePrefab;
            else prefab = nNotePrefab;

            int note = myStageData[stage][idx] % 100;

            newNote = PoolCp.Inst.BringObjectCp(prefab);

            newNote.transform.position = new Vector2(xPos, mySoundLines[note].position.y);

            // set note info
            newNote.TryGetComponent<QNote>(out QNote targetQNote);
            targetQNote.MyPitchNum = myStageData[stage][idx];
            targetQNote.Setting();


            if (emptyNoteIdx.Contains(idx)) qNoteList.Add(newNote);
            else nNoteList.Add(newNote);

            xPos += distance;
        }

    }


    // check if playable note is match with Qnote
    public void CheckPlayableNotePos(PlayableNote target)
    {
        // check if playable note touched Qnote
        foreach (GameObject note in qNoteList)
        {
            // check position and compare with QNote position 
            if (Mathf.Abs(note.transform.position.x - target.transform.position.x) < 1)
            {
                if (Mathf.Abs(note.transform.position.y - target.transform.position.y) < 0.3f)
                {
                    // move playable note torwards to Qnote
                    target.MoveNote(note.transform.position, 3f);

                    // remove Qnote from list
                    qNoteList.Remove(note);
                    nNoteList.Add(note);

                    note.TryGetComponent<QNote>(out QNote myQNote);
                    myQNote.Correct();
                    SoundManager.Inst.PlayNote(myQNote.MyPitchNum);

                    // check how many Qnotes are left
                    if (qNoteList.Count == 0)
                    {
                        SoundManager.Inst.StopMusic();
                        GameManager.Inst.UpdateCurProcess(GameStatus.GetAllQNotes);
                    }

                    //don't destroy
                    target.UseNote(false);

                    return;
                }
            }
        }


        // no this is not an answer note 
        // check if toucched soundline
        isThisTouchedSoundLine(target);


        // is not touched Qnote, destroy it. 
        target.UseNote(true);
    }


    void isThisTouchedSoundLine(PlayableNote target)
    {
        Transform closestSoundLine = null;

        foreach (Transform line in mySoundLines)
        {
            if (Mathf.Abs(line.transform.position.y - target.transform.position.y) < 0.8f)
                closestSoundLine = line;// cloesest line
        }

        if (closestSoundLine == null) return;

        closestSoundLine.TryGetComponent<SoundLine>(out SoundLine targetLine);

        SoundManager.Inst.PlayNote(targetLine.MyPitchNum);
    }

    
}