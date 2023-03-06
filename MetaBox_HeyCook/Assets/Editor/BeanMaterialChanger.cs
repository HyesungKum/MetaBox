using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BeanTool
{
    public class BeanMaterialChanger : EditorWindow
    {
        private Material changeMaterial;

        [MenuItem("BeanTool/BeanMaterialChanger")]
        public static void ShowWindow()
        {
            GetWindow<BeanMaterialChanger>("Material Changer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Select Material", EditorStyles.boldLabel);
            changeMaterial = (Material)EditorGUILayout.ObjectField(changeMaterial, typeof(Material), false);
            
            if (GUILayout.Button("Change Material"))
            {
                ChangeMaterial();
            }
        }

        public void ChangeMaterial()
        {
            GameObject[] rootObj = GetSceneRootObject();

            for (int i = 0; i < rootObj.Length; i++)
            {
                GameObject go = (GameObject)rootObj[i] as GameObject;
                Component[] com1 = go.transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
                foreach (SpriteRenderer sr in com1)
                {
                    sr.material = changeMaterial;
                }
                Component[] com2 = go.transform.GetComponentsInChildren(typeof(Image), true);
                foreach (Image img in com2)
                {
                    img.material = changeMaterial;
                }
            }

            GameObject workObj = new GameObject("plz Delete job complete!");
        }

        static GameObject[] GetSceneRootObject()
        {
            Scene curscene = EditorSceneManager.GetActiveScene();
            return curscene.GetRootGameObjects();
        }

    }
}
