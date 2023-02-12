using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LineColorChanged : MonoBehaviour
{
    [Header("[Color Button]")]
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Button colorFour = null;
    [SerializeField] Button colorFive = null;
    [SerializeField] Button colorSix = null;
    [SerializeField] Button colorSeven = null;
    [SerializeField] Button colorEight = null;
    [SerializeField] Button colorNine = null;
    [SerializeField] Button colorTen = null;

    [Header("[AnimationCurve Up and Down]")]
    [SerializeField] AnimationCurve curveMoveUp = null; 
    [SerializeField] AnimationCurve curveMoveDown = null; 
    [SerializeField] private float moveTime = 0.2f;
    public float MoveTime { get { return moveTime; } set { moveTime = value; } }
    public float moveUpHeight = 60f;

    private Color getColor;
    public Color GetColor { get { return getColor; } set { getColor = value; } }
    public LineColorData lineColorData = null;

    void Awake()
    {
        if (lineColorData == null)
            lineColorData = Resources.Load<LineColorData>("Data/ColorRGBData");

        #region color Data Setting
        colorOne.onClick.AddListener(delegate { 
            GetColors(lineColorData.ColorOneR, lineColorData.ColorOneG, lineColorData.ColorOneB, lineColorData.ColorOneA); 
            StartCoroutine(ColorPositionMove(colorOne)); 
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorTwo.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorTwoR, lineColorData.ColorTwoG, lineColorData.ColorTwoB, lineColorData.ColorTwoA);
            StartCoroutine(ColorPositionMove(colorTwo));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorThree.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorThreeR, lineColorData.ColorThreeG, lineColorData.ColorThreeB, lineColorData.ColorThreeA);
            StartCoroutine(ColorPositionMove(colorThree)); 
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorFour.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorFourR, lineColorData.ColorFourG, lineColorData.ColorFourB, lineColorData.ColorFourA);
            StartCoroutine(ColorPositionMove(colorFour));
            SoundManager.Inst.ChangeLineAndColorSFXPlay(); 
        });

        colorFive.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorFiveR, lineColorData.ColorFiveG, lineColorData.ColorFiveB, lineColorData.ColorFiveA);
            StartCoroutine(ColorPositionMove(colorFive));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorSix.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorSixR, lineColorData.ColorSixG, lineColorData.ColorSixB, lineColorData.ColorSixA);
            StartCoroutine(ColorPositionMove(colorSix));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorSeven.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorSevenR, lineColorData.ColorSevenG, lineColorData.ColorSevenB, lineColorData.ColorSevenA);
            StartCoroutine(ColorPositionMove(colorSeven));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorEight.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorEightR, lineColorData.ColorEightG, lineColorData.ColorEightB, lineColorData.ColorEightA);
            StartCoroutine(ColorPositionMove(colorEight));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorNine.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorNineR, lineColorData.ColorNineG, lineColorData.ColorNineB, lineColorData.ColorNineA);
            StartCoroutine(ColorPositionMove(colorNine));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });

        colorTen.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorTenR, lineColorData.ColorTenG, lineColorData.ColorTenB, lineColorData.ColorTenA);
            StartCoroutine(ColorPositionMove(colorTen));
            SoundManager.Inst.ChangeLineAndColorSFXPlay();
        });
        #endregion
    }

    public IEnumerator ColorPositionMove(Button buttonNum)
    {
        Vector3 startPos = buttonNum.transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0, moveUpHeight,0);

        float timer = 0.0f;
        while(timer < MoveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            buttonNum.transform.localPosition = Vector3.Lerp(startPos, targetPos, curveMoveUp.Evaluate(check));
            yield return null;
        }

        timer = 0.0f;
        startPos = buttonNum.transform.localPosition;
        targetPos = startPos - new Vector3(0, moveUpHeight, 0);

        while(timer < MoveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            buttonNum.transform.localPosition = Vector3.Lerp(startPos, targetPos, curveMoveDown.Evaluate(check));
        }
    }

    Color GetColors(float r, float g, float b, float a)
    {
        GetColor = new Color(r / 255, g / 255, b / 255, a / 255);
        return GetColor;
    }
}