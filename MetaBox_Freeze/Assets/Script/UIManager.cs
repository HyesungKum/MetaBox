using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    static private UIManager instance;
    static public UIManager Instance
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

    [SerializeField] Button reStart = null;
    [SerializeField] Button pause = null;
    [SerializeField] ScrollRect wantedList = null;
    [SerializeField] TextMeshProUGUI gameStartText = null;
    [SerializeField] TextMeshProUGUI timer = null;
    [SerializeField] TextMeshProUGUI catchCount = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;

    public int AnimalNumber { get; set; }
    int countDown = 3;
    int catchNumber = 0;
    float playTime = 50f;

    // Start is called before the first frame update
    void Start()
    {
        reStart.onClick.AddListener(() => SceneManager.LoadScene(0));
        pause.onClick.AddListener(OnClick_Pause);
        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);
        reStart.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        wantedList.gameObject.SetActive(false);
        gameStartText.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        catchCount.gameObject.SetActive(false);
    }

    void OnClick_Pause()
    {
        if (Time.timeScale == 0) Time.timeScale = 1f;
        else Time.timeScale = 0f;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public void RunStartText()
    {
        GameManager.Instance.GameSetting();
        StartCoroutine(nameof(_RunStartText));
    }

    IEnumerator _RunStartText()
    {
        gameStartText.gameObject.SetActive(true);

        gameStartText.text = countDown.ToString();
        yield return waitHalf;
        gameStartText.text = null;
        countDown--;
        yield return waitHalf;


        gameStartText.text = countDown.ToString();
        yield return waitHalf;
        gameStartText.text = null;
        countDown--;
        yield return waitHalf;


        gameStartText.text = countDown.ToString();
        yield return waitHalf;
        gameStartText.text = null;
        countDown--;
        yield return waitHalf;


        gameStartText.text = "Game Start";
        yield return new WaitForSeconds(0.3f);
        gameStartText.gameObject.SetActive(false);
        GameManager.Instance.GameStart();
        StartCoroutine(nameof(_RunTimer));
    }

    IEnumerator _RunTimer()
    {
        pause.gameObject.SetActive(true);
        wantedList.gameObject.SetActive(true);
        timer.gameObject.SetActive(true);
        catchCount.gameObject.SetActive(true);
        catchCount.text = $"{catchNumber} / {AnimalNumber}";
        while (playTime > 0f)
        {
            timer.text = $"{(int)playTime / 60} Min {(int)playTime % 60} Sec";
            
            yield return wait1;
            playTime -= 1f;
            if(playTime <= 10f)
            {
                timer.color = Color.red;
                if(playTime <= 0)
                {
                    timer.text = "00";
                    if (catchNumber < AnimalNumber)
                    {
                        GameManager.Instance.isGaming = false;
                        gameStartText.gameObject.SetActive(true);
                        reStart.gameObject.SetActive(true);
                        gameStartText.text = "You Lose";
                    }
                    yield break;
                }
            }
        }
        
    }
    public void Catch()
    {
        catchNumber++;
        catchCount.text = $"{catchNumber} / {AnimalNumber}";
        if(catchNumber == AnimalNumber)
        {
            playTime = 0;
            GameManager.Instance.isGaming = false;
            gameStartText.gameObject.SetActive(true);
            reStart.gameObject.SetActive(true);
            gameStartText.text = "You Win";
        }
    }
}