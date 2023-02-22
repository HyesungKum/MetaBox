using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, ISerializationCallbackReceiver
{

    [SerializeField] private List<Tkey> myKeys = new List<Tkey>();
    [SerializeField] private List<Tvalue> myValues = new List<Tvalue>();

    // save         
    public void OnBeforeSerialize()
    {
        myKeys.Clear();
        myValues.Clear();

        foreach (KeyValuePair<Tkey, Tvalue> pair in this)
        {
            myKeys.Add(pair.Key);
            myValues.Add(pair.Value);
        }
    }

    // load dictionary from lists 
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (myKeys.Count != myValues.Count)
            throw new Exception(String.Format("Keys and Values are not match!"));

        for (int i = 0; i < myKeys.Count; ++i)
        {
            this.Add(myKeys[i], myValues[i]);
        }
    }
}

