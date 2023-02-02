using System.Collections;
using UnityEngine;

public class WantedPost : MonoBehaviour
{
    [SerializeField] GameObject thiefImg = null;
    [SerializeField] GameObject arrestImg = null;
    [SerializeField] AnimationCurve PostPosYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -1f), new Keyframe(1.2f, 3f) });
    [SerializeField] AnimationCurve PostRotCurve;
    [SerializeField] AnimationCurve PostScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.2f, 1f) });

    Vector3 postPos = new Vector3(7.2f, -1f, 0);
    Vector3 postRot = Vector3.zero;
    Vector3 postScale = new Vector3(0.6f, 0.5f, 0.5f);


    private void OnEnable()
    {
        thiefImg.SetActive(true);
        arrestImg.SetActive(false);
        PostRotCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.4f, 360f), new Keyframe(0.8f, 720f), new Keyframe(1.2f, 1080f + Random.Range(-10f, 10f)) });
        this.transform.position = postPos;
        this.transform.rotation = Quaternion.identity;
        this.transform.localScale = postScale;
        StartCoroutine(nameof(PostShow));
    }

    IEnumerator PostShow()
    {
        float startTime = 0;
        while (PostPosYCurve.keys[PostPosYCurve.keys.Length - 1].time >= startTime)
        {
            postPos.y = PostPosYCurve.Evaluate(startTime);
            this.transform.position = postPos;

            postRot.z = PostRotCurve.Evaluate(startTime);
            this.transform.rotation = Quaternion.Euler(postRot);

            this.transform.localScale = postScale * PostScaleCurve.Evaluate(startTime);
            
            startTime += Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.PostDone();
        arrestImg.SetActive(true);
    }

    public void SetImg()
    {
        thiefImg.SetActive(false);
        arrestImg.SetActive(false);
    }
}
