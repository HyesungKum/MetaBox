using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "Data/SFXData", order = 0)]
public class SFXData : ScriptableObject
{
    [Header("[InGame Audio]")]
    [SerializeField] public AudioClip AlarmClock = null;
    [SerializeField] public AudioClip GameClear = null;
    [SerializeField] public AudioClip GameLose = null;
    [SerializeField] public AudioClip StageClear = null;
    [SerializeField] public AudioClip noName = null;

    [Header("[Line Audio]")]
    [SerializeField] public AudioClip ChangeLineAndColor = null;
    [SerializeField] public AudioClip ConnectLine = null;
}
