using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

namespace BeanTool
{
    public class BeanFontChanger
    {
        public const string PATH_FONT_TEXTMESHPRO_M = "Assets/Fonts/CookieRun Bold SDF.asset";

        [MenuItem("BeanTool/BeanFontChanger")]
        public static void ChangeFont()
        {
            GameObject[] rootObj = GetSceneRootObject();

            for(int i = 0; i < rootObj.Length; i++)
            {
                GameObject go = (GameObject)rootObj[i] as GameObject;
                Component[] com1 = go.transform.GetComponentsInChildren(typeof(TMP_InputField), true);
                foreach (TMP_InputField txt in com1)
                {
                    txt.fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(PATH_FONT_TEXTMESHPRO_M);
                }
                Component[] com2 = go.transform.GetComponentsInChildren(typeof(TextMeshProUGUI), true);
                foreach(TextMeshProUGUI txt in com2)
                {
                    txt.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(PATH_FONT_TEXTMESHPRO_M);
                }
            }
            Debug.Log("¹Ù²å¾îÀ¯");
        }
        static GameObject[] GetSceneRootObject()
        {
            Scene curscene = SceneManager.GetActiveScene();
            return curscene.GetRootGameObjects();
        }

    }
}