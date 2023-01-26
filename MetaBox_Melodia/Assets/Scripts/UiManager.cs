
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UiManager : MonoBehaviour
{
    public static DelegateAudioControl myDelegateAudioControl;


    public delegate void DelegateUiManager(string myText);
    public static DelegateUiManager myDelegateUiManager;

    [SerializeField] TextMeshProUGUI myTextCountdown;    // play time countdown
    [SerializeField] TextMeshProUGUI myTextTimer;       // ready time countdown
    [SerializeField] TextMeshProUGUI myTextCorrectedNote;    // text whether get correct answer or not
    [SerializeField] TextMeshProUGUI myTextResult;       // GameResult panel

    [Header("Panel")]
    [SerializeField] GameObject myPaneUntouchable;
    [SerializeField] GameObject myPanelPause;
    [SerializeField] GameObject myPanelGameResult;
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
    bool isPaused = false;



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
        // touchable == panel off 
        myPaneUntouchable.SetActive(!status);
    }



    // text whether get correct answer or not
    void correctedNote(string text)
    {
        Debug.Log("맞췄나?");

        // show text 
        correctedNote(true);

        myTextCorrectedNote.text = text;

        // change color
        switch (text)
        {
            case "잘했어요!":
                {
                    Debug.Log("맞춤");

                    myTextCorrectedNote.color = new Color(0, 1, 0.3f);
                }
                break;


            case "다시 생각해봐요":
                {
                    Debug.Log("못맞춤");

                    myTextCorrectedNote.color = new Color(1, 0, 0.8f);
                }
                break;

            //default:
            //    {
            //        myTextCorrectedNote.enabled = false;
            //    }
            //    break;
                
        }

        calledTime = curTime;
        hideText();
    }


    void correctedNote(bool onOff)
    {
        Debug.Log("텍스트 보여줘");
        myTextCorrectedNote.enabled = onOff;
    }

    float calledTime;
    void hideText()
    {
        if (calledTime - curTime <= 0.4f)
        {
            Invoke("hideText", 0.4f);
            return;
        }

        correctedNote(false);
    }


    public void GameOver(string text)
    {
        myPanelGameResult.SetActive(true);
        //myButtonResult.onClick.AddListener(OnClickRestart);
        myButtonResult.GetComponentInChildren<TextMeshProUGUI>().text = "다시 할래";
        myTextResult.text = text;
    }


    public void NextStage(string text)
    {
        myPanelGameResult.SetActive(true);
        myButtonResult.GetComponentInChildren<TextMeshProUGUI>().text = "다음으로";
        myTextResult.text = text;
    }

    public void StageClear(string text)
    {
        myPanelStageClear.SetActive(true);
        myButtonResult.GetComponentInChildren<TextMeshProUGUI>().text = "또 할래";
        myTextResult.text = text;
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
                    NextStage("다 맞췄어요!");
                }
                break;
            case GameStatus.NoMorePlayableNote:
                {
                    GameOver("음표가 더이상 없어요");
                }
                break;

            case GameStatus.ClearStage:
                {
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






    // Option panel ==========================================================
    public void OnClickPause()
    {
        CurrentVolume();

        if (isPaused == false)
        {
            GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
            myPanelPause.SetActive(true);
            isPaused = true;

            return;
        }

        myPanelPause.SetActive(false);

        isPaused = false;
        GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
    }


    public void MasterAudioControl()
    {
        float volume = myAudioSliderMaster.value;

        myDelegateAudioControl("Master", volume);
    }

    public void BGMAudioControl()
    {
        float volume = myAudioSliderBGM.value;

        myDelegateAudioControl("BGM", volume);
    }

    public void SFXAudioControl()
    {
        float volume = myAudioSliderSFX.value;

        myDelegateAudioControl("SFX", volume);
    }


    void CurrentVolume()
    {
        float masterVolume;
        float bGMVolume;
        float sFXVolume;

        SoundManager.Inst.MyAudioMixer.GetFloat("Master", out masterVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("BGM", out bGMVolume);
        SoundManager.Inst.MyAudioMixer.GetFloat("SFX", out sFXVolume);

        myAudioSliderMaster.value = masterVolume;
        myAudioSliderBGM.value = bGMVolume;
        myAudioSliderSFX.value = sFXVolume;
    }


    public void OnClickQuitGame()
    {
        SoundManager.Inst.StopMusic();

        SceneManager.LoadScene("MelodiaLobby");
        Time.timeScale = 1;

        // back to start scene
        SceneModeController.MySceneMode = SceneModeController.SceneMode.StartScene;      // turn start panel on, turn lobby panel off


    }


    // fucntion for test, reload scene 
    public void OnClickExitStage()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 1;
        SceneManager.LoadScene("MelodiaLobby");

        // re-select mode 
        SceneModeController.MySceneMode = SceneModeController.SceneMode.LobbyScene;      // turn start panel off, turn lobby panel on

    }

    // Option panel ==========================================================


    // GameOver panel ==========================================================
    public void OnClickRestart()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 0;

        PlayTimer.DelegateTimer -= playCountDown;

        readyReplay();

        GameManager.Inst.UpdateCurProcess(GameStatus.Idle);
    }
    // GameOver panel ==========================================================







}
