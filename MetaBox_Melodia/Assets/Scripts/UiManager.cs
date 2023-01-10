using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    [SerializeField] Button myButtonReplayMusic;        // replay music for hint
    [SerializeField] Button myButtonClickInventory;     // open note box

    [SerializeField] Button myButtonRestart4Test;       // re-load scene for test

    [SerializeField] TextMeshProUGUI myTextCountdown;    // play time countdown
    [SerializeField] TextMeshProUGUI myTextTimer;       // ready time countdown

    [SerializeField] TextMeshProUGUI myTextResult;       // Result panel


    public GameObject myPanel;
    public GameObject myGameResultPanel;

    private void Awake()
    {
        myGameResultPanel.SetActive(false);
        PlayTimer.DelegateTimer = playTimer;

        myTextTimer.enabled = true;
        myPanel.SetActive(true);

        myTextTimer.text = "Ready";
    }


    void playTimer(float t)
    {
        if (t <= 1)
        {
            myTextTimer.text = "Go!";
            PlayTimer.DelegateTimer += playCountdown;
            PlayTimer.DelegateTimer -= playTimer;
            return;
        }

        myTextTimer.text = Mathf.Round(t).ToString();
    }

    public void playCountdown(float t)
    {
        if (myTextTimer.isActiveAndEnabled == true)
        {
            myTextTimer.enabled = false;

            myPanel.SetActive(false);
        }

        myTextCountdown.text = Mathf.Round(t).ToString();
    }


    public void GameResult(string text)
    {
        myGameResultPanel.SetActive(true);
        myTextResult.text = text;
    }

}
