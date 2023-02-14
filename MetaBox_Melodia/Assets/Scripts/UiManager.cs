using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myTimer;

    [Header("Panel")]
    [SerializeField] GameObject myPanelUntouchable;
    [SerializeField] GameObject myPanelNextStage;
    [SerializeField] GameObject myPanelGameClear;
    [SerializeField] GameObject myPanelGameOver;
    [SerializeField] GameObject myPanelOption;

    [Header("Buttons")]
    [SerializeField] Button myButtonOption;
    [SerializeField] Button myButtonReplay;
    [SerializeField] Button myButtonNext;

    [SerializeField] TextMeshProUGUI playTime = null;

    public float ReplayCoolTime { get; set; }

    private void Awake()
    {
        myButtonOption.onClick.AddListener(OnClickOption);
        myButtonReplay.onClick.AddListener(OnClickReplay);
        myButtonNext.onClick.AddListener(OnClickNextStage);
        // observe game status 
        GameManager.Inst.myDelegateGameStatus += curGameStatus;

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
                    myPanelUntouchable.SetActive(false);
                }
                break;

            case GameStatus.TimeOver:
                {
                    myPanelGameOver.SetActive(true);
                }
                break;

            case GameStatus.GetAllQNotes:
                {
                    myPanelNextStage.SetActive(true);
                }
                break;

            case GameStatus.NoMorePlayableNote:
                {
                    myPanelGameOver.SetActive(true);
                }
                break;

            case GameStatus.GameResult:
                {
                    myPanelUntouchable.SetActive(true);
                }
                break;

            case GameStatus.ClearStage:
                {
                    myPanelGameClear.SetActive(true);
                    playTime.text = $"학습 시간 : {GameManager.Inst.MelodiaData.countDown - int.Parse(myTimer.text)}초";
                }
                break;
        }
    }

    public void StartGame()
    {
        PlayTimer.DelegateTimer = playTimer;

        // text for ready 
        myTimer.text = "Ready";

        // to disable touch interaction 
        myPanelUntouchable.SetActive(true);
        myPanelNextStage.SetActive(false);
        myPanelGameClear.SetActive(false);
        myPanelGameOver.SetActive(false);
        myPanelOption.SetActive(false);
        
    }



    // timer for start
    void playTimer(float t)
    {
        if (GameManager.Inst.CurStatus != GameStatus.StartGame && t <= 1)
        {
            myTimer.text = "Go!";
            return;
        }

        myTimer.text = Mathf.Round(t).ToString();
    }


    // Replay Music ==========================================================
    public void OnClickReplay()
    {
        PlayTimer.timerStop();

        SoundManager.Inst.PlayStageMusic();

        myButtonReplay.interactable = false;

        Invoke(nameof(readyReplay), ReplayCoolTime);
    }

    void readyReplay()
    {
        PlayTimer.timerStop();
        myButtonReplay.interactable = true;
    }
    // Replay Music ==========================================================






    public void OnClickOption()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
        myPanelOption.SetActive(true);
    }


    // Move to next stage ==========================================================
    public void OnClickNextStage()
    {
        Time.timeScale = 0;

        readyReplay();

        GameManager.Inst.UpdateCurProcess(GameStatus.Idle);
    }


    // Exit game and return to start scene ===========================================
    public void OnClickHome()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 1;
        SceneManager.LoadScene("Start");

    }


    // Restart stage from beginning =================================================
    public void OnClickReStart()
    {
        SoundManager.Inst.StopMusic();

        Time.timeScale = 0;

        readyReplay(); 

        GameManager.Inst.UpdateCurProcess(GameStatus.Restart);
    }

}
