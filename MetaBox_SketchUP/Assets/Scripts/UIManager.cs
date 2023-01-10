using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글턴
    private static UIManager instance;
    public static UIManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(UIManager), typeof(UIManager)).GetComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField] GameObject selectPanel = null;
    [SerializeField] Button reStartBut = null;

    [SerializeField] Button RevertButton = null;

    //public Stack<GameObject> lineStack = new Stack<GameObject>();

    

    void Awake()
    {

    }

    void Start()
    {
        //RevertButton.onClick.AddListener(() => PoplineStack(check));
        reStartBut.onClick.AddListener(() => OnClickRestart());
    }

    void Update()
    {
        


    }

    public void PushlineStack(GameObject line)
    {
        //lineStack.Push(line);
        Debug.Log("## 스택에 추가 됨 :" + line);
    }

    public void PoplineStack(GameObject objPop)
    {
       // objPop = lineStack.Pop();
        Debug.Log("## 스택에서 하나 뺌");
    }


    void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
