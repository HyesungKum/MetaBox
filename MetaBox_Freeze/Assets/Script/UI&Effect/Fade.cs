using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] Loading loading = null; //Objects for loading presentation

    [Header("policeCar Control")]
    [SerializeField] Transform policeCar = null; //a policeCar showing the game screen
    [SerializeField] AnimationCurve FadeInCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f)});
    [SerializeField] AnimationCurve FadeOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1.8f, 0f)});

    Vector3 fadeScale = new Vector3(18, 18, 0.1f);

    void Start()
    {
        policeCar.localScale = fadeScale;
        StartCoroutine(nameof(_FadeIn));
    }

    public void FadeOut(int scene)
    {
        policeCar.localScale = fadeScale;
        StartCoroutine(_FadeOut(scene));
    }

    IEnumerator _FadeIn()
    {
        float startTime = 0;
        while (FadeInCurve.keys[FadeInCurve.keys.Length - 1].time >= startTime)
        {
            policeCar.localScale = fadeScale * FadeInCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        loading?.StartLoading();
    }

    IEnumerator _FadeOut(int scene)
    {
        float startTime = 0;
        while (FadeOutCurve.keys[FadeOutCurve.keys.Length - 1].time >= startTime)
        {
            policeCar.localScale = fadeScale * FadeOutCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }
}
