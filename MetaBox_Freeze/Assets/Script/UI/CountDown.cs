using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] ScriptableObj countDownObj = null;
    [SerializeField] Image countDownImg = null;
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.5f), new Keyframe(1f, 1.5f) });

    public bool nomal { get; private set; } = true;
    public void Show(int number)
    {
        nomal = true;
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
            if (GameManager.Instance.reStart)
            {
                nomal = false;
                yield break;
            }
            yield return null;
        }
    }
}
