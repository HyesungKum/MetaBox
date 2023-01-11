using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public delegate void DelegateSoundManager(string mySound);
    public static DelegateSoundManager myDelegateSoundManager;



    [SerializeField] AudioClip myClickSound;
    [SerializeField] AudioSource myAudioSource;












}
