using Kum;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoSingleTon<LoadingManager>
{
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    [Header("[UI Referance]")]
    [SerializeField] GameObject ProgressingUI;
    [SerializeField] Slider ProgressBar;
    [Space]
    [SerializeField] GameObject ProgressDoneUI;
    [SerializeField] Button StartButton;

    [Header("[Production]")]
    [SerializeField] GameObject production;
    [SerializeField] GameObject viewHall;

    //================================async progress==========================
    private AsyncOperation operation;

    //==================================Caching===============================
    float secPerFrame;

    private new void Awake()
    {
        secPerFrame = Time.deltaTime;
        StartButton.onClick.AddListener(() => SceneOutReady());
        StartButton.onClick.AddListener(() => EventReceiver.CallButtonClicked());
    }

    private void Start()
    {
        StartCoroutine(nameof(SceneIncome));
    }
    IEnumerator SceneIncome()
    {
        yield return StartCoroutine(nameof(ViewHallExtension));
        yield return StartCoroutine(nameof(Progressing));
    }

    //============================UI Object Controll====================================
    void ShowUI(GameObject targetUIObj)
    {
        if(curUI != null) curUI.SetActive(false);

        curUI = targetUIObj;
        curUI.SetActive(true);
    }
    IEnumerator ViewHallExtension()
    {
        production.SetActive(true);
    
        float timer = 0f;
        while (viewHall.transform.localScale.x <= 45)
        {
            timer += Time.deltaTime / 15f;
    
            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.one * 46f, timer);
            yield return null;
        }
    
        viewHall.transform.localScale = Vector3.one * 45f;
        production.SetActive(false);

        //start async loading
        operation = SceneManager.LoadSceneAsync(SceneName.Main);
        operation.allowSceneActivation = false;
    }

    //=============================progress controll====================================
    IEnumerator Progressing()
    {
        yield return new WaitUntil(() => operation != null);

        while (true)
        {
            ProgressBar.value = operation.progress;

            if (operation.progress >= 0.89)
            {
                StartCoroutine(nameof(FinalFill));
                yield break;
            }

            yield return null;
        }
    }
    IEnumerator FinalFill()
    {
        while (ProgressBar.value < 0.98f)
        {
            ProgressBar.value = Mathf.Lerp(ProgressBar.value, 1f, secPerFrame);
            yield return null;
        }

        ProgressBar.value = 1f;
        ShowUI(ProgressDoneUI);
    }

    //=================================Scene Out=======================================
    void SceneOutReady()
    {
        StartCoroutine(nameof(ViewHallShrink));
    }
    IEnumerator ViewHallShrink()
    {
        production.SetActive(true);
    
        float timer = 0f;
        while (viewHall.transform.localScale.x >= 0.8f)
        {
            timer += Time.deltaTime / 15f;
    
            viewHall.transform.localScale = Vector3.Lerp(viewHall.transform.localScale, Vector3.zero, timer);
            yield return null;
        }
    
        viewHall.transform.localScale = Vector3.zero;

        operation.allowSceneActivation = true;
    }
}
