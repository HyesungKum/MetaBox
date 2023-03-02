using Kum;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

[Serializable]
struct UserData
{
    public string ID;
    public int charIndex;
    public bool troughTown;
}


public class DataCheckManager : MonoSingleTon<DataCheckManager>
{
    [SerializeField] UserData curUserData;

    public int GetCharIndex() => curUserData.charIndex;
    public string GetID() => curUserData.ID;
    //====================================================save path===========================================================
    [SerializeField] string fileName = "SaveData.json";

    #if UNITY_EDITOR
    [SerializeField] private string defaultPath = "/MetaBox/SaveData/";
    #else
    private string defaultPath = "/storage/emulated/0/MetaBox/SaveData/";
    #endif

    //======================================================Flag==============================================================
    public bool IsExist = false;

    protected override void Awake()
    {
        base.Awake();

        EventReceiver.saveEvent += FileSave;
        EventReceiver.appQuit += AppQuit;
    }

    private void Start() => FileCheck();

    private void FileCheck()
    {
        if (File.Exists(defaultPath + fileName))
        {
            IsExist = true;
            curUserData = ReadSaveData(defaultPath + fileName);

            //call move main event
            EventReceiver.CallMainEvent();
        }
        else//존재하지 않을때 
        {
            IsExist = false;

            //call move Init event
            EventReceiver.CallInitEvent();
        }
    }
    private void FileSave(string id, int charIndex, bool troughTown)
    {
        UserData userData = new()
        {
            ID = id,
            charIndex = charIndex,
            troughTown = troughTown
        };

        curUserData = userData;

        MakeSaveJson(userData, fileName, defaultPath);

        EventReceiver.CallSaveDoneEvent();
    }

    //=======================================================SaveFile IO=================================================
    /// <summary>
    /// save any data in path to json,
    /// if u dont have path will be make,
    /// if u dont initialize path will be make default path
    /// </summary>
    /// <param name="data"> saving target </param>
    /// <param name="fileName"> saving json data name </param>
    /// <param name="path"> saving path </param>
    private void MakeSaveJson(UserData data, string fileName, string path)
    {
        if (Directory.Exists(path)) Directory.CreateDirectory(path);
        else Directory.CreateDirectory(path);

        //============make file=========================
        string jsonData = JsonConvert.SerializeObject(data);

        //turn off read only
        FileAttributes fas = File.GetAttributes(path);
        if ((fas & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) File.SetAttributes(path, FileAttributes.Normal);

        File.WriteAllText(path + fileName, jsonData);
    }
    /// <summary>
    /// read json file and transform saveData struct in path
    /// </summary>
    /// <param name="path"> read json data when file exist in this path</param>
    /// <returns> transform json data to SaveData </returns>
    private UserData ReadSaveData(string path)
    {
        Debug.Log(path);
        string dataStr = File.ReadAllText(path);
        UserData readData = JsonConvert.DeserializeObject<UserData>(dataStr);

        return readData;
    }
    /// <summary>
    /// if u have file in right path will be deleted
    /// </summary>
    /// <param name="filePath">file path include filename and extension</param>
    private void DelSaveData(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    //====================================================Application Quit===============================================
    private void AppQuit()
    {
        curUserData.troughTown = false;
        MakeSaveJson(curUserData, fileName, defaultPath);
        Application.Quit();
    }
}
