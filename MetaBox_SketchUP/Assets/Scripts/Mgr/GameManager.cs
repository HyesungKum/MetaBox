using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    //private static GameManager instance;
    //public static GameManager Inst
    //{
    //    get
    //    {
    //        if(instance == null)
    //        {
    //            instance = FindObjectOfType<GameManager>();
    //            if(instance == null)
    //            {
    //                instance = new GameObject(nameof(GameManager), typeof(GameManager)).GetComponent<GameManager>();
    //            }
    //        }
    //        return instance;
    //    }
    //}
    #endregion

    private float playerPlayTime = 600;
    public float PlayerPlayTime
    { get { return playerPlayTime; } set { playerPlayTime = value; }}


    void Start()
    {
        
    }

    void CheckPlayTime()
    {
        float beforeTime = 0;
        float checkTime = PlayerPlayTime - InGamePanelSet.Inst.PlayTime;

        if(checkTime < beforeTime)
        {
            Debug.Log("더 빨리 풀었습니다 !!");
        }
    }
}
