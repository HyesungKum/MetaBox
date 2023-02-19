using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] Loading loading = null; //Objects for loading presentation


    [Header("circle Control")]
    [SerializeField] GameObject circle = null; //a circle showing the game screen
    [SerializeField] AnimationCurve FadeInCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.4f, 1f)});
    [SerializeField] AnimationCurve FadeOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1.4f, 0f)});

    Vector3 fadeScale = new Vector3(25, 25, 1);

    void Start()
    {
        circle.transform.localScale = fadeScale;
        StartCoroutine(nameof(_FadeIn));
    }

    public void FadeOut()
    {
        circle.transform.localScale = fadeScale;
        StartCoroutine(nameof(_FadeOut));
    }

    IEnumerator _FadeIn()
    {
        float startTime = 0;
        while (FadeInCurve.keys[FadeInCurve.keys.Length - 1].time >= startTime)
        {
            circle.transform.localScale = fadeScale * FadeInCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        loading?.StartLoading();
    }

    IEnumerator _FadeOut()
    {
        float startTime = 0;
        while (FadeOutCurve.keys[FadeOutCurve.keys.Length - 1].time >= startTime)
        {
            circle.transform.localScale = fadeScale * FadeOutCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Loading");
    }
}
