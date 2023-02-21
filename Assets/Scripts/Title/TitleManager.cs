using Kum;
using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Production;

public class TitleManager : MonoSingleTon<TitleManager>
{
    //caching
    WaitUntil waitUntil = null;

    [Header("[Production]")]
    [SerializeField] Production production;

    private new void Awake()
    {
        EventReceiver.saveCheck += AfterCheck;
        EventReceiver.touchScreen += SceneMove;
    }


    //=================save data check routine==========================
    void AfterCheck(bool exist)
    {
        Debug.Log("확인");
        StartCoroutine(nameof(ProductionRoutine), exist);
    }
    IEnumerator ProductionRoutine(bool exist)
    {
        Debug.Log("연출");
        production.DoProduction();

        yield return new WaitUntil(() => production.IsEnd); 

        if (exist) StartCoroutine(nameof(TitleRoutine));
        else StartCoroutine(nameof(InitRoutine));
    }
    //==================================================================


    //=================init routine==============================
    IEnumerator InitRoutine()
    {
        Debug.Log("초기설정!");
        EventReceiver.CallInit();

        yield return null;
    }

    void Title()
    {
        Debug.Log("준비완료");
        StartCoroutine(nameof(TitleRoutine));
    }
    IEnumerator TitleRoutine()
    {
        Debug.Log("메인씬!");
        EventReceiver.CallSelectDone();
        yield return null;
    }
    //=====================scene move
    void SceneMove()
    {
        SceneManager.LoadSceneAsync("TownScene");
    }
}
