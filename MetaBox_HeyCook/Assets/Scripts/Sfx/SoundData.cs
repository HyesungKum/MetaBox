using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/SoundData", fileName = "SoundData")]
public class SoundData : ScriptableObject
{
    public CustomDictionary<string, AudioClip> clips;
}