using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLine : MonoBehaviour
{
    [SerializeField] protected PitchName myPitchName;
    public PitchName MyPitchName { get { return myPitchName; } set { myPitchName = value; } }


}
