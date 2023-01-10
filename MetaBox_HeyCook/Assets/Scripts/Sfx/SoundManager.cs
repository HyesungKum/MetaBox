using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public struct Dic<Tkey,Tvalue>
{
    [SerializeField] Tkey key;
    [SerializeField] Tvalue value;
}

public class CD<Tkey,Tvalue>
{
    public Dictionary<Tkey, Tvalue> dictionary;

    public Dic<Tkey,Tvalue>[] dic;
}


[CreateAssetMenu(menuName = "ScriptableObj/SoundData", fileName = "SoundData")]
public class SoundData : ScriptableObject
{
    public Dic<string, AudioClip>[] dic;
}

//[CustomEditor(typeof(SoundData))]
//public class SoundDataEditor : Editor
//{
//    private Dictionary<string, AudioClip> clips;

//    private void OnEnable()
//    {
//        dictionary = (CustomDictionary)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (dictionary.keys.Count > 0)
//        {
//            for (int i = 0; i < dictionary.keys.Count; i++)
//            {
//                EditorGUILayout.BeginHorizontal();

//                EditorGUILayout.TextField(dictionary.keys[0]);
//                EditorGUILayout.IntField(dictionary.values[0]);

//                EditorGUILayout.EndHorizontal();
//            }
//        }
//    }
//}