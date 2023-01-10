using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowImgManger : MonoBehaviour
{
    [SerializeField] private string imgName = "";
    [SerializeField] private int imgIndex = 0;

    public string GetImgName() => imgName;
    public int GetImgIndex() => imgIndex;

}
