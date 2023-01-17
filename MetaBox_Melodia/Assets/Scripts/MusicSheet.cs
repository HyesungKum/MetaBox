using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolCP;
using UnityEditor.Experimental.GraphView;

public class MusicSheet : MonoBehaviour
{
    [SerializeField] List<GameObject> qNoteList = new List<GameObject>();
    [SerializeField] List<GameObject> mySoundLines = new List<GameObject>();


    void Awake()
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
            if (Vector2.Distance(note.transform.position, target.transform.position) < 0.1f)
            {
                // move playable note torwards to Qnote
                myPlayableNote.MoveNote(note.transform.position, 3f);

                // remove Qnote from list
                qNoteList.Remove(note);

                // check how many Qnotes are left
                if (qNoteList.Count == 0)
                {
                    gameIsOver();
                    return;
                }

                // if Qnote remains more than 0
                myPlayableNote.UseNote();

                SoundManager.Inst.PlayNote(note.GetComponent<QNote>().MyPitchName);

                return;
            }
        }


        // no this is not an answer note 
        // check if toucched soundline
        isThisTouchedSoundLine(target);


        // is not touched Qnote, destroy it. 
        myPlayableNote.DestroyNote();
    }




    void isThisTouchedSoundLine(GameObject target)
    {
        float closestDistance = float.MaxValue;
        GameObject closestSoundLine = null;

        foreach (GameObject line in mySoundLines)
        {
            float tempDistance = Mathf.Abs(line.transform.position.y - target.transform.position.y);

            if (tempDistance < 0.2f)
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

        Debug.Log($"³ª´Â {closestSoundLine.GetComponent<SoundLine>().MyPitchName} ¾ß");
        SoundManager.Inst.PlayNote(closestSoundLine.GetComponent<SoundLine>().MyPitchName);
    }



    void gameIsOver()
    {
        GameManager.Inst.UpdateGameStatus(GameStatus.GetAllQNotes);
        Debug.Log("Success!");
    }


    private void OnDisable()
    {
        qNoteList.Clear();
    }


}