using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region ╫л╠шео
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

    void Awake()
    {
        reStartBut.onClick.AddListener(() => OnClickRestart());
    }

    void Start()
    {
        
    }

    void Update()
    {
        


    }


    
    void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
