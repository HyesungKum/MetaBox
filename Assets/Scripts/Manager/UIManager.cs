using KumTool.AppTransition;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //current activation UI Object
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    [Header("UI Group")]
    [SerializeField] GameObject mainUIGroup;
    [SerializeField] Button optionButton;

    [Header("Game UI Group")]
    [SerializeField] GameObject heyCookUIGroup;
    [SerializeField] Button heyCookExitButton;
    [SerializeField] Button heyCookStartButton;
    [Space]
    [SerializeField] GameObject freezeUIGroup;
    [SerializeField] Button freezeExitButton;
    [SerializeField] Button freezeStartButton;
    [Space]
    [SerializeField] GameObject melodiaUIGroup;
    [SerializeField] Button melodiaExitButton;
    [SerializeField] Button melodiaStartButton;
    [Space]
    [SerializeField] GameObject sketchUPUIGroup;
    [SerializeField] Button sketchUPExitButton;
    [SerializeField] Button sketchUPStartButton;

    [Header("Option UI Group")]
    [SerializeField] GameObject optionUIGrpoup;
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SfxSlider;
    [SerializeField] Button optionResumeButton;
    [SerializeField] Button optionExitButton;

    [Header("ExitCheck UI Group")]
    [SerializeField] GameObject exitCheckUIGroup;
    [SerializeField] Button exitButton;
    [SerializeField] Button resumeButton;

    private const string defaultName = "com.MataBox.";

    private void Awake()
    {
        //main UI Button Listener
        optionButton.onClick.AddListener(()=> ShowUI(optionUIGrpoup));

        //Game UI Group button listener
        heyCookStartButton.onClick.AddListener(() => AppTrans.MoveScene("com.MetaBox.HeyCook"));
        heyCookExitButton.onClick.AddListener(()=> ShowUI(mainUIGroup));

        freezeStartButton.onClick.AddListener(() => AppTrans.MoveScene("com.MetaBox.Freeze"));
        freezeExitButton.onClick.AddListener(()=> ShowUI(mainUIGroup));

        melodiaStartButton.onClick.AddListener(() => AppTrans.MoveScene("com.MetaBox.Melodia"));
        melodiaExitButton.onClick.AddListener(()=> ShowUI(mainUIGroup));

        sketchUPStartButton.onClick.AddListener(() => AppTrans.MoveScene("com.MetaBox.SketchUP"));
        sketchUPExitButton.onClick.AddListener(()=> ShowUI(mainUIGroup));

        //option UI Group Listener
        BgmSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("BGM", call));
        SfxSlider.onValueChanged.AddListener((call) => SoundManager.Inst.VolumeControll("SFX", call));
        optionResumeButton.onClick.AddListener(() => ShowUI(mainUIGroup));
        optionExitButton.onClick.AddListener(()=>ShowUI(exitCheckUIGroup));

        //Exit UI Group button Listener
        exitButton.onClick.AddListener(()=> EventReceiver.CallAppQuit());
        resumeButton.onClick.AddListener(()=> ShowUI(mainUIGroup));

        //delegate chain
        EventReceiver.gameIn += OpenGamePanel;
    }

    private void OnDisable()
    {
        EventReceiver.gameIn -= OpenGamePanel;
    }

    void OpenGamePanel(string gameName)
    {
        switch (gameName)
        {
            case GameName.HeyCook :
                {
                    SoundManager.Inst.CallSfx("HeyCookSfx");
                    ShowUI(heyCookUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled($"com.MetaBox.HeyCook")) heyCookStartButton.interactable = true;
                    else heyCookStartButton.interactable = false;
#endif
                }
                break;
            case GameName.Freeze  :
                {
                    SoundManager.Inst.CallSfx("FreezeSfx");
                    ShowUI(freezeUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled($"com.MetaBox.Freeze")) freezeStartButton.interactable = true;
                    else freezeStartButton.interactable = false;
#endif
                }
                break;
            case GameName.Melodia :
                {
                    SoundManager.Inst.CallSfx("MelodiaSfx");
                    ShowUI(melodiaUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled("com.MetaBox.Melodia")) melodiaStartButton.interactable = true;
                    else melodiaStartButton.interactable = false;
#endif
                }
                break;
            case GameName.SketchUP :
                { 
                    SoundManager.Inst.CallSfx("SketchUPSfx");
                    ShowUI(sketchUPUIGroup);

#if UNITY_ANDROID && !UNITY_EDITOR
                    if (PackageChecker.IsAppInstalled("com.MetaBox.SketchUP")) sketchUPStartButton.interactable = true;
                    else melodiaStartButton.interactable = false;
#endif
                }
                break;
        }
    }

    void ShowUI(GameObject targetUIObj)
    {
        if(curUI.activeSelf) curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }
}
