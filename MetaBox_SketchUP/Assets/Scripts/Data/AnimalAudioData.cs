using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalAudioData", menuName = "Data/AnimalAudioData", order = 0)]

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
    public AudioClip PolarbearAudioClip { get { return polarbearAudioClip; } }
    public AudioClip ReindeerAudioClip { get { return reindeerAudioClip; } }
    public AudioClip PenguinAudioClip { get { return penguinAudioClip; } }

    public AudioClip OrcaAudioClip { get { return orcaAudioClip; } }
    public AudioClip WalrusAudioClip { get { return walrusAudioClip; } }
    public AudioClip DolphinAudioClip { get { return dolphinAudioClip; } }

    public AudioClip GiraffeAudioClip { get { return giraffeAudioClip; } }
    public AudioClip ElephantAudioClip { get { return elephantAudioClip; } }
    public AudioClip CheetahAudioClip { get { return cheetahAudioClip; } }

    public AudioClip TigerAudioClip { get { return tigerAudioClip; } }
    public AudioClip DeerAudioClip { get { return deerAudioClip; } }
    public AudioClip RabbitAudioClip { get { return rabbitAudioClip; } }
    #endregion
}
