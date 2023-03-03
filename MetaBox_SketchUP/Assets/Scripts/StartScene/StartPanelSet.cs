using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPanelSet : MonoBehaviour
{
    [Header("[Start Panel]")]
    [SerializeField] Button gameStartBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button gotoTownBut = null;
    [SerializeField] Button exitBut = null;
    [SerializeField] Button tutorial = null;

    [Header("[ID Check]")]
    [SerializeField] TextMeshProUGUI id = null;

    void Awake()
    { 
        gameStartBut.onClick.AddListener(delegate {  OnClickStartBut(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(gameStartBut.transform.position); }); 

        optionBut.onClick.AddListener(delegate { OnClickOption(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(optionBut.transform.position); }); 

        tutorial.onClick.AddListener(delegate { OnClickTutorial(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(tutorial.transform.position);});

        exitBut.onClick.AddListener(delegate{ OnClickQuitBut(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(exitBut.transform.position);});
    }

    void Start()
    {
        id.text = LoadIDMgr.Inst.id;
    }

    public void OnClickStartBut()
    {
        StartSceneManager.Inst.LevelPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickOption()
    {
        StartSceneManager.Inst.OptionPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickTutorial()
    {
        StartSceneManager.Inst.TutorialPanelSet(true);
        this.gameObject.SetActive(false);
    }

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.QuitPanelSet(true);
        this.gameObject.SetActive(false);
    }
}