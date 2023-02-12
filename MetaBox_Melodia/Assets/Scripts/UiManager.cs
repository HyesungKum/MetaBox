
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UiManager : MonoBehaviour
{
    public delegate void DelegateUiManager(string myText);
    public static DelegateUiManager myDelegateUiManager;

    [SerializeField] TextMeshProUGUI myTextCountdown;    // play time countdown
    [SerializeField] TextMeshProUGUI myTextTimer;       // ready time countdown
    [SerializeField] TextMeshProUGUI myTextCorrectedNote;    // text whether get correct answer or not
    [SerializeField] TextMeshProUGUI myTextGameOver;       // GameResult panel
    [SerializeField] TextMeshProUGUI myTextResult;       // GameResult panel
    [SerializeField] TextMeshProUGUI myTextClear;

    [Header("Panel")]
    [SerializeField] GameObject myPaneUntouchable;
    [SerializeField] GameObject myPanelPause;
    [SerializeField] GameObject myPanelGameResult;
    [SerializeField] GameObject myPanelNextStage;
    [SerializeField] GameObject myPanelStageClear;


    [Header("Audio Volume Control")]
    [SerializeField] Slider myAudioSliderMaster;
    [SerializeField] Slider myAudioSliderBGM;
    [SerializeField] Slider myAudioSliderSFX;


    [Header("Buttons")]
    [SerializeField] Button myButtonReplay;
    [SerializeField] Button myButtonQuitGame;
    [SerializeField] Button myButtonResult;




    [SerializeField] float replayCoolTime;
    public float ReplayCoolTime { set { replayCoolTime = value; } }



    float curTime;



    private void Start()
    {
        // observe game status 
        GameManager.myDelegateGameStatus += curGameStatus;

        // delegate chain
        myDelegateUiManager = correctedNote;
    }

    public void StartGame()
    {
        PlayTimer.DelegateTimer = playTimer;

        // text whether get correct answer or not
        myTextCorrectedNote.enabled = false;

        // text for time countdown
        myTextCountdown.text = "";
        myTextCountdown.enabled = false;

        // text for time count 
        myTextTimer.enabled = true;

        // text for ready 
        myTextTimer.text = "Ready";

        // to disable touch interaction 
        Touchable(false);


        // all panel off
        // no show pause panel
        myPanelPause.SetActive(false);

        // Game result panel 
        myPanelGameResult.SetActive(false);

        // stage clear panel
        myPanelStageClear.SetActive(false);

        // Next stage panel 
        myPanelNextStage.SetActive(false);


        myTextResult.enabled = false;
    }



    // timer for start
    void playTimer(float t)
    {
        if (t <= 1)
        {
            myTextTimer.text = "Go!";
            PlayTimer.DelegateTimer += playCountDown;
            PlayTimer.DelegateTimer -= playTimer;
            return;
        }

        myTextTimer.text = Mathf.Round(t).ToString();
    }


    // time count down for play time 
    public void playCountDown(float t)
    {
        curTime = t;


        if (myTextTimer.isActiveAndEnabled == true)
        {
            myTextTimer.enabled = false;

            myTextCountdown.enabled = true;
        }

        myTextCountdown.text = Mathf.Round(t).ToString();
    }


    public void Touchable(bool status)
    {
        // touchable false == panel on
        // touchable true == panel off
        myPaneUntouchable.SetActive(!status);
    }



    // text whether get correct answer or not
    void correctedNote(string text)
    {
        // show text 
        correctedNote(true);

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
        if (calledTime - curTime <= 0.4f)
        {
            Invoke(nameof(hideText), 0.4f);
            return;
        }

        correctedNote(false);
    }


    void resultText()
    {
        myTextResult.enabled = true;
        myTextResult.text = "와 다 맞췄어요!";
    }



    public void GameOver(string text)
    {
        myPanelGameResult.SetActive(true);

        myButtonResult.GetComponentInChildren<TextMeshProUGUI>().text = "다시 할래";
        myTextGameOver.text = text;
    }


    public void NextStage()
    {
        myTextResult.enabled = false;


        myPanelNextStage.SetActive(true);
    }

    public void StageClear(string text)
    {
        myTextResult.enabled = false;


        myPanelStageClear.SetActive(true);
        myTextClear.text = text;
    }



    void curGameStatus(GameStatus curStatus)
    {
        switch (curStatus)
        {
            case GameStatus.Idle:
                {
                    StartGame();
                }
                break;

            case GameStatus.Ready:
                {
                    ReplayCoolTime = GameManager.Inst.MyCoolTime;
                }
                break;

            case GameStatus.StartGame:
                {
                    Touchable(true);
                }
                break;

            case GameStatus.TimeOver:
                {
                    GameOver("시간이 없어요");
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    NextStage();
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    GameOver("음표가 더이상 없어요");
                }
                break;


            case GameStatus.GameResult:
                {
                    // myPaneUntouchable is activated 
                    Touchable(false);

                    // hide correct text
                    correctedNote(false);

                    resultText();
                }
                break;

            case GameStatus.ClearStage:
                {
                    Touchable(true);

                    StageClear("와! 성공했어요!");
                }
                break;
        }
    }




    // Replay Music ==========================================================
    public void OnClickReplay()
    {
        SoundManager.Inst.PlayStageMusic();

        myButtonReplay.interactable = false;

        Invoke("readyReplay", replayCoolTime);
    }

    void readyReplay()
    {
        myButtonReplay.interactable = true;
    }
    // Replay Music ==========================================================






    public void OnClickPause()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
        myPanelPause.SetActive(true);
    }



    // Exit game and return to level select scene ===========================================
    public void OnClickQuitGame()
    {
        SoundManager.Inst.StopMusic();

        SceneManager.LoadScene("MelodiaLobby");
        Time.timeScale = 1;

        // back to start scene
        //SceneModeController.MySceneMode = SceneMode.StartScene;      // turn start panel on, turn lobby panel off
    }




    // Exit game and return to start scene ===========================================
    public void OnClickExitStage()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaLobby");

        // re-select mode 
        //SceneModeController.MySceneMode = SceneMode.LobbyScene;      // turn start panel off, turn lobby panel on

    }




    // Restart stage from beginning =================================================
    public void OnClickReStart()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 0;

        PlayTimer.DelegateTimer -= playCountDown;

        readyReplay(); 



        GameManager.Inst.UpdateCurProcess(GameStatus.Restart);
    }


    // Move to next stage ==========================================================
    public void OnClickNextStage()
    {
        Time.timeScale = 0;

        PlayTimer.DelegateTimer -= playCountDown;

        readyReplay();

        GameManager.Inst.UpdateCurProcess(GameStatus.Idle);
    }






}
