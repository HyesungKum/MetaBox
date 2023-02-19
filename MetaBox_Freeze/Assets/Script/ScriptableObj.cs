using UnityEngine;

[CreateAssetMenu(fileName = "New ScriptableObj", menuName = "ScriptableObjects/ScriptableObj", order = 5)]
public class ScriptableObj : ScriptableObject
{
    public Thief[] NPC;
    public Sprite[] CountDown;
    public AudioClip[] BGM;
    public AudioClip[] SFX;
    
}
