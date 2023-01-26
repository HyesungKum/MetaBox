using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;


public class MusicSheet : MonoBehaviour
{
    [SerializeField] List<GameObject> qNoteList = new List<GameObject>();
    [SerializeField] List<GameObject> noteList = new List<GameObject>();
    [SerializeField] List<Transform> mySoundLines = new List<Transform>();

    [SerializeField] GameObject QnotePrefab;
    [SerializeField] GameObject NormalnotePrefab;

    static Dictionary<int, List<int>> myStageData = new();
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
                    ReadyGame();
                }
                break;

            case GameStatus.Ready:
                {
                    // ask game manager for current stage number
                    int curStage = GameManager.Inst.CurStage;
                    BuildStage(curStage);

                }
                break;


            case GameStatus.NoMorePlayableNote:
                {
                    // is has fail music, play it 

                }
                break;

        }
    }



    public void GetStageData()
    {
        MyStageData = GameManager.Inst.CurStageInfo();
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


    public void BuildStage(int stage)
    {
        GetStageData();



        int noteIdx = myStageData[stage].Count;
        int emptyNote = stage / 3 + 2;

        float xPos = (noteIdx / 2) * -1.5f;
        //xPos = (noteIdx % 2 == 0) ? xPos += 1f : xPos;


        List<int> emptyNoteIdx = new();

        for (int i = 0; i < emptyNote; ++i)
        {
            int temp;

            do
            {
                temp = Random.Range(1, noteIdx);

            } while (emptyNoteIdx.Contains(temp));

            emptyNoteIdx.Add(temp);
        }

        for (int idx = 0; idx < noteIdx; ++idx)
        {
            GameObject newNote;
            GameObject prefab = NormalnotePrefab;

            if (emptyNoteIdx.Contains(idx))
            {
                prefab = QnotePrefab;
            }


            int note = myStageData[stage][idx] % 100;

            newNote = PoolCp.Inst.BringObjectCp(prefab);

            newNote.transform.position = new Vector2(xPos, mySoundLines[note].position.y);

            // set note info
            newNote.GetComponent<QNote>().MyPitchNum = myStageData[stage][idx];

            newNote.transform.SetParent(this.transform);

            if (emptyNoteIdx.Contains(idx))
            {
                qNoteList.Add(newNote);
            }

            else
            {
                noteList.Add(newNote);
            }


            xPos += 1.5f;
        }

    }



    // check if playable note is match with Qnote
    public void CheckPlayableNotePos(GameObject target)
    {
        //// get note's script

        PlayableNote myPlayableNote;
        myPlayableNote = target.GetComponent<PlayableNote>();


        // check if playable note touched Qnote
        foreach (GameObject note in qNoteList)
        {
            // check position and compare with QNote position 
            //if (Vector2.Distance(note.transform.position, target.transform.position) < 0.15f)


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
                    SoundManager.Inst.PlayNote(note.GetComponent<QNote>().MyPitchNum, 1);

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


    void isThisTouchedSoundLine(GameObject target)
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

        SoundManager.Inst.PlayNote(closestSoundLine.GetComponent<SoundLine>().MyPitchNum, 1);
    }



    void getAllQNotes()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.GetAllQNotes);
    }

}