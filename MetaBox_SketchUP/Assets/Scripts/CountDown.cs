using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] CountDownImgData countDownImgData = null;
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.5f), new Keyframe(1f, 1.5f) });
    Image countDownsImg = null;

    int index;
    void Awake()
    {
        if (countDownImgData == null)
            countDownImgData = Resources.Load<CountDownImgData>("Data/CountDownImgData");
    }

    public void ShowWaitTime(int index)
    {
        this.TryGetComponent<Image>(out countDownsImg);
        countDownsImg.sprite = countDownImgData.CountDownImg[index - 1];
        StartCoroutine(ShowCountDown());
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