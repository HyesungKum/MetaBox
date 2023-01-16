using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCheck : MonoBehaviour
{
    [SerializeField] private string imgName = "";
    [SerializeField] private int ObjDrawCount = 0;

    //public string imgName;
    void Awake()
    {
        SetObjName();
    }

    public string GetPrentName() => imgName;

    
    private int objOneDrawCount = 3;
    public int ObjOneDrawCount
    { get { return objOneDrawCount; }
        set { objOneDrawCount = value; } 
    }

    private int objTwoDrawCount = 10;
    public int ObjTwoDrawCount
    {
        get { return objTwoDrawCount; }
        set { objTwoDrawCount = value; }
    }

    private int objThreeDrawCount = 8;
    public int ObjThreeDrawCount
    {
        get { return objThreeDrawCount; }
        set { objThreeDrawCount = value; }
    }

    //public string GetName() => imgName;

    public string SetObjName()
    {
        imgName = this.gameObject.name;
        //Debug.Log(" this.gameObject.name :" + this.gameObject.name);
        return imgName;
    }
}
