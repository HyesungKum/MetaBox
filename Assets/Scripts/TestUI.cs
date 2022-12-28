#define Mobile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;
using KumTool;
using KumTool.AppTransition;
using KumTool.InputManager;

public class TestUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI[] test1;

    [SerializeField] TextMeshProUGUI text;

    [Header("==========Apk Button==========")]
    [SerializeField] Button apk1;
    [SerializeField] Button apk2;
    [SerializeField] Button apk3;
    [SerializeField] Button apk4;

    [Header("=============PackageName============")]
    [SerializeField] string apk1PackName = null;
    [SerializeField] string apk2PackName = null;
    [SerializeField] string apk3PackName = null;
    [SerializeField] string apk4PackName = null;


    [SerializeField] Button quit;

    readonly Touch[] touches = new Touch[10];

    private void Awake()
    {
        apk1.onClick.AddListener(() => AppTrans.MoveScene(apk1PackName));
        apk2.onClick.AddListener(() => AppTrans.MoveScene(apk2PackName));
        apk3.onClick.AddListener(() => AppTrans.MoveScene(apk3PackName));
        apk4.onClick.AddListener(() => AppTrans.MoveScene(apk4PackName)); 
        
        quit.onClick.AddListener(() => Application.Quit());

        //=================screen setting==================

        Screen.SetResolution(1920, 1080, true);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

    }

    private void Update()
    {
        count.text = HandInput.GetTouchCount.ToString();
        
        for (int i = 0; i < HandInput.GetTouchCount; i++)
        {
            test1[i].text = $"{HandInput.GetTouches[i].position.x} {HandInput.GetTouches[i].position.y}";
        }
    }
}

