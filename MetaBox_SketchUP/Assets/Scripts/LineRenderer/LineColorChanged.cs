using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineColorChanged : MonoBehaviour
{
    [SerializeField] Button colorOne = null;
    [SerializeField] Button colorTwo = null;
    [SerializeField] Button colorThree = null;

    public Color newColor;

    void Awake()
    {
        colorOne.onClick.AddListener(delegate { GetOtherColor(colorOne); });
        colorTwo.onClick.AddListener(delegate { GetOtherColor(colorTwo); });
        colorThree.onClick.AddListener(delegate { GetOtherColor(colorThree); });
    }

    public Color GetOtherColor(Button colorNum)
    {
        Image imgageColor = null;
        colorNum.transform.gameObject.TryGetComponent<Image>(out imgageColor);
        newColor = imgageColor.color;
        return newColor;
        Debug.Log("newColor : " + newColor);
    }
}