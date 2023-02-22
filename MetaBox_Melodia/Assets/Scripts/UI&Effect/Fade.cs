using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [Header("bird Control")]
    [SerializeField] Transform bird = null; //a bird showing the game screen
    [SerializeField] AnimationCurve FadeInCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f)});
    [SerializeField] AnimationCurve FadeOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1.8f, 0f)});

    Vector3 fadeScale = new Vector3(15, 15, 1);

    void Start()
    {
        bird.localScale = fadeScale;
        StartCoroutine(nameof(_FadeIn));
    }

    public void FadeOut()
    {
        bird.localScale = fadeScale;
        StartCoroutine(nameof(_FadeOut));
    }

    IEnumerator _FadeIn()
    {
        float startTime = 0;
        while (FadeInCurve.keys[FadeInCurve.keys.Length - 1].time >= startTime)
        {
            bird.localScale = fadeScale * FadeInCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator _FadeOut()
    {
        float startTime = 0;
        while (FadeOutCurve.keys[FadeOutCurve.keys.Length - 1].time >= startTime)
        {
            bird.localScale = fadeScale * FadeOutCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
        SoundManager.Inst.BGMPlay(1);
        SceneManager.LoadScene("Start");
    }
}
