using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
    TextMeshProUGUI timerText = null;

    float time = 600;

    void Awake()
    {
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
            Debug.Log("##게임 시간 종료 !!");
            return;
        }
    }
}
