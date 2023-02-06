using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] ScriptableObj scriptableCountDown = null;
    [SerializeField] Image countDownImg = null;
    [SerializeField] AnimationCurve PosXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 7f), new Keyframe(0.9f, -1.5f) });
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.1f), new Keyframe(0.9f, 1f) });

    Vector3 curPos;
    Vector3 curScale;

    public void Show(int number)
    {
        curPos = this.transform.position;
        countDownImg.sprite = scriptableCountDown.CountDown[number-1];
        StartCoroutine(nameof(ShowCountDown));
    }
    IEnumerator ShowCountDown()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            curPos.x = PosXCurve.Evaluate(startTime);
            this.transform.position = curPos;

            curScale = Vector3.one * ScaleCurve.Evaluate(startTime);
            transform.localScale = curScale;

            startTime += Time.deltaTime;

            yield return null;
        }
    }
}
