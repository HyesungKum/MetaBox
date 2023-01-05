using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    //================in game ui===================
    [Header("In Game UI")]
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] TextMeshProUGUI Score;

    [SerializeField] GameObject inGameUIObj;

    [SerializeField] Button ExitButton;


    //=================end game ui=================
    [Header("End Game UI")]
    [SerializeField] GameObject gameOverUIObj;

    [SerializeField] Button RestartButton;
    [SerializeField] Button EndExitButton;

    private void Awake()
    {
        //add button listener
        ExitButton.onClick.AddListener(()=>SceneMove(SceneName.Start));
        RestartButton.onClick.AddListener(()=>SceneMove(SceneName.Main));
        EndExitButton.onClick.AddListener(()=>SceneMove(SceneName.Start));

        //delegate chain
        EventReciver.ScoreModi += UIScoreModi;
        EventReciver.GameOver += UIGameOver;
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

    void SceneMove(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    void UIGameOver()
    {
        inGameUIObj.SetActive(false);
        gameOverUIObj.SetActive(true);
    }

    void UIScoreModi(int value)
    {
        Score.text = GameManager.Inst.Score.ToString();
    }
}
