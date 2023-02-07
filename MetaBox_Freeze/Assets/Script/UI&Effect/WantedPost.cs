using System.Collections;
using UnityEngine;

public class WantedPost : MonoBehaviour
{
    [SerializeField] SpriteRenderer postImg = null;
    [SerializeField] SpriteRenderer thiefImg = null;
    [SerializeField] SpriteRenderer arrestImg = null;
    [SerializeField] AnimationCurve PostPosYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -1f), new Keyframe(1.2f, 3f) });
    [SerializeField] AnimationCurve PostRotCurve;
    [SerializeField] AnimationCurve PostScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.2f, 1f) });

    Vector3 postPos = new Vector3(7.2f, -1f, 0);
    Vector3 postRot = Vector3.zero;
    Vector3 postScale = new Vector3(0.6f, 0.5f, 0.5f);


    private void OnEnable()
    {
        PostRotCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.4f, 360f), new Keyframe(0.8f, 720f), new Keyframe(1.2f, 1080f + Random.Range(-10f, 10f)) });
        postImg.sortingOrder = 3;
        thiefImg.sortingOrder = 4;
        arrestImg.sortingOrder = 5;
        arrestImg.gameObject.SetActive(false);
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
        arrestImg.gameObject.SetActive(true);
    }

    public void SetImg()
    {
        postImg.sortingOrder = 0;
        thiefImg.sortingOrder = 1;
        arrestImg.sortingOrder = 2;
    }

    public void HideImg()
    {
        postImg.sortingOrder = 0;
        thiefImg.sortingOrder = 0;
        arrestImg.sortingOrder = 0;
    }
}
