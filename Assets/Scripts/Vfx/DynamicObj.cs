using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum ColorControll
{
    CURVE,
    LERP
}

public enum RenderType
{
    Text,
    Sprite,
    Image
}

[Serializable]
public class DynamicObj : MonoBehaviour
{
    delegate void DoDynamic();

    //===========================components==============================
    private TextMeshProUGUI _textMesh;
    private SpriteRenderer _renderer;
    private Image _image;

    private RenderType renderType;

    //===================Position Production Controll====================
    public bool editXpos;
    public AnimationCurve xCurve;
    [Space]
    public bool editYpos;
    public AnimationCurve yCurve;
    [Space]

    //====================Rotation Production Controll===================
    public bool editXrot;
    public AnimationCurve xRotCurve;
    [Space]
    public bool editYrot;
    public AnimationCurve yRotCurve;
    [Space]
    public bool editZrot;
    public AnimationCurve zRotCurve;
    [Space]

    //====================Scale Production Controll======================
    public bool editScale;
    public AnimationCurve scaleCurve;

    //===================Color Production Controll====================
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
    public bool OnAwake;
    public bool DontCareTime;

    //===========================init state==============================
    private Vector3 tempVec;
    private Quaternion tempRot;
    private Vector3 tempScale;

    private Color tempCol;

    //==========================fixed state==============================
    private Vector3 fixedPos;
    private Quaternion fixedRot;
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
        if (editXrot) doDynamic += ChangeXRot;
        if (editYrot) doDynamic += ChangeYRot;
        if (editZrot) doDynamic += ChangeZRot;
        if (editScale) doDynamic += ChangeScale;
        if (editColor && colorControll == ColorControll.CURVE) doDynamic += ChangeCurveColor;
        if (editColor && colorControll == ColorControll.LERP) doDynamic += ChangeLerpColor;

        if(OnAwake) StartCoroutine(nameof(Production));
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
        TryGetComponent(out _image);

        if (_textMesh != null) renderType = RenderType.Text;
        if (_renderer != null) renderType = RenderType.Sprite;
        if (_image != null) renderType = RenderType.Image;

        //position
        tempVec = fixedPos = this.transform.localPosition;

        //rotation
        tempRot = fixedRot = this.transform.localRotation;

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
        timer = 0;
        while (this.gameObject.activeSelf)
        {
            if (DontCareTime) timer += Time.fixedDeltaTime;
            else timer += Time.deltaTime;

            this.transform.localPosition = fixedPos;
            this.transform.localRotation = fixedRot;
            this.transform.localScale = fixedScale;

            doDynamic?.Invoke();

            switch (renderType)
            {
                case RenderType.Text:   _textMesh.color = fixedCol; break;
                case RenderType.Sprite: _renderer.color = fixedCol; break;
                case RenderType.Image:  _image.color    = fixedCol; break;
            }

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
    void ChangeXRot()
    {
        fixedRot.x = xRotCurve.Evaluate(timer);
    }
    void ChangeYRot()
    {
        fixedRot.y = yRotCurve.Evaluate(timer);
    }
    void ChangeZRot()
    {
        fixedRot.z = zRotCurve.Evaluate(timer);
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

    //==============================Production Call=========================================
    public void CallDoDynamic()
    {
        StopCoroutine(nameof(Production));
        StartCoroutine(nameof(Production));
    }
}