using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/SoundData", fileName = "SoundData")]
public class SoundData : ScriptableObject
{
    public CustomDictionary<string, AudioClip> clips;
}

