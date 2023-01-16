using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
    [Header("[Lose Panel]")]
    [SerializeField] GameObject losePanel = null;

    TextMeshProUGUI timerText = null;

    float time = 600;

    // === development mode ===
    //float time = 6;

    void Awake()
    {
        losePanel.gameObject.SetActive(false);
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        //TimerDown();
    }

    void Update()
    {
        TimerDown();
    }

    public void TimerDown()
    {
        time -= 1 * Time.deltaTime;
        timerText.text = $" Time : {Mathf.Round(time).ToString()}";
        //Debug.Log("## time : " + time);

        if (Mathf.Round(time) <= 0)
        {
            time = 0;
            timerText.text = $" Time : {Mathf.Round(time).ToString()}";
            losePanel.gameObject.SetActive(true);
        }
    }
}
