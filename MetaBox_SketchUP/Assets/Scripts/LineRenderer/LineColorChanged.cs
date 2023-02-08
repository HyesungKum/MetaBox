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
    //[SerializeField] Button colorSix = null;
    
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

        colorOne.onClick.AddListener(delegate { 
            GetColors(lineColorData.ColorOneR, lineColorData.ColorOneG, lineColorData.ColorOneB, lineColorData.ColorOneA); 
            StartCoroutine(ColorPositionMove(colorOne)); });

        colorTwo.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorTwoR, lineColorData.ColorTwoG, lineColorData.ColorTwoB, lineColorData.ColorTwoA);
            StartCoroutine(ColorPositionMove(colorTwo)); });

        colorThree.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorThreeR, lineColorData.ColorThreeG, lineColorData.ColorThreeB, lineColorData.ColorThreeA);
            StartCoroutine(ColorPositionMove(colorThree)); });

        colorFour.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorFourR, lineColorData.ColorFourG, lineColorData.ColorFourB, lineColorData.ColorFourA);
            StartCoroutine(ColorPositionMove(colorFour)); });

        colorFive.onClick.AddListener(delegate {
            GetColors(lineColorData.ColorFiveR, lineColorData.ColorFiveG, lineColorData.ColorFiveB, lineColorData.ColorFiveA);
            StartCoroutine(ColorPositionMove(colorFive)); });

        //colorSix.onClick.AddListener(delegate { });
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
        //Debug.Log("## r : " + GetColor.r + "## g : " +  GetColor.g + "## b : " + GetColor.b + "## a : " + GetColor.a);

        return GetColor;
    }
}