using System.Collections;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public enum ColorControll
{
    CURVE,
    LERP
}

[Serializable]
public class DynamicObj : MonoBehaviour
{
    delegate void DoDynamic();

    //===========================components==============================
    private TextMeshProUGUI textMesh;
    private SpriteRenderer renderer;

    //===================Position Production Controll====================
    public bool editXpos;
    [SerializeField] public AnimationCurve xCurve;
    [Space]
    public bool editYpos;
    [SerializeField] public AnimationCurve yCurve;
    [Space]

    //====================Scale Production Controll======================
    public bool editScale;
    [SerializeField] public AnimationCurve scaleCurve;

    //===================Position Production Controll====================
    [Space]
    public bool editColor;
    public ColorControll colorControll;

    //color curve
    [SerializeField] public AnimationCurve colorRCurve;
    [SerializeField] public AnimationCurve colorGCurve;
    [SerializeField] public AnimationCurve colorBCurve;
    [SerializeField] public AnimationCurve colorACurve;

    //between color lerp
    [SerializeField] public float lerpTime;
    [SerializeField] public Color startColor;
    [SerializeField] public Color endColor;
    private float lerpCal;

    //===========================init state==============================
    private Vector3 tempVec;
    private Vector3 tempScale;

    private Color tempCol;

    //==========================fixed state==============================
    private Vector3 fixedPos;
    private Vector3 fixedScale;

    private Color fixedCol;

    //==========================inner variables==========================
    private DoDynamic doDynamic;
    float timer = 0;

    private void OnEnable()
    {
        Initializing();
        
        //local delegate chain
        if (editXpos) doDynamic += ChangeX;
        if (editYpos) doDynamic += ChangeY;
        if (editScale) doDynamic += ChangeScale;
        if (editColor && colorControll == ColorControll.CURVE) doDynamic += ChangeCurveColor;
        if (editColor && colorControll == ColorControll.LERP) doDynamic += ChangeLerpColor;

        StartCoroutine(nameof(Production));
    }
    private void OnDisable()
    {
        //local delegate unchain
        if (editXpos) doDynamic -= ChangeX;
        if (editYpos) doDynamic -= ChangeY;
        if (editScale) doDynamic -= ChangeScale;
        if (editColor) doDynamic -= ChangeCurveColor;
        if (editColor && colorControll == ColorControll.CURVE) doDynamic -= ChangeCurveColor;
        if (editColor && colorControll == ColorControll.LERP) doDynamic -= ChangeLerpColor;
    }

    private void Initializing()
    {
        //textMeshProUGUI
        TryGetComponent<TextMeshProUGUI>(out textMesh);
        TryGetComponent<SpriteRenderer>(out renderer);

        //position
        tempVec = fixedPos = this.transform.position;

        //scale
        tempScale = fixedScale = this.transform.localScale;

        //color
        if (textMesh != null) fixedCol = textMesh.color;
        if (renderer != null) fixedCol = renderer.color;
        lerpCal = 0f;

        //timer
        timer = 0;
    }

    //================================Production============================================
    IEnumerator Production()
    {
        while (this.gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            doDynamic?.Invoke();

            this.transform.localPosition = fixedPos;
            this.transform.localScale = fixedScale;

            if (textMesh != null) textMesh.color = fixedCol;
            if (renderer != null) renderer.color = fixedCol;

            yield return null;
        }
    }

    //===============================Move Function Connect===================================
    void ChangeX()
    {
        fixedPos.x = xCurve.Evaluate(timer);
    }
    void ChangeY()
    {
        fixedPos.y = yCurve.Evaluate(timer);
    }
    void ChangeScale()
    {
        fixedScale = Vector3.one * scaleCurve.Evaluate(timer);
    }
    void ChangeCurveColor()
    {
        fixedCol = new Color(colorRCurve.Evaluate(timer), colorGCurve.Evaluate(timer), colorBCurve.Evaluate(timer), colorACurve.Evaluate(timer));
    }
    void ChangeLerpColor()
    {
        if (lerpCal >= 1) return;

        fixedCol = Color.Lerp(startColor, endColor, lerpCal);
        lerpCal += Time.deltaTime / lerpTime;
    }
}