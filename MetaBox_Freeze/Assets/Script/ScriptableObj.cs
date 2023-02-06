using UnityEngine;

[CreateAssetMenu(fileName = "New ScriptableObj", menuName = "ScriptableObjects/ScriptableObj", order = 5)]
public class ScriptableObj : ScriptableObject
{
    public Sprite[] DefaultNPC;
    public Sprite[] CitizenNPC;
    public Sprite[] ThiefNPC;
    public Sprite[] CountDown;
    public AudioClip[] BGM;
    public AudioClip[] SFX;
    
}
