using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QNote : MonoBehaviour
{
    

    [SerializeField] protected PitchName myPitchName;
    public PitchName MyPitchName { get { return myPitchName; } set { myPitchName = value; } }


    SheetMusic mySheetMusic;

    private void Awake()
    {
        mySheetMusic = this.GetComponentInParent<SheetMusic>();
    }


}
