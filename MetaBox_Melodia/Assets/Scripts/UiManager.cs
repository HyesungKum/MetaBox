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
    public delegate void DelegateUiManager(string myText);
    public static DelegateUiManager myDelegateUiManager;

    [Header("Button")]
    [SerializeField] Button myButtonReplayMusic;        // replay music for hint
    [SerializeField] Button myButtonClickInventory;     // open note box

    [SerializeField] TextMeshProUGUI myTextCountdown;    // play time countdown
    [SerializeField] TextMeshProUGUI myTextTimer;       // ready time countdown
    [SerializeField] TextMeshProUGUI myTextCorrectedNote;    // text whether get correct answer or not
    [SerializeField] TextMeshProUGUI myTextResult;       // Result panel

    [Header("Panel")]
    [SerializeField] GameObject myPaneUntouchable;
    [SerializeField] GameObject myPanelPause;
    [SerializeField] GameObject myPanelGameResult;

    float curTime;
    bool isPaused = false;

    private void Awake()
    {
        // delegate chain
        myDelegateUiManager = correctedNote;
        myDelegateUiManager += correctedNote;
        PlayTimer.DelegateTimer = playTimer;

        // Game result panel 
        myPanelGameResult.SetActive(false);

        // text whether get correct answer or not
        myTextCorrectedNote.enabled = false;

        // text for time count 
        myTextTimer.enabled = true;

        // to disable touch interaction 
        myPaneUntouchable.SetActive(true);

        // no show pause panel
        myPanelPause.SetActive(false);

        // text for ready 
        myTextTimer.text = "Ready";
    }


    // timer for start
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


    // time count down for play time 
    public void playCountdown(float t)
    {
        curTime = t;

        if (myTextTimer.isActiveAndEnabled == true)
        {
            myTextTimer.enabled = false;

            myPaneUntouchable.SetActive(false);
        }

        myTextCountdown.text = Mathf.Round(curTime).ToString();
    }


    // text whether get correct answer or not
    void correctedNote(string text)
    {
        // show text 
        myTextCorrectedNote.enabled = true;
        myTextCorrectedNote.text = text;

        // change color
        switch (text)
        {
            case "잘했어요!":
                {
                    myTextCorrectedNote.color = new Color(0, 1, 0.3f);
                }
                break;


            case "다시 생각해봐요":
                {
                    myTextCorrectedNote.color = new Color(1, 0, 0.8f);
                }
                break;

            default:
                {
                    myTextCorrectedNote.enabled = false;
                }
                return;
        }

        calledTime = curTime;
        hideText();
    }


    void correctedNote(bool onOff)
    {
        myTextCorrectedNote.enabled = onOff;
    }

    float calledTime;
    void hideText()
    {
        if (calledTime - curTime <= 0.5f)
        {
            Invoke("hideText", 0.5f);
            return;
        }

        myTextCorrectedNote.enabled = false;

    }


    public void GameResult(string text)
    {
        myPanelGameResult.SetActive(true);
        myTextResult.text = text;
    }


    public void OnClickPause()
    {
        if (isPaused == false)
        {
            Time.timeScale = 0f;
            myPanelPause.SetActive(true);
            isPaused = true;

            return;
        }

        Time.timeScale = 1;
        myPanelPause.SetActive(false);

        isPaused = false;
    }

}
