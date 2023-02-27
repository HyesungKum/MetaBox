using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolKum.AppTransition;
using System;

[Serializable]
public struct UserData
{
    public bool troughTown;
}

public class QuitPanelSet : MonoBehaviour
{
    //================app transition============================
    [Header("[Application Setting]")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    [Header("[Quit Panel Button]")]
    [SerializeField] Button quitPanelOkBut = null;
    [SerializeField] Button quitPanelQuitBut = null;

    public UserData curUserData;

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

    private void AppQuit()
    {
        if (curUserData.troughTown) AppTrans.MoveScene(mainPackName);
        Application.Quit();
    }
}