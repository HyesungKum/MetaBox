using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineColorChanged : MonoBehaviour
{
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Slider lineColorslider = null;
    [SerializeField] Slider linSizeslider = null;

    public Color newColor;
    float a;
    float sliderValue;

    void Awake()
    {
        colorOne.onClick.AddListener(delegate { GetOtherColor(colorOne); });
        colorTwo.onClick.AddListener(delegate { GetOtherColor(colorTwo); });
        colorThree.onClick.AddListener(delegate { GetOtherColor(colorThree); });
        lineColorslider.value = 1;
        lineColorslider.onValueChanged.AddListener(delegate { SetColorAlpha(); });
        //linSizeslider.onValueChanged.AddListener(delegate { });
    }

    void SetColorAlpha()
    {
        a = newColor.a;
        a = lineColorslider.value;
        Debug.Log("a" + a);
    }


    Color GetOtherColor(Button colorNum) 
    {
        Image imgageColor = null;
        colorNum.transform.gameObject.TryGetComponent<Image>(out imgageColor);
        newColor = imgageColor.color;
        Debug.Log("newColor : " + newColor);
        return newColor;
    }

    public Color GetColor()
    {
        return newColor;
    }
}