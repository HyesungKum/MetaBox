using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    //=================button=====================
    [SerializeField] Button backButton;

    //=================viewing ui=================
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] GameObject gameOverObj;

    private void Awake()
    {
        gameOverObj = GameObject.Find("GameOverText");

        //add button listener
        backButton.onClick.AddListener(()=>SceneMove());

        //delegate chain
        EventReciver.ScoreModi += UIScoreModi;
        EventReciver.GameOver += UIGameOver;
    }

    private void Start()
    {
        gameOverObj.SetActive(false);
    }

    private void Update()
    {
        Timer.text = string.Format("{0:D2} : {1:D2} ",(int)(GameManager.Inst.Timer/60f),(int)(GameManager.Inst.Timer % 60f));
    }

    private void OnDisable()
    {

        EventReciver.ScoreModi -= UIScoreModi;
        EventReciver.GameOver -= UIGameOver;
    }

    void SceneMove()
    {
        SceneManager.LoadScene("1. StartScene");
    }
    void UIGameOver()
    {
        gameOverObj.SetActive(true);
    }

    void UIScoreModi(int value)
    {
        Score.text = GameManager.Inst.Score.ToString();
    }
}
