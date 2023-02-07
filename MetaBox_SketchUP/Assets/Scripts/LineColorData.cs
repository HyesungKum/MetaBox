using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Data/ColorData", order = 0)]

public class LineColorData : ScriptableObject
{
    [Header("[Color One]")]
    #region
    [SerializeField] private float colorOneR;
    public float ColorOneR { get { return colorOneR; } set { colorOneR = value; } }
    [SerializeField] private float colorOneG;
    public float ColorOneG { get { return colorOneG; } set { colorOneG = value; } }
    [SerializeField] private float colorOneB;
    public float ColorOneB { get { return colorOneB; } set { colorOneB = value; } }
    [SerializeField] private float colorOneA;
    public float ColorOneA { get { return colorOneA; } set { colorOneA = value; } }
    #endregion

    [Header("[Color Two]")]
    #region
    [SerializeField] private float colorTwoR;
    public float ColorTwoR { get { return colorTwoR; } set { colorTwoR = value; } }
    [SerializeField] private float colorTwoG;
    public float ColorTwoG { get { return colorTwoG; } set { colorTwoG = value; } }
    [SerializeField] private float colorTwoB;
    public float ColorTwoB { get { return colorTwoB; } set { colorTwoB = value; } }

    [SerializeField] private float colorTwoA;
    public float ColorTwoA { get { return colorTwoA; } set { colorTwoA = value; } }
    #endregion

    [Header("[Color Three]")]
    #region
    [SerializeField] private float colorThreeR;
    public float ColorThreeR { get { return colorThreeR; } set { colorThreeR = value; } }

    [SerializeField] private float colorThreeG;
    public float ColorThreeG { get { return colorThreeG; } set { colorThreeG = value; } }

    [SerializeField] private float colorThreeB;
    public float ColorThreeB { get { return colorThreeB; } set { colorThreeB = value; } }

    [SerializeField] private float colorThreeA;
    public float ColorThreeA { get { return colorThreeA; } set { colorThreeA = value; } }

    #endregion

    [Header("[Color Four]")]
    #region
    [SerializeField] private float colorFourR;
    public float ColorFourR { get { return colorFourR; } set { colorFourR = value; } }

    [SerializeField] private float colorFourG;
    public float ColorFourG { get { return colorFourG; } set { colorFourG = value; } }

    [SerializeField] private float colorFourB;
    public float ColorFourB { get { return colorFourB; } set { colorFourB = value; } }

    [SerializeField] private float colorFourA;
    public float ColorFourA { get { return colorFourA; } set { colorFourA = value; } }

    #endregion

    [Header("[Color Five]")]
    #region
    [SerializeField] private float colorFiveR;
    public float ColorFiveR { get { return colorFiveR; } set { colorFiveR = value; } }

    [SerializeField] private float colorFiveG;
    public float ColorFiveG { get { return colorFiveG; } set { colorFiveG = value; } }

    [SerializeField] private float colorFiveB;
    public float ColorFiveB { get { return colorFiveB; } set { colorFiveB = value; } }

    [SerializeField] private float colorFiveA;
    public float ColorFiveA { get { return colorFiveA; } set { colorFiveA = value; } }

    #endregion

    [Header("[Color Six]")]
    #region
    [SerializeField] private float colorSixR;
    public float ColorSixR { get { return colorSixR; } set { colorSixR = value; } }
    [SerializeField] private float colorSixG;
    public float ColorSixG { get { return colorSixG; } set { colorSixG = value; } }
    [SerializeField] private float colorSixB;
    public float ColorSixB { get { return colorSixB; } set { colorSixB = value; } }

    [SerializeField] private float colorSixA;
    public float ColorSixA { get { return colorSixA; } set { colorSixA = value; } }
    #endregion
}
