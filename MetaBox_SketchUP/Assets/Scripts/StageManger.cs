using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageRandomName = null;

    string StageOnebyOneName = "On a Starry Night";
    string StageOnebyTwoName = "Mona Lisa";

    int check = 2;

    void Start()
    {
        RandomSetting();
    }

    public void RandomSetting()
    {
        int random = Random.Range(1, 10);

        for(int i = 0; i < check; ++ i)
        {
            if(random < 5)
            { 
                stageRandomName.text = $"StageName : {StageOnebyOneName}";
                //Debug.Log("## 1) stageRandomName.text : " + stageRandomName.text);
            }
            else
            {
                stageRandomName.text = $"StageName : {StageOnebyTwoName}";
                //Debug.Log("## 2) stageRandomName.text : " + stageRandomName.text);
            }
        }
    }
}
