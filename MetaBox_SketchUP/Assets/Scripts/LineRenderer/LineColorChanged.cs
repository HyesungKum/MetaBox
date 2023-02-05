using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineColorChanged : MonoBehaviour
{
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;
    [SerializeField] Slider slider = null;

    public Color newColor;
    float a;
    float sliderValue;

    void Awake()
    {
        colorOne.onClick.AddListener(delegate { GetOtherColor(colorOne); });
        colorTwo.onClick.AddListener(delegate { GetOtherColor(colorTwo); });
        colorThree.onClick.AddListener(delegate { GetOtherColor(colorThree); });
        slider.value = 1;
        slider.onValueChanged.AddListener(delegate { SetColorAlpha(); });
    }

    void SetColorAlpha()
    {
        a = newColor.a;
        a = slider.value;
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