using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] ScriptableObj countDownObj = null;
    [SerializeField] Image countDownImg = null;
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.5f), new Keyframe(0.95f, 1f) });

    public void Show(int number)
    {
        countDownImg.sprite = countDownObj.CountDown[number-1];
        StartCoroutine(nameof(ShowCountDown));
    }
    IEnumerator ShowCountDown()
    {
        float startTime = 0;
        Vector3 curScale;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            curScale = Vector3.one * ScaleCurve.Evaluate(startTime);
            transform.localScale = curScale;

            startTime += Time.deltaTime;

            yield return null;
        }
    }
}
