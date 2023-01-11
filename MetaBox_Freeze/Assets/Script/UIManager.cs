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

    [SerializeField] GameObject wantedListImage = null;
    [SerializeField] Sprite arrestImage = null;

    [SerializeField] Button reStart = null;
    [SerializeField] Button pause = null;
    [SerializeField] Button exit = null;
    [SerializeField] ScrollRect wantedList = null;
    [SerializeField] TextMeshProUGUI gameStartText = null;
    [SerializeField] TextMeshProUGUI timer = null;

    WaitForSeconds waitHalf = null;
    WaitForSeconds wait1 = null;

    int wantedCount;
    int countdown;
    

    // Start is called before the first frame update
    void Start()
    {
        reStart.onClick.AddListener(() => SceneManager.LoadScene(0));
        pause.onClick.AddListener(OnClick_Pause);
        exit.onClick.AddListener(delegate { OnClick_Exit(); });

        waitHalf = new WaitForSeconds(0.5f);
        wait1 = new WaitForSeconds(1f);

        reStart.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        wantedList.gameObject.SetActive(false);
        gameStartText.gameObject.SetActive(false);
    }

    public void DataSetting(int wantedCount, int startCountdown)
    {
        this.wantedCount = wantedCount;
        this.countdown = startCountdown;
        Debug.Log(wantedCount);
        wantedList.gameObject.SetActive(true);
        for (int i = 0; i < wantedCount; i++)
        {
            Instantiate(wantedListImage, wantedList.content.transform);
        }
    }

    public void Timer()
    {
        timer.text = $"{GameManager.Instance.PlayTime / 60} Min {GameManager.Instance.PlayTime % 60} Sec";
        if (GameManager.Instance.PlayTime <= 10f)
        {
            timer.color = Color.red;
        }
    }

    public void Arrest(int catchCount)
    {
        wantedList.content.GetChild(catchCount - 1).GetComponent<Image>().sprite = arrestImage;
    }

    public void WaveStart()
    {
        StartCoroutine(nameof(RunCountdown));
    }

    public void WaveClear()
    {
        for(int i = 0; i < wantedList.content.childCount; i++)
        {
            Destroy(wantedList.content.GetChild(i).gameObject);
        }
        pause.gameObject.SetActive(false);
        StartCoroutine(nameof(Success));
    }

    public void Win()
    {
        gameStartText.gameObject.SetActive(true);
        reStart.gameObject.SetActive(true);
        gameStartText.text = "You Win";
    }

    public void Lose()
    {
        gameStartText.gameObject.SetActive(true);
        reStart.gameObject.SetActive(true);
        gameStartText.text = "You Lose";
    }

    IEnumerator Success()
    {
        gameStartText.gameObject.SetActive(true);
        gameStartText.text = "Success";
        yield return waitHalf;
        gameStartText.gameObject.SetActive(false);
        GameManager.Instance.WaveSetting();
    }

    IEnumerator RunCountdown()
    {
        gameStartText.gameObject.SetActive(true);

        for(int i = countdown; i > 0; i--)
        {
            gameStartText.text = countdown.ToString();
            yield return waitHalf;
            gameStartText.text = null;
            countdown--;
            yield return waitHalf;
        }

        gameStartText.text = "Game Start";
        yield return waitHalf;
        gameStartText.gameObject.SetActive(false);
        GameManager.Instance.WaveStart();
        pause.gameObject.SetActive(true);
    }

    void OnClick_Pause()
    {
        if (Time.timeScale == 0) Time.timeScale = 1f;
        else Time.timeScale = 0f;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void OnClick_Exit()
    {
        Application.Quit();
    }
}
