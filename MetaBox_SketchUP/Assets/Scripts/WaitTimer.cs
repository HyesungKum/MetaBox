using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitTimer : MonoBehaviour
{
    TextMeshProUGUI startWaitTime = null;

    float waitTime = 3;
    bool wait = false;

    void OnEnable()
    {
        startWaitTime = gameObject.GetComponent<TextMeshProUGUI>();
        startWaitTime.text = "Game Start";
        wait = true;
    }

    void Update()
    {
        if (wait)
        {
            SetWaitTime();
            //Debug.Log("wait : " + wait);
        }
    }

    void SetWaitTime()
    {
        if (waitTime > 0)
        {
            waitTime -= 1 * Time.deltaTime;
            startWaitTime.text = $"Time : {Mathf.Round(waitTime).ToString()}";

            if (Mathf.Round(waitTime) == 0)
            {
                startWaitTime.text = "Go".ToString();
            }
        }
        else if (waitTime < 0)
        {
            this.gameObject.SetActive(false);
            wait = false;
            return;
        }
    }
}
