using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AnimalAudioData",menuName ="Data/AnimalAudioData", order = 0)]

public class AnimalAudioData : ScriptableObject
{
    [Header("[Level 1]")]
    [SerializeField] private AudioClip polarbearAudioClip;
    [SerializeField] private AudioClip reindeerAudioClip;
    [SerializeField] private AudioClip penguinAudioClip;

    [Header("[Level 2]")]
    [SerializeField] private AudioClip orcaAudioClip;
    [SerializeField] private AudioClip walrusAudioClip;
    [SerializeField] private AudioClip dolphinAudioClip;

    [Header("[Level 3]")]
    [SerializeField] private AudioClip giraffeAudioClip;
    [SerializeField] private AudioClip elephantAudioClip;
    [SerializeField] private AudioClip cheetahAudioClip;

    [Header("[Level 4]")]
    [SerializeField] private AudioClip tigerAudioClip;
    [SerializeField] private AudioClip deerAudioClip;
    [SerializeField] private AudioClip rabbitAudioClip;

    #region Property
    public AudioClip PolarbearAudioClip { get { return polarbearAudioClip;} set { polarbearAudioClip = value; } }
    public AudioClip ReindeerAudioClip { get { return reindeerAudioClip; } set { reindeerAudioClip = value; } }
    public AudioClip PenguinAudioClip { get { return penguinAudioClip; } set { penguinAudioClip = value; } }

    public AudioClip OrcaAudioClip { get { return orcaAudioClip; } set { orcaAudioClip = value; } }
    public AudioClip WalrusAudioClip { get { return walrusAudioClip; } set { walrusAudioClip = value; } }
    public AudioClip DolphinAudioClip { get { return dolphinAudioClip; } set { dolphinAudioClip = value; } }

    public AudioClip GiraffeAudioClip { get { return giraffeAudioClip; } set { giraffeAudioClip = value; } }
    public AudioClip ElephantAudioClip { get { return elephantAudioClip; } set { elephantAudioClip = value; } }
    public AudioClip CheetahAudioClip { get { return cheetahAudioClip; } set { cheetahAudioClip = value; } }

    public AudioClip TigerAudioClip { get { return tigerAudioClip; } set { tigerAudioClip = value; } }
    public AudioClip DeerAudioClip { get { return deerAudioClip; } set { deerAudioClip = value; } }
    public AudioClip RabbitAudioClip { get { return rabbitAudioClip; } set { rabbitAudioClip = value; } }
    #endregion
}
