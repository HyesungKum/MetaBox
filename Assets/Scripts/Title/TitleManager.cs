using Kum;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoSingleTon<TitleManager>
{
    [SerializeField] Camera mainCam;
    private AudioListener mainAudioListener;

    [Header("[Production]")]
    [SerializeField] Production spreadProd;
    [SerializeField] Production lerpProd;

    //caching
    WaitUntil waitSpreadProdEnd = null;
    WaitUntil waitLerpProdEnd = null;

    private new void Awake()
    {
        //reference
        mainCam.TryGetComponent(out mainAudioListener);

        //caching
        waitSpreadProdEnd = new WaitUntil(() => spreadProd.IsEnd);
        waitLerpProdEnd = new WaitUntil(() => lerpProd.IsEnd);

        //delegate chain
        EventReceiver.initEvent += InitRoutine;
        EventReceiver.mainEvnet += MainRoutine;
        EventReceiver.touchScreen += UnLoadRoutine;

        EventReceiver.saveDoneEvent += CallUndoProduction;
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReceiver.initEvent -= InitRoutine;
        EventReceiver.mainEvnet -= MainRoutine;
        EventReceiver.touchScreen -= UnLoadRoutine;

        EventReceiver.saveDoneEvent -= CallUndoProduction;
    }

    void InitRoutine() => StartCoroutine(nameof(InitProcess));
    void MainRoutine() => StartCoroutine(nameof(LoadProcess));
    void UnLoadRoutine() => StartCoroutine(nameof(UnloadSceneProcess));
    void CallUndoProduction() => StartCoroutine(nameof(UndoProdRoutine));

    IEnumerator InitProcess()
    {
        spreadProd.DoProduction();

        yield return waitSpreadProdEnd;

        EventReceiver.CallSelectEvent();
    }
    IEnumerator LoadProcess()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TownScene", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        mainAudioListener.enabled = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress > 0.89f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (!spreadProd.gameObject.activeSelf)
        {
            spreadProd.gameObject.SetActive(true);
            spreadProd.IsEnd = true;
        }
        spreadProd.DoProduction();
        
        yield return waitSpreadProdEnd;

        EventReceiver.CallLodingDone();
    }
    IEnumerator UnloadSceneProcess()
    {
        spreadProd.gameObject.SetActive(false);
        lerpProd.DoProduction();
        yield return waitLerpProdEnd;
        EventReceiver.CallUnloadScene();
        SceneManager.UnloadSceneAsync("TitleScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }
    IEnumerator UndoProdRoutine()
    {
        spreadProd.gameObject.SetActive(true);
        spreadProd.UndoProduction();

        yield return waitSpreadProdEnd;

        StartCoroutine(nameof(LoadProcess));
    }
}
