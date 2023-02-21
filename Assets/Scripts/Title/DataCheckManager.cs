using Kum;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] string fileName = "saveData.json";

    [SerializeField] private string defaultPath = "MetaBox/SaveData/";//"/storage/emulated/0/MetaBox/SaveData/";

    //======================================================Flag==============================================================
    public bool IsExist = false;

    protected override void Awake()
    {
        base.Awake();
        EventReceiver.save += FileSave;
    }

    private void Start()
    {
        FileCheck();
    }

    private void FileCheck()
    {
        if (File.Exists(defaultPath + fileName)) //경로에 파일이 존재할때
        {
            IsExist = true; //변수 변경 후 
            Debug.Log("바로 main Scene으로");

            //읽어오기
            curUserData = ReadSaveData(defaultPath + fileName);
        }
        else//존재하지 않을때 
        {
            IsExist = false;
            Debug.Log("Init Scene으로");
        }

        //세이브 확인 이벤트 발생
        EventReceiver.CallSaveCheck(IsExist);
    }
    private void FileSave(string id, int charIndex, bool troughTown)
    {
        UserData userData = new()
        {
            ID = id,
            charIndex = charIndex,
            troughTown = troughTown
        };

        MakeSaveJson(userData, fileName, defaultPath);
    }

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

    //=======================================================SaveFile IO=================================================
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
}
