using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    [Header("Current Object")]
    [SerializeField] GameObject curObj;
    public int index = 0;

    [Header("object List")]
    [SerializeField] GameObject[] Objs;

    public void NextObj()
    {
        curObj.SetActive(false);
        index = (index + 1) % Objs.Length;
        curObj = Objs[index];
        curObj.SetActive(true);

    }
    public void PrevObj()
    {
        curObj.SetActive(false);
        if(index == 0) index = Objs.Length;
        else index -= 1; 

        curObj = Objs[index];
        curObj.SetActive(true);
    }
}
