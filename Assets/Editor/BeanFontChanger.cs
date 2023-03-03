using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

namespace BeanTool
{
    public class BeanMaterialChanger
    {
        public const string PATH_MATERIAL_URP = "Assets/UrpMaterial.mat";

        [MenuItem("BeanTool/BeanMaterialChanger")]
        public static void ChangeMaterial()
        {
            GameObject[] rootObj = GetSceneRootObject();

            for (int i = 0; i < rootObj.Length; i++)
            {
                GameObject go = (GameObject)rootObj[i] as GameObject;
                Component[] com1 = go.transform.GetComponentsInChildren(typeof(SpriteRenderer), true);
                foreach (SpriteRenderer sp in com1)
                {
                    sp.material = AssetDatabase.LoadAssetAtPath<Material>(PATH_MATERIAL_URP);
                }
                Component[] com2 = go.transform.GetComponentsInChildren(typeof(Image), true);
                foreach (Image img in com2)
                {
                    img.material = AssetDatabase.LoadAssetAtPath<Material>(PATH_MATERIAL_URP);
                }
            }
        }

        static GameObject[] GetSceneRootObject()
        {
            Scene curscene = EditorSceneManager.GetActiveScene();
            return curscene.GetRootGameObjects();
        }
    }
}