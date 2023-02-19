using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;
    [SerializeField] Slider loadingBar = null;
    [SerializeField] TextMeshProUGUI loadingprogress = null;

    [Header("bird Control")]
    [SerializeField] GameObject bird = null; //a bird showing the game screen
    [SerializeField] AnimationCurve FadeOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1.8f, 0f) });

    Vector3 fadeScale = new Vector3(15, 15, 1);

    void Start()
    {
        StartCoroutine(nameof(Load));
    }

    IEnumerator Load()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Melodia");
        asyncLoad.allowSceneActivation = false;

        bird.transform.localScale = fadeScale;

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            loadingprogress.text = string.Format("{0}%", (int)(asyncLoad.progress * 100));

            if (asyncLoad.progress >= 0.89)
            {
                yield return StartCoroutine(nameof(_FadeOut));
                yield break;
            }
            yield return null;
        }
        
    }
    IEnumerator _FadeOut()
    {
        loadingBar.value = 1;
        loadingprogress.text = "100%";
        loadingBar.gameObject.SetActive(false);
        loadingprogress.gameObject.SetActive(false);

        float startTime = 0;
        while (FadeOutCurve.keys[FadeOutCurve.keys.Length - 1].time >= startTime)
        {
            bird.transform.localScale = fadeScale * FadeOutCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

}

