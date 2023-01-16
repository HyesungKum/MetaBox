using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCheck : MonoBehaviour
{
    [SerializeField] private string imgName = "";
    [SerializeField] private int winCount = 0;

    private int objOneDrawCount = 5;
    public int ObjOneDrawCount
    { get { return objOneDrawCount; }
        set { objOneDrawCount = value; } 
    }

    private int objTwoDrawCount = 5;
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


    public string ObjName(string name)
    {
        imgName = name;
        return imgName;
    }

}
