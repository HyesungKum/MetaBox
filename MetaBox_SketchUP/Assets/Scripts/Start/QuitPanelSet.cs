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
        quitPanelOkBut.onClick.AddListener(delegate { AppQuit(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelOkBut.transform.position);
        });

        quitPanelQuitBut.onClick.AddListener(delegate { OnClickQuitBut();  
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelQuitBut.transform.position);
        });
    }

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.StartPanelSet(true);
        this.gameObject.SetActive(false);
    }

    private void AppQuit() => Application.Quit();
}
