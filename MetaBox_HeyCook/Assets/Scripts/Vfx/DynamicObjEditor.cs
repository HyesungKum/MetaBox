using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(DynamicObj))]
public class DynamicObjEditor : Editor
{
    //target class
    private DynamicObj dynamicObj;

    //position property
    private SerializedProperty xProperty;
    private SerializedProperty yProperty;

    //scale property
    private SerializedProperty scaleProperty;

    //color property
    private SerializedProperty colorRProperty;
    private SerializedProperty colorGProperty;
    private SerializedProperty colorBProperty;
    private SerializedProperty colorAProperty;

    private SerializedProperty lerpTimeProperty;
    private SerializedProperty startColorProperty;
    private SerializedProperty endColorProperty;

    private void OnEnable()
    {
        dynamicObj = target as DynamicObj;

        xProperty = serializedObject.FindProperty(nameof(dynamicObj.xCurve));
        yProperty = serializedObject.FindProperty(nameof(dynamicObj.yCurve));

        scaleProperty = serializedObject.FindProperty(nameof(dynamicObj.scaleCurve));

        colorRProperty = serializedObject.FindProperty(nameof(dynamicObj.colorRCurve));
        colorGProperty = serializedObject.FindProperty(nameof(dynamicObj.colorGCurve));
        colorBProperty = serializedObject.FindProperty(nameof(dynamicObj.colorBCurve));
        colorAProperty = serializedObject.FindProperty(nameof(dynamicObj.colorACurve));

        lerpTimeProperty = serializedObject.FindProperty(nameof(dynamicObj.lerpTime));
        startColorProperty = serializedObject.FindProperty(nameof(dynamicObj.startColor));
        endColorProperty = serializedObject.FindProperty(nameof(dynamicObj.endColor));
    }

    public override void OnInspectorGUI()
    {
        dynamicObj.editXpos = EditorGUILayout.Toggle("Custom X Postion", dynamicObj.editXpos);
        if (dynamicObj.editXpos)
        {
            EditorGUILayout.PropertyField(xProperty);
            EditorGUILayout.Space();
        }

        dynamicObj.editYpos = EditorGUILayout.Toggle("Custom Y Postion", dynamicObj.editYpos);
        if (dynamicObj.editYpos)
        {
            EditorGUILayout.PropertyField(yProperty);
            EditorGUILayout.Space();
        }

        dynamicObj.editScale = EditorGUILayout.Toggle("Custom Scale", dynamicObj.editScale);
        if (dynamicObj.editScale)
        {
            EditorGUILayout.PropertyField(scaleProperty);
            EditorGUILayout.Space();
        }

        dynamicObj.editColor = EditorGUILayout.Toggle("Custom Color", dynamicObj.editColor);
        if (dynamicObj.editColor)
        {
            dynamicObj.colorControll = (ColorControll)EditorGUILayout.EnumPopup(dynamicObj.colorControll);
            switch (dynamicObj.colorControll)
            {
                case ColorControll.CURVE:
                    {
                        EditorGUILayout.PropertyField(colorRProperty);
                        EditorGUILayout.PropertyField(colorGProperty);
                        EditorGUILayout.PropertyField(colorBProperty);
                        EditorGUILayout.PropertyField(colorAProperty);
                        EditorGUILayout.Space();
                    }
                    break;
                case ColorControll.LERP:
                    {
                        EditorGUILayout.PropertyField(lerpTimeProperty);
                        EditorGUILayout.PropertyField(startColorProperty);
                        EditorGUILayout.PropertyField(endColorProperty);
                    }
                    break;
            }
        }

        dynamicObj.DonCareTime = EditorGUILayout.Toggle("Dont Care Time", dynamicObj.DonCareTime);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
