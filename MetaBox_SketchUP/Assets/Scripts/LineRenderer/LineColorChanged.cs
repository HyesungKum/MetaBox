using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineColorChanged : MonoBehaviour
{
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Button colorFour = null;
    [SerializeField] Button colorFive = null;
    //[SerializeField] Button colorSix = null;

    public LineColorData lineColorData;
    
    public Color getColor;

    void Awake()
    {
        colorOne.onClick.AddListener(delegate { GetColorOne();  });
        colorTwo.onClick.AddListener(delegate { GetColorTwo();  });
        colorThree.onClick.AddListener(delegate { GetColorThree(); });
        colorFour.onClick.AddListener(delegate { GetColorFour(); });
        colorFive.onClick.AddListener(delegate { GetColorFive(); });
        //colorSix.onClick.AddListener(delegate { });
    }

    void ColorPosMove(Button colorNum)
    {
        // ========== 애니메이션 커브로 다시 만들자 =============
        Transform rackPos = colorNum.gameObject.transform;
        Transform startPos = rackPos;

        rackPos.localPosition = new Vector3(rackPos.localPosition.x, rackPos.localPosition.y + 60, rackPos.localPosition.z);
        //Debug.Log("rackPos.localPosition : " + rackPos.localPosition);
    }

    Color GetColorOne()
    {
        float r = lineColorData.ColorOneR / 255;
        float g = lineColorData.ColorOneG / 255;
        float b = lineColorData.ColorOneB / 255;
        float a = lineColorData.ColorOneA / 255;
        //Debug.Log("r : "  + r + "g :" + g + "b : " + b + "a : " + a );

        getColor = new Color(r, g, b, a);
        //Debug.Log("getColor(Colorone) : " + getColor);
        return getColor;
    }

    Color GetColorTwo()
    {
        float r = lineColorData.ColorTwoR / 255;
        float g = lineColorData.ColorTwoG / 255;
        float b = lineColorData.ColorTwoB / 255;
        float a = lineColorData.ColorTwoA / 255;
        //Debug.Log("r : " + r + "g :" + g + "b : " + b + "a : " + a);

        getColor = new Color(r, g, b, a);
        //Debug.Log("getColor(ColorTwo) : " + getColor);
        return getColor;
    }

    Color GetColorThree()
    {
        float r = lineColorData.ColorThreeR / 255;
        float g = lineColorData.ColorThreeG / 255;
        float b = lineColorData.ColorThreeB / 255;
        float a = lineColorData.ColorThreeA / 255;
        //Debug.Log("r : " + r + "g :" + g + "b : " + b + "a : " + a);

        getColor = new Color(r, g, b, a);
        //Debug.Log("getColor(ColorThree) : " + getColor);
        return getColor;
    }

    Color GetColorFour() 
    {
        float r = lineColorData.ColorFourR / 255;
        float g = lineColorData.ColorFourG / 255;
        float b = lineColorData.ColorFourB / 255;
        float a = lineColorData.ColorFourA / 255;
        //Debug.Log("r : " + r + "g :" + g + "b : " + b + "a : " + a);

        getColor = new Color(r, g, b, a);
        //Debug.Log("getColor(ColorFour) : " + getColor);
        return getColor;
    }

    Color GetColorFive()
    {
        float r = lineColorData.ColorFiveR / 255;
        float g = lineColorData.ColorFiveG / 255;
        float b = lineColorData.ColorFiveB / 255;
        float a = lineColorData.ColorFiveA / 255;
        //Debug.Log("r : " + r + "g :" + g + "b : " + b + "a : " + a);

        getColor = new Color(r, g, b, a);
        //Debug.Log("getColor(ColorFive) : " + getColor);
        return getColor;
    }

    // Color six
    //Color GetColorSix()
    //{
    //   // float r = lineColorData.ColorSixR / 255;
    //   // float g = lineColorData.ColorSixG / 255;
    //   // float b = lineColorData.ColorSixB / 255;
    //   // float a = lineColorData.ColorSixA / 255;
    //   //
    //   // getColor = new Color(r, g, b, a);
    //   // return getColor;
    //}
}