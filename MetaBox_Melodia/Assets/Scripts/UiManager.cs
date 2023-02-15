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
        GameManager.Inst.DelegateTimer = playTimer;

    }

    private void Start()
    {
        ReplayCoolTime = GameManager.Inst.MelodiaData.replayCooltime;
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

            case GameStatus.GamePlaying:
                {
                    myPanelUntouchable.SetActive(false);
                }
                break;
            case GameStatus.Pause:
                {
                    myPanelUntouchable.SetActive(true);
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

            case GameStatus.TimeOver:
                {
                    myPanelGameOver.SetActive(true);
                }
                break;

            case GameStatus.GameClear:
                {
                    myPanelGameClear.SetActive(true);
                    playTime.text = $"학습 시간 : {GameManager.Inst.MelodiaData.countDown - GameManager.Inst.MyPlayableTime}초";
                }
                break;
        }
    }

    public void StartGame()
    {
        
        // text for ready 
        myTimer.text = "";

        // to disable touch interaction 
        myPanelUntouchable.SetActive(true);
        myPanelNextStage.SetActive(false);
        myPanelGameClear.SetActive(false);
        myPanelGameOver.SetActive(false);
        myPanelOption.SetActive(false);
        
    }



    // timer for start
    void playTimer(int t)
    {
        if (GameManager.Inst.CurStatus.Equals(GameStatus.Idle))
        {
            if (t == 999) myTimer.text = "Go!";
            else myTimer.text = t.ToString();

        }
        else myTimer.text = string.Format("{0:D2} : {1:D2} ", (t / 60), (t % 60));
    }


    // Replay Music ==========================================================
    public void OnClickReplay()
    {
        SoundManager.Inst.PlayStageMusic();

        myButtonReplay.interactable = false;

        Invoke(nameof(readyReplay), ReplayCoolTime);
    }

    void readyReplay()
    {
        myButtonReplay.interactable = true;
    }
    // Replay Music ==========================================================






    public void OnClickOption()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.Pause);
        myPanelOption.SetActive(true);
    }


    // Move to next stage ==========================================================
    void OnClickNextStage()
    {
        GameManager.Inst.UpdateCurProcess(GameStatus.Idle);
    }


    // Exit game and return to start scene ===========================================
    public void OnClickHome()
    {
        SoundManager.Inst.StopMusic();

        SceneManager.LoadScene("Start");

    }


    // Restart stage from beginning =================================================
    public void OnClickReStart()
    {
        SoundManager.Inst.StopMusic();

        GameManager.Inst.UpdateCurProcess(GameStatus.Restart);
    }

}
