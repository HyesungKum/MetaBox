using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MaterialChanger : EditorWindow
{
    private Material material;

    [MenuItem("Tools/Material Changer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialChanger>("Material Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Material", EditorStyles.boldLabel);
        material = (Material)EditorGUILayout.ObjectField(material, typeof(Material), false);

        if (GUILayout.Button("Change Material"))
        {
            ChangeMaterialInScene();
        }
    }

    private void ChangeMaterialInScene()
    {
        GameObject[] rootObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject root in rootObjects)
        {
            // Change material in sprite renderer components
            foreach (SpriteRenderer sr in root.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (sr.sharedMaterial != material)
                {
                    sr.sharedMaterial = material;
                }
            }

            // Change material in image components
            foreach (Image img in root.GetComponentsInChildren<Image>(true))
            {
                if (img.material != material)
                {
                    img.material = material;
                }
            }

            // Change material in tilemap components
            foreach (TilemapRenderer tilemap in root.GetComponentsInChildren<TilemapRenderer>(true))
            {
                if (tilemap.sharedMaterial != material)
                {
                    tilemap.sharedMaterial = material;
                }
            }

            // Change material in renderer components
            foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer.material != material)
                {
                    renderer.material = material;
                }
            }
        }
    }
}