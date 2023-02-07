using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSizeChange : MonoBehaviour
{
    [SerializeField] Button thickLineBut = null;
    [SerializeField] Button thinLineBut = null;

    private float lienSize;
    public float LineSize { get { return lienSize; } set { lienSize = value; } }  

    void Awake()
    {
        thickLineBut.onClick.AddListener(delegate { GetThickLineSize(); });
        thinLineBut.onClick.AddListener(delegate { GetThinLineButSize(); });
    }

    float GetThickLineSize()
    {
        float lineThickSize = 0.3f;
        LineSize = lineThickSize;
        //Debug.Log("ThicklienSize :" +lienSize);
        return LineSize;
    }

    float GetThinLineButSize()
    {
        float lineThinSize = 0.15f;
        LineSize = lineThinSize;
        //Debug.Log("ThinlienSize :" + lienSize);
        return LineSize;
    }
}
