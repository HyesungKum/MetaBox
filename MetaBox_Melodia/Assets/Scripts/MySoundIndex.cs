using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
[CreateAssetMenu(fileName = "AudioClips", menuName = "Scriptable Object/AudioClips", order = int.MaxValue)]
#endif
public class MySoundIndex : ScriptableObject
{

    [SerializeField] CustomDictionary<int, AudioClip> myClipList = new();
    public CustomDictionary<int, AudioClip> MyClipList { get { return myClipList; } }

}