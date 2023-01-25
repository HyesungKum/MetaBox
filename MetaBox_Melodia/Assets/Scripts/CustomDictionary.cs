using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




[CustomPropertyDrawer(typeof(EditingGenericClass), true)]
class CustomDictionaryEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keys = property.FindPropertyRelative("keys");
        SerializedProperty values = property.FindPropertyRelative("value");

        int totalCount = keys.arraySize;

        if (totalCount == 0)
            totalCount = 1;

        return 50f + totalCount * 20f;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label.text = null;

        SerializedProperty keys = property.FindPropertyRelative("keys");
        SerializedProperty values = property.FindPropertyRelative("values");

        float widthSize = position.width / 2f;
        float height = position.height;

        Rect keyPos = new(position.x, position.y, widthSize, height);
        Rect valuePos = new(position.x + widthSize, position.y, widthSize, height);

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(keyPos, keys, new GUIContent("key"));
        EditorGUI.PropertyField(valuePos, values, new GUIContent("value"));
        EditorGUI.EndProperty();
    }

}


public abstract class EditingGenericClass { }

[Serializable]
public class DictionaryView<Tkey, Tvalue> :  EditingGenericClass
{
    public List<Tkey> keys = new();
    public List<Tvalue> values = new();
}




[Serializable]
public class CustomDictionary<Tkey, Tvalue> : ISerializationCallbackReceiver
{
    public Dictionary<Tkey, Tvalue> myDictionary;


    public DictionaryView<Tkey, Tvalue> myDictionaryView = new();

    [HideInInspector] public Tkey inputKey = default;
    [HideInInspector] public Tvalue inputValue = default;



    public void OnBeforeSerialize()
    {
        if (myDictionaryView == null)
            return;

        if (myDictionaryView.keys == null)
            return;

        if (myDictionaryView.values == null)
            return;

        if (myDictionaryView.keys.Count == 0)
            return;

        if (myDictionaryView.values.Count == 0)
            return;


        myDictionaryView.keys.Clear();
        myDictionaryView.values.Clear();


        foreach (var dicData in myDictionary)
        {
            myDictionaryView.keys.Add(dicData.Key);
            myDictionaryView.values.Add(dicData.Value);
        }

    }

    public void OnAfterDeserialize()
    {
        myDictionary = new Dictionary<Tkey, Tvalue>();

        bool added = false;
        int count = 0;

        for (int i = 0; i < Mathf.Max(myDictionaryView.keys.Count, myDictionaryView.values.Count); ++i)
        {
            if (myDictionaryView.keys.Count > myDictionaryView.values.Count)
            {
                added = true;
                myDictionaryView.values.Add(default);
            }

            if (myDictionaryView.keys.Count < myDictionaryView.values.Count)
            {
                added = true;
                myDictionaryView.keys.Add(default);
            }

            count++;
        }

        if (added)
        {
            // [^1] == length -1 
            myDictionaryView.keys[^1] = inputKey;
            myDictionaryView.values[^1] = inputValue;

            inputKey = default;
            inputValue = default;
        }

        for (int i = 0; i < count; ++i)
        {
            try
            {
                myDictionary.Add(myDictionaryView.keys[i], myDictionaryView.values[i]);
            }

            catch (Exception ex)
            {
                Debug.Log("Dictionary input error : " + ex);
            }
        }
    }


    public void Add(Tkey key, Tvalue value) => myDictionary.Add(key, value);
    public bool Remove(Tkey key) => myDictionary.Remove(key);
    public bool TryGetValue(Tkey key, out Tvalue value) => myDictionary.TryGetValue(key, out value);
    public void Clear() => myDictionary.Clear();


}
