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

    void Awake()
    {
        losePanel.gameObject.SetActive(false);
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
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

        if (time <= 0)
        {
            losePanel.gameObject.SetActive(true);
        }
    }
}
