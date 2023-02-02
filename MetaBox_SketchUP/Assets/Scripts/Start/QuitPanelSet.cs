using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuitPanelSet : MonoBehaviour
{
    [Header("[Quit Panel Button]")]
    [SerializeField] Button quitPanelOkBut = null;
    [SerializeField] Button quitPanelQuitBut = null;

    void Awake()
    {
        quitPanelOkBut.onClick.AddListener(delegate { AppQuit(); SoundManager.Inst.ButtonSFXPlay(); });
        quitPanelQuitBut.onClick.AddListener(delegate { OnClickQuitBut();  SoundManager.Inst.ButtonSFXPlay(); });
    }

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.StartPanelSet(true);
        this.gameObject.SetActive(false);
    }

    private void AppQuit()
    {
        Application.Quit();
    }
}
