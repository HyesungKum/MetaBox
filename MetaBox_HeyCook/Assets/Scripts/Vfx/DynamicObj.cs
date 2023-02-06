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
    private TextMeshProUGUI _textMesh;
    private SpriteRenderer _renderer;

    //===================Position Production Controll====================
    public bool editXpos;
    public AnimationCurve xCurve;
    [Space]
    public bool editYpos;
    public AnimationCurve yCurve;
    [Space]

    //====================Scale Production Controll======================
    public bool editScale;
    public AnimationCurve scaleCurve;

    //===================Position Production Controll====================
    [Space]
    public bool editColor;
    public ColorControll colorControll;

    //color curve
    public AnimationCurve colorRCurve;
    public AnimationCurve colorGCurve;
    public AnimationCurve colorBCurve;
    public AnimationCurve colorACurve;

    //between color lerp
    public float lerpTime;
    public Color startColor;
    public Color endColor;
    private float lerpCal;

    //=========================influenced about frame=====================
    [Space]
    public bool DonCareTime;

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
        TryGetComponent(out _textMesh);
        TryGetComponent(out _renderer);

        //position
        tempVec = fixedPos = this.transform.localPosition;

        //scale
        tempScale = fixedScale = this.transform.localScale;

        //color
        if (_textMesh != null) fixedCol = _textMesh.color;
        if (_renderer != null) fixedCol = _renderer.color;
        lerpCal = 0f;

        //timer
        timer = 0;
    }

    //================================Production============================================
    IEnumerator Production()
    {
        while (this.gameObject.activeSelf)
        {
            if (DonCareTime) timer += Time.fixedDeltaTime;
            else timer += Time.deltaTime;

            this.transform.localPosition = fixedPos;
            this.transform.localScale = fixedScale;

            doDynamic?.Invoke();

            if (_textMesh != null) _textMesh.color = fixedCol;
            if (_renderer != null) _renderer.color = fixedCol;

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