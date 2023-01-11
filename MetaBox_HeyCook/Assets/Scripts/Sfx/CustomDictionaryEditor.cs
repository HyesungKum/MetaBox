using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(EditingGenericClass), true)]
class CustomDictionaryEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keys = property.FindPropertyRelative("keys");
        SerializedProperty values = property.FindPropertyRelative("values");

        int totalCount = keys.arraySize;

        if (totalCount == 0) totalCount = 1;

        return 50f + totalCount * 20f ;
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

        //===========================================================
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(keyPos, keys, new GUIContent("key"));
        EditorGUI.PropertyField(valuePos, values, new GUIContent("value"));

        EditorGUI.EndProperty();
        //===========================================================
    }
}