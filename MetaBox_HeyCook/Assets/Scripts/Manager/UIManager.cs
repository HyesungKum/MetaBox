using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    //================in game ui===================
    [Header("[In Game UI]")]
    [SerializeField] GameObject inGameUI;
    [Space]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score;
    [Space]
    [SerializeField] Button optionButton;

    //=================option ui===================
    [Header("[Option UI]")]
    [SerializeField] GameObject optionUI;
    [Space]
    [SerializeField] Button masterSoundButton;
    [SerializeField] Slider masterSlider;
    [Space]
    [SerializeField] Button bgmSoundButton;
    [SerializeField] Slider bgmSlider;
    [Space]
    [SerializeField] Button sfxSoundButton;
    [SerializeField] Slider sfxSlider;
    [Space]
    [SerializeField] Button opRestartButton;
    [SerializeField] Button opResumeButton;
    [Space]
    [SerializeField] Button opExitButton;

    //=================end game ui=================
    [Header("[Game Over UI]")]
    [SerializeField] GameObject gameOverUI;
    [Space]
    [SerializeField] Button restartButton;
    [SerializeField] Button endExitButton;

    private void Awake()
    {
        //===============in game button listener=======================================
        optionButton.onClick.AddListener(() => StaticEventReciver.CallGamePause());
        optionButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //===============option button listener========================================
        opRestartButton.onClick.AddListener(() => SceneMove(SceneName.Main));
        opRestartButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        masterSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("Master", masterSlider.value));
        masterSoundButton.onClick.AddListener(() => ToggleSlider(masterSlider));
        masterSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("Master", call));

        bgmSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("BGM", bgmSlider.value));
        bgmSoundButton.onClick.AddListener(() => ToggleSlider(bgmSlider));
        bgmSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));

        sfxSoundButton.onClick.AddListener(() => SoundManager.Inst.ToggleControll("SFX", sfxSlider.value));
        sfxSoundButton.onClick.AddListener(() => ToggleSlider(sfxSlider));
        sfxSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));

        opResumeButton.onClick.AddListener(() => StaticEventReciver.CallGameResume());
        opResumeButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        opExitButton.onClick.AddListener(() => SceneMove(SceneName.Start));
        opExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //===============game end button listener======================================
        endExitButton.onClick.AddListener(() => SceneMove(SceneName.Start));
        endExitButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        restartButton.onClick.AddListener(() => SceneMove(SceneName.Main));
        restartButton.onClick.AddListener(() => SoundManager.Inst.CallSound("ButtonClick"));

        //delegate chain
        StaticEventReciver.ScoreModi += UIScoreModi;
        StaticEventReciver.GamePause += UIGamePause;
        StaticEventReciver.GameResume += UIGameResume;
        StaticEventReciver.GameOver += UIGameOver;
    }

    private void Update()
    {
        //이벤트 추가로 업데이트에서 뽑기
        timer.text = string.Format("{0:D2} : {1:D2} ",(int)(GameManager.Inst.Timer/60f),(int)(GameManager.Inst.Timer % 60f));
    }

    private void OnDisable()
    {
        //delegate unchain
        StaticEventReciver.ScoreModi -= UIScoreModi;
        StaticEventReciver.GamePause -= UIGamePause;
        StaticEventReciver.GameResume -= UIGameResume;
        StaticEventReciver.GameOver -= UIGameOver;
    }

    //=============================================UI Viewing Controll==================================
    void UIGamePause()
    {
        ShowUI(optionUI);
    }
    void UIGameResume()
    {
        ShowUI(inGameUI);
    }
    void UIGameOver()
    {
        ShowUI(gameOverUI);
    }
    void ShowUI(GameObject targetUIObj)
    {
        curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    //=============================================UI Value Vontroll======================================
    void ToggleSlider(Slider target)
    {
        target.interactable = target.interactable == true ? false : true;
    }
    /// <summary>
    /// modify ui score text controll
    /// </summary>
    /// <param name="value">target time value</param>
    void UIScoreModi(int value)
    {
        score.text = GameManager.Inst.Score.ToString();
    }

    //================================================SceneMove============================================
    void SceneMove(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
