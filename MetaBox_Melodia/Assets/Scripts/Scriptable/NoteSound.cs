using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "NoteAudioClips", menuName = "Scriptable Object/NoteAudio", order = int.MaxValue)]
#endif
public class NoteSound : ScriptableObject
{

    [SerializeField] CustomDictionary<PitchName, AudioClip> myPitchClips = new();
    public CustomDictionary<PitchName, AudioClip> MyPitchClips { get { return myPitchClips; } }

}
