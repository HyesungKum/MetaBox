using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NoteAudioClips", menuName = "Scriptable Object/NoteClips", order = int.MaxValue)]
public class NoteSoundIndex : ScriptableObject
{

    [SerializeField] CustomDictionary<int, AudioClip> myPitchClips = new();
    public CustomDictionary<int, AudioClip> MyPitchClips { get { return myPitchClips; } }

}
