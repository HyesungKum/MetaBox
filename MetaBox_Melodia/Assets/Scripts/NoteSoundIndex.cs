using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
[CreateAssetMenu(fileName = "NoteAudioClips", menuName = "Scriptable Object/NoteClips", order = int.MaxValue)]
#endif
public class NoteSoundIndex : ScriptableObject
{

    [SerializeField] CustomDictionary<int, AudioClip> myPitchClips = new();
    public CustomDictionary<int, AudioClip> MyPitchClips { get { return myPitchClips; } }

}