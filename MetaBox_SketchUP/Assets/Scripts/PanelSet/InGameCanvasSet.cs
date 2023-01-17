using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvasSet : MonoBehaviour
{
    [SerializeField] GameObject selectPanel = null;
    [SerializeField] GameObject inGameOther = null;
    [SerializeField] GameObject losePanel = null;
    [SerializeField] GameObject oneBrushObj = null;


    void Awake()
    {
        FirstSetting();
    }

    void FirstSetting()
    {
        selectPanel.gameObject.SetActive(true);
        inGameOther.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }
}
