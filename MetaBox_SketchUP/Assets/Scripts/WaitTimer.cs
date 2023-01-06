using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitTimer : MonoBehaviour
{
    TextMeshProUGUI startWaitTime = null;

    float waitTime = 3f;
    //int waitTime = 3;

    void OnEnable()
    {
        startWaitTime = gameObject.GetComponent<TextMeshProUGUI>();
        startWaitTime.text = "Game Start";
    }
    
    void Update()
    {
        SetWaitTime();

    }

    void SetWaitTime()
    {
        waitTime -= 1 * Time.deltaTime;
        startWaitTime.text = $"Time : {Mathf.Round(waitTime).ToString()}";
        //Debug.Log("## waiteTime : " + waitTime);

        if (waitTime <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}
