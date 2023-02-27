using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolKum.AppTransition;
using System;
using System.IO;
using Newtonsoft.Json;

[Serializable]
public struct UserData
{
    public string id;
    public int charIndex;
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

    [SerializeField] private string localSavePath = "/MetaBox/SaveData/HCSaveData.json";
    public UserData curUserData;

    void Awake()
    {
        quitPanelOkBut.onClick.AddListener(delegate { AppQuit(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelOkBut.transform.position);
        });

        quitPanelQuitBut.onClick.AddListener(delegate { OnClickQuitBut();  
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelQuitBut.transform.position);
        });

        if (File.Exists(localSavePath))
        {
            curUserData = ReadSaveData(localSavePath);
        }
        else//존재하지 않을때 
        {
            curUserData.id = "전설의연습생";
        }
    }

    private UserData ReadSaveData(string path)
    {
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

        return readData;
    }

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.StartPanelSet(true);
        this.gameObject.SetActive(false);
    }

    private void AppQuit()
    {
        if (curUserData.id == "전설의연습생") Application.Quit(); 
        else AppTrans.MoveScene(mainPackName);
    }
}