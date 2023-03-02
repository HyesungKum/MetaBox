using Kum;
using System;

[Serializable]
public struct GameName
{
    public const string DreamSketch = "DreamSketch";
    public const string HeyCook = "HeyCook";
    public const string Melodia = "Melodia";
    public const string PoliRun = "PoliRun";
}

public class GameManager : MonoSingleTon<GameManager>
{
    public string HeyCookPackageName;
    public string MelodiaPackageName;
    public string PoliRunPackageName;
    public string DreamSketchPackageName;

    private new void Awake()
    {
        SoundManager.Inst.SetBGM("VillageBgm");
        SoundManager.Inst.SetBGMLoop();
        SoundManager.Inst.PlayBGM();
    }
}
