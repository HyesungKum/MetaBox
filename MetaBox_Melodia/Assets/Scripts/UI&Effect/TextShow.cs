using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextShow : MonoBehaviour
{
    [Header("ImgText Control")]
    //[SerializeField] Image text = null; //a bird showing the game screen
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.9f, 1f) });

    private void OnEnable()
    {
        StartCoroutine(nameof(_TextShow));
    }

    IEnumerator _TextShow()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            this.transform.localScale = Vector3.one * ScaleCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
