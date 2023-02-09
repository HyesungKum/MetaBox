using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Data/ColorData", order = 0)]

public class LineColorData : ScriptableObject
{
    [Header("[Color One]")]
    [SerializeField] private float colorOneR;
    [SerializeField] private float colorOneG;
    [SerializeField] private float colorOneB;
    [SerializeField] private float colorOneA;
    #region Property
    public float ColorOneR { get { return colorOneR; }}
    public float ColorOneG { get { return colorOneG; }}
    public float ColorOneB { get { return colorOneB; }}
    public float ColorOneA { get { return colorOneA; }}
    #endregion

    [Header("[Color Two]")]
    [SerializeField] private float colorTwoR;
    [SerializeField] private float colorTwoG;
    [SerializeField] private float colorTwoB;
    [SerializeField] private float colorTwoA;
    #region Property
    public float ColorTwoR { get { return colorTwoR; }}
    public float ColorTwoG { get { return colorTwoG; }}
    public float ColorTwoB { get { return colorTwoB; }}
    public float ColorTwoA { get { return colorTwoA; }}
    #endregion

    [Header("[Color Three]")]
    [SerializeField] private float colorThreeR;
    [SerializeField] private float colorThreeG;
    [SerializeField] private float colorThreeB;
    [SerializeField] private float colorThreeA;
    #region Property
    public float ColorThreeR { get { return colorThreeR; }}
    public float ColorThreeG { get { return colorThreeG; }}
    public float ColorThreeB { get { return colorThreeB; }}
    public float ColorThreeA { get { return colorThreeA; }}
    #endregion

    [Header("[Color Four]")]
    [SerializeField] private float colorFourR;
    [SerializeField] private float colorFourG;
    [SerializeField] private float colorFourB;
    [SerializeField] private float colorFourA;
    #region Property
    public float ColorFourR { get { return colorFourR; }}
    public float ColorFourG { get { return colorFourG; }}
    public float ColorFourB { get { return colorFourB; }}
    public float ColorFourA { get { return colorFourA; }}
    #endregion

    [Header("[Color Five]")]
    [SerializeField] private float colorFiveR;
    [SerializeField] private float colorFiveG;
    [SerializeField] private float colorFiveB;
    [SerializeField] private float colorFiveA;
    #region Property
    public float ColorFiveR { get { return colorFiveR; }}
    public float ColorFiveG { get { return colorFiveG; }}
    public float ColorFiveB { get { return colorFiveB; }}
    public float ColorFiveA { get { return colorFiveA; }}
    #endregion

    [Header("[Color Six]")]
    [SerializeField] private float colorSixR;
    [SerializeField] private float colorSixG;
    [SerializeField] private float colorSixB;
    [SerializeField] private float colorSixA;
    #region Property
    public float ColorSixR { get { return colorSixR; }}
    public float ColorSixG { get { return colorSixG; }}
    public float ColorSixB { get { return colorSixB; }}
    public float ColorSixA { get { return colorSixA; }}
    #endregion

    [Header("[Color Seven]")]
    [SerializeField] private float colorSevenR;
    [SerializeField] private float colorSevenG;
    [SerializeField] private float colorSevenB;
    [SerializeField] private float colorSevenA;
    #region Property
    public float ColorSevenR { get { return colorSevenR; }}
    public float ColorSevenG { get { return colorSevenG; }}
    public float ColorSevenB { get { return colorSevenB; }}
    public float ColorSevenA { get { return colorSevenA; }}
    #endregion

    [Header("[Color Eight]")]
    [SerializeField] private float colorEightR;
    [SerializeField] private float colorEightG;
    [SerializeField] private float colorEightB;
    [SerializeField] private float colorEightA;
    #region Property
    public float ColorEightR { get { return colorEightR; } }
    public float ColorEightG { get { return colorEightG; } }
    public float ColorEightB { get { return colorEightB; } }
    public float ColorEightA { get { return colorEightA; } }
    #endregion

    [Header("[Color Nine]")]
    [SerializeField] private float colorNineR;
    [SerializeField] private float colorNineG;
    [SerializeField] private float colorNineB;
    [SerializeField] private float colorNineA;
    #region Property
    public float ColorNineR { get { return colorNineR; } }
    public float ColorNineG { get { return colorNineG; } }
    public float ColorNineB { get { return colorNineB; } }
    public float ColorNineA { get { return colorNineA; } }
    #endregion

    [Header("[Color Ten]")]
    [SerializeField] private float colorTenR;
    [SerializeField] private float colorTenG;
    [SerializeField] private float colorTenB;
    [SerializeField] private float colorTenA;
    #region Property
    public float ColorTenR { get { return colorTenR; } }
    public float ColorTenG { get { return colorTenG; } }
    public float ColorTenB { get { return colorTenB; } }
    public float ColorTenA { get { return colorTenA; } }
    #endregion
}
