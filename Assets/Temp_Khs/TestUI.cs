#define Mobile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kum.InputControll;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;

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

        apk1.onClick.AddListener(() => moveScene(apk1PackName));
        apk2.onClick.AddListener(() => moveScene(apk2PackName));
        apk3.onClick.AddListener(() => moveScene(apk3PackName));
        apk4.onClick.AddListener(() => moveScene(apk4PackName)); 
        apk1.onClick.AddListener(() => AppQuit());
        apk2.onClick.AddListener(() => AppQuit());
        apk3.onClick.AddListener(() => AppQuit());
        apk4.onClick.AddListener(() => AppQuit());

        quit.onClick.AddListener(() => AppQuit());

        Screen.SetResolution(1920, 1080, true);

    }

    private void Update()
    {
        count.text = HandInput.GetTouchCount.ToString();

        for (int i = 0; i < HandInput.GetTouchCount; i++)
        {
            test1[i].text = $"{HandInput.GetTouches[i].position.x} {HandInput.GetTouches[i].position.y}";
        }
    }

    void moveScene(string pakageName)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", pakageName);

        jo.Call("startActivity", intent);

    }

    private void AppQuit()
    {
        Application.Quit();
    }
}

namespace Kum
{
    namespace InputControll
    {
        public class HandInput
        {
            static readonly Touch[] touches = new Touch[10];

            /// <summary>
            /// This function getting just one finger position 
            /// </summary>
            public static Vector3 GetOnePos
            {
                get
                {
                    if (Input.touchCount == 1)
                    {
                        touches[0] = Input.GetTouch(0);
                    }
                    return touches[0].position;
                }
            }

            /// <summary>
            /// This function Get count for how many finger interacting touched 
            /// </summary>
            public static int GetTouchCount
            {
                get
                {
                    if (Input.touchCount > 10)
                    {
                        return 10;
                    }
                    else
                    {
                        return Input.touchCount;
                    }
                }
            }

            /// <summary>
            /// this function using when need multiple touch limited 10
            /// can be return multi finger position
            /// </summary>
            public static Touch[] GetTouches
            {
                get
                {
                    int count;

                    if (Input.touchCount > 10)
                    {
                        count = 10;
                    }
                    else
                    {
                        count = Input.touchCount;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        touches[i] = Input.GetTouch(i);
                    }

                    return touches;
                }
            }
        }
    }
}
