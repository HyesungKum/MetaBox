using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;
    [SerializeField] Slider loadingBar = null;
    [SerializeField] TextMeshProUGUI loadingprogress = null;

    [Header("bird Control")]
    [SerializeField] Transform bird = null; //a bird showing the game screen
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

        bird.localScale = fadeScale;

        while (!asyncLoad.allowSceneActivation)
        {
            loadingBar.value = asyncLoad.progress;
            loadingprogress.text = string.Format("{0}%", (int)(asyncLoad.progress * 100));

            if (asyncLoad.progress >= 0.89)
            {
                yield return StartCoroutine(nameof(_FadeOut));
            }
            yield return null;
        }
        
    }
    IEnumerator _FadeOut()
    {
        loadingBar.value = 1;
        loadingprogress.text = "100%";
        loadingBar.gameObject.SetActive(false);

        float startTime = 0;
        while (FadeOutCurve.keys[FadeOutCurve.keys.Length - 1].time >= startTime)
        {
            bird.localScale = fadeScale * FadeOutCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

}

