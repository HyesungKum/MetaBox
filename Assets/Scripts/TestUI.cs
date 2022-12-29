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
using System;
using UnityEngine.Android;
using Unity.Notifications.Android;

public class TestUI : MonoBehaviour
{
    [Header("BGControll")]
    [SerializeField] GameObject BG;
    [SerializeField] float Sensitive = 5f;

    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI[] test1;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] TextMeshProUGUI deltatext;

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

    float yPos;

    private void Awake()
    {
        apk1.onClick.AddListener(() => TestSceneMove(apk1PackName));

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

        yPos = BG.transform.position.y;
    }

    private void Update()
    {
        count.text = HandInput.GetTouchCount.ToString();

        for (int i = 0; i < HandInput.GetTouchCount; i++)
        {
            test1[i].text = $"{HandInput.GetTouches[0].fingerId}";

            if (HandInput.GetTouchCount >= 2)
            {
                if (   HandInput.GetTouches[0].deltaPosition.x != 0 
                    || HandInput.GetTouches[0].deltaPosition.y != 0
                    || HandInput.GetTouches[1].deltaPosition.x != 0
                    || HandInput.GetTouches[1].deltaPosition.y != 0 )
                {
                    float newX = (HandInput.GetTouches[0].deltaPosition.x + HandInput.GetTouches[1].deltaPosition.x) / 2f;
                    float newY = (HandInput.GetTouches[0].deltaPosition.y + HandInput.GetTouches[1].deltaPosition.y) / 2f;

                    Vector3 newVec = new Vector3(newX, newY, 0f).normalized;
                    
                    BG.transform.position += Time.deltaTime * Sensitive * newVec;

                    deltatext.text = $"{HandInput.GetTouches[i].deltaPosition.x} {HandInput.GetTouches[i].deltaPosition.y}";
                }
            }
        }

        BG.transform.position = Vector3.Lerp(BG.transform.position, new Vector3(BG.transform.position.x, yPos, 0f), Time.deltaTime * 2f);
    }

    //=====================================================================list of DLLize=====================================================================
    //now dll list
    //handinput
    //app transition

    public void TestSceneMove(string pakageName)
    {
        AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>());
        AndroidJavaObject androidJavaObject2 = null;
        try
        {
            androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getLaunchIntentForPackage", new object[1] { pakageName });
        }
        catch (Exception ex)
        {
            Debug.LogError("exception" + ex.Message);
        }

        @static.Call("startActivity", androidJavaObject2);

        Application.Unload();
    }
}

