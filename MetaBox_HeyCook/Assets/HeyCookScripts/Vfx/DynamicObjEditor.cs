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

    //rotation property
    private SerializedProperty xRotProperty;
    private SerializedProperty yRotProperty;
    private SerializedProperty zRotProperty;

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

        //position property
        xProperty = serializedObject.FindProperty(nameof(dynamicObj.xCurve));
        yProperty = serializedObject.FindProperty(nameof(dynamicObj.yCurve));

        //rotation porperty
        xRotProperty = serializedObject.FindProperty(nameof(dynamicObj.xRotCurve));
        yRotProperty = serializedObject.FindProperty(nameof(dynamicObj.yRotCurve));
        zRotProperty = serializedObject.FindProperty(nameof(dynamicObj.zRotCurve));

        //scale property
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
        //position property drawing
        EditorGUILayout.LabelField("[Dynamic Position]");
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

        //rotation property drawing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("[Dynamic Rotation]");
        dynamicObj.editXrot = EditorGUILayout.Toggle("Custom X Rotation", dynamicObj.editXrot);
        if (dynamicObj.editXrot)
        {
            EditorGUILayout.PropertyField(xRotProperty);
            EditorGUILayout.Space();
        }

        dynamicObj.editYrot = EditorGUILayout.Toggle("Custom Y Rotation", dynamicObj.editYrot);
        if (dynamicObj.editYrot)
        {
            EditorGUILayout.PropertyField(yRotProperty);
            EditorGUILayout.Space();
        }

        dynamicObj.editZrot = EditorGUILayout.Toggle("Custom Z Rotation", dynamicObj.editZrot);
        if (dynamicObj.editZrot)
        {
            EditorGUILayout.PropertyField(zRotProperty);
            EditorGUILayout.Space();
        }

        //scale property drawing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("[Dynamic Scale]");
        dynamicObj.editScale = EditorGUILayout.Toggle("Custom Scale", dynamicObj.editScale);
        if (dynamicObj.editScale)
        {
            EditorGUILayout.PropertyField(scaleProperty);
            EditorGUILayout.Space();
        }

        //color property drawing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("[Dynamic Color]");
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

        //setting drawing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("[Dynamic Setting]");
        //setting care about frame time
        dynamicObj.DontCareTime = EditorGUILayout.Toggle("Dont Care Time", dynamicObj.DontCareTime);
        //looping movement
        dynamicObj.OnAwake = EditorGUILayout.Toggle("Dynamic OnAwake", dynamicObj.OnAwake);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
