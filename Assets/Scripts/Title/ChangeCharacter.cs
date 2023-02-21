using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] Objs;
    [SerializeField] GameObject curObj;

    public int index = 0;

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
        index = index <= 0 ? Objs.Length : index - 1; 
        curObj = Objs[index];
        curObj.SetActive(true);
    }
}
