using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// base class for customized generic editor 
/// </summary>
public abstract class EditingGenericClass { }

[Serializable]
public class DictionaryView<Tkey, Tvalue> : EditingGenericClass
{
    public List<Tkey> keys = new();
    public List<Tvalue> values = new();
}

[Serializable]
public class CustomDictionary<Tkey, Tvalue> : ISerializationCallbackReceiver
{
    public Dictionary<Tkey, Tvalue> dictionary;

    #region Editor
#if UNITY_EDITOR
    /// <summary>
    /// Viewalbe Data for Serialize dictionary
    /// </summary>
    public DictionaryView<Tkey, Tvalue> dictionaryView = new();
    /// <summary>
    /// should not contact raw inputKey must be contact dictionary
    /// </summary>
    [HideInInspector] public Tkey inputKey = default;
    /// <summary>
    /// should not contact raw inputValue must be contact dictionary
    /// </summary>
    [HideInInspector] public Tvalue inputValue = default;

    public void OnBeforeSerialize()
    {
        if (dictionaryView == null) return;
        if (dictionaryView.keys == null) return;
        if (dictionaryView.values == null) return;
        if (dictionaryView.keys.Count == 0) return;
        if (dictionaryView.values.Count == 0) return;

        dictionaryView.keys.Clear();
        dictionaryView.values.Clear();

        foreach (var dicData in dictionary)
        {
            dictionaryView.keys.Add(dicData.Key);
            dictionaryView.values.Add(dicData.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<Tkey, Tvalue>();

        bool sometingAdd = false;
        int count = 0;

        for (int i = 0; i < Mathf.Max(dictionaryView.keys.Count, dictionaryView.values.Count); i++)
        {
            if (dictionaryView.keys.Count > dictionaryView.values.Count)//키 눌려 밸류 확장
            {
                sometingAdd = true;
                dictionaryView.values.Add(default);
            }
            if (dictionaryView.keys.Count < dictionaryView.values.Count)//밸류 ++
            {
                sometingAdd = true;
                dictionaryView.keys.Add(default);
            }

            count++;
        }

        if (sometingAdd)
        {
            dictionaryView.keys[^1] = inputKey;
            dictionaryView.values[^1] = inputValue;

            inputKey = default;
            inputValue = default;
        }

        for (int i = 0; i < count; i++)
        {
            try
            {
                dictionary.Add(dictionaryView.keys[i], dictionaryView.values[i]);
            }
            catch (Exception ex)
            {
                Debug.Log("dictionary input error ## : " + ex);
            }
        }
    }
#endif
    #endregion

    #region Connecting Legacy Dictionary Function
    public void Add(Tkey key, Tvalue value) => dictionary.Add(key, value);
    public bool Remove(Tkey key) => dictionary.Remove(key);
    public bool TryGetValue(Tkey key, out Tvalue value) => dictionary.TryGetValue(key, out value);
    public void Clear() => dictionary.Clear();
    #endregion
}