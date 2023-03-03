using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeanTool
{
    public class BeanFontChanger : EditorWindow
    {
        private TMP_FontAsset changeFont;

        [MenuItem("BeanTool/BeanFontChanger")]
        public static void ShowWindow()
        {
            GetWindow<BeanFontChanger>("Font Changer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Select Font", EditorStyles.boldLabel);
            changeFont = (TMP_FontAsset)EditorGUILayout.ObjectField(changeFont, typeof(TMP_FontAsset), false);

            if (GUILayout.Button("Change Font"))
            {
                ChangeFont();
            }
        }

        private void ChangeFont()
        {
            GameObject[] rootObj = GetSceneRootObject();

            for(int i = 0; i < rootObj.Length; i++)
            {
                GameObject go = (GameObject)rootObj[i] as GameObject;
                Component[] com1 = go.transform.GetComponentsInChildren(typeof(TMP_InputField), true);
                foreach (TMP_InputField txt in com1)
                {
                    txt.fontAsset = changeFont;
                }
                Component[] com2 = go.transform.GetComponentsInChildren(typeof(TextMeshProUGUI), true);
                foreach(TextMeshProUGUI txt in com2)
                {
                    txt.font = changeFont;
                }
            }
        }

        private GameObject[] GetSceneRootObject()
        {
            Scene curscene = SceneManager.GetActiveScene();
            return curscene.GetRootGameObjects();
        }

    }
}