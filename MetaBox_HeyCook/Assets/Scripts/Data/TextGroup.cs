using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextData
{
    [SerializeField] public string[] texts;
}

[CreateAssetMenu(menuName = "ScriptableObj/TextGroup", fileName = "TextGroup")]
public class TextGroup : ScriptableObject
{
    public TextData textData;
}
