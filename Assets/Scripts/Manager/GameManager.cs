using Kum;
using KumTool.AppTransition;
using System;
using UnityEngine;

[Serializable]
public enum GameName
{
    SketchUP,
    HeyCook,
    Melodia,
    Freeze,
}

public class GameManager : MonoSingleTon<GameManager>
{
    public void ApprochGame(GameName gameName)
    {
        Debug.Log($"com.MetaBox.{gameName}");
        
    }

    private new void Awake()
    {
        SoundManager.Inst.SetBGM("VillageBgm");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();
    }


    public void AppMove()
    {
        //AppTrans.MoveScene($"com.MetaBox.{nameof(gameName)}");
    }
}
