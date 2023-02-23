using Kum;
using System;

[Serializable]
public struct GameName
{
    public const string SketchUP = "SketchUP";
    public const string HeyCook = "HeyCook";
    public const string Melodia = "Melodia";
    public const string Freeze = "Freeze";
}

public class GameManager : MonoSingleTon<GameManager>
{
    private new void Awake()
    {
        SoundManager.Inst.SetBGM("VillageBgm");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();
    }
}
