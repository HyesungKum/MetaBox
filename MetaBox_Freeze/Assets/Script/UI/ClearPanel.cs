using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI clearPlayTime = null;
    [SerializeField] Button home = null;

    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(4f, 1f) });
    [SerializeField] AnimationCurve PosXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -5f), new Keyframe(4f, 0f) });
    [SerializeField] AnimationCurve PosYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 3f), new Keyframe(4f, 0f) });

    Vector3 oriScale = new Vector3(1, 1, 1);
    Vector3 oriPos = new Vector3(0, 0, 0);

    void Awake()
    {
        home.onClick.AddListener(() => SceneManager.LoadScene("Start"));
    }
    
    void Start()
    {
        clearPlayTime.text = (GameManager.Instance.FreezeData.playTime - GameManager.Instance.PlayTime).ToString();
        StartCoroutine(nameof(ClearPanelShow));
    }
    IEnumerator ClearPanelShow()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            this.transform.localScale = oriScale * ScaleCurve.Evaluate(startTime);
            oriPos.x = PosXCurve.Evaluate(startTime);
            oriPos.y = PosYCurve.Evaluate(startTime);
            this.transform.position = oriPos;

            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
