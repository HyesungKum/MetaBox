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
    public float ColorOneR { get { return colorOneR; } set { colorOneR = value; } }
    public float ColorOneG { get { return colorOneG; } set { colorOneG = value; } }
    public float ColorOneB { get { return colorOneB; } set { colorOneB = value; } }
    public float ColorOneA { get { return colorOneA; } set { colorOneA = value; } }
    #endregion

    [Header("[Color Two]")]
    [SerializeField] private float colorTwoR;
    [SerializeField] private float colorTwoG;
    [SerializeField] private float colorTwoB;
    [SerializeField] private float colorTwoA;
    #region Property
    public float ColorTwoR { get { return colorTwoR; } set { colorTwoR = value; } }
    public float ColorTwoG { get { return colorTwoG; } set { colorTwoG = value; } }
    public float ColorTwoB { get { return colorTwoB; } set { colorTwoB = value; } }
    public float ColorTwoA { get { return colorTwoA; } set { colorTwoA = value; } }
    #endregion

    [Header("[Color Three]")]
    [SerializeField] private float colorThreeR;
    [SerializeField] private float colorThreeG;
    [SerializeField] private float colorThreeB;
    [SerializeField] private float colorThreeA;
    #region Property
    public float ColorThreeR { get { return colorThreeR; } set { colorThreeR = value; } }
    public float ColorThreeG { get { return colorThreeG; } set { colorThreeG = value; } }
    public float ColorThreeB { get { return colorThreeB; } set { colorThreeB = value; } }
    public float ColorThreeA { get { return colorThreeA; } set { colorThreeA = value; } }
    #endregion

    [Header("[Color Four]")]
    [SerializeField] private float colorFourR;
    [SerializeField] private float colorFourG;
    [SerializeField] private float colorFourB;
    [SerializeField] private float colorFourA;
    #region Property
    public float ColorFourR { get { return colorFourR; } set { colorFourR = value; } }
    public float ColorFourG { get { return colorFourG; } set { colorFourG = value; } }
    public float ColorFourB { get { return colorFourB; } set { colorFourB = value; } }
    public float ColorFourA { get { return colorFourA; } set { colorFourA = value; } }
    #endregion

    [Header("[Color Five]")]
    [SerializeField] private float colorFiveR;
    [SerializeField] private float colorFiveG;
    [SerializeField] private float colorFiveB;
    [SerializeField] private float colorFiveA;
    #region Property
    public float ColorFiveR { get { return colorFiveR; } set { colorFiveR = value; } }
    public float ColorFiveG { get { return colorFiveG; } set { colorFiveG = value; } }
    public float ColorFiveB { get { return colorFiveB; } set { colorFiveB = value; } }
    public float ColorFiveA { get { return colorFiveA; } set { colorFiveA = value; } }
    #endregion

    [Header("[Color Six]")]
    [SerializeField] private float colorSixR;
    [SerializeField] private float colorSixG;
    [SerializeField] private float colorSixB;
    [SerializeField] private float colorSixA;
    #region Property
    public float ColorSixR { get { return colorSixR; } set { colorSixR = value; } }
    public float ColorSixG { get { return colorSixG; } set { colorSixG = value; } }
    public float ColorSixB { get { return colorSixB; } set { colorSixB = value; } }
    public float ColorSixA { get { return colorSixA; } set { colorSixA = value; } }
    #endregion
}
