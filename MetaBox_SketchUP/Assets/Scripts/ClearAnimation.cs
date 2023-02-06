using System.Collections;
using UnityEngine;

public class ClearAnimation : MonoBehaviour
{
    public AnimationCurve leftClearAnimation; // аб
    public AnimationCurve rightClearAnimationCurve; // ©Л

    private float curTime;
    public float moveTime = 0.5f;

    void Start()
    {
        //StartCoroutine(LeftRightAnimation());
        //StartCoroutine(Moving());
    }

    void Update()
    {

    }


    public IEnumerator Moving()
    {
        Quaternion start = transform.localRotation;
        //Debug.Log("start : " + start);
        Quaternion moveLeft = Quaternion.Euler(0, 0, -20);
        //Debug.Log("moves : " + moveLeft);

        float timer = 0.0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localRotation = Quaternion.Lerp(start, moveLeft, leftClearAnimation.Evaluate(check));
            yield return null;
        }

        timer = 0.0f;
        start = transform.localRotation;
        Quaternion moveRight = Quaternion.Euler(0, 0, 20);

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localRotation = Quaternion.Lerp(start, moveRight, rightClearAnimationCurve.Evaluate(check));
            yield return null;
        }

        timer = 0.0f;
        start = transform.localRotation;
        Quaternion moveZore = Quaternion.Euler(0, 0, 0);

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localRotation = Quaternion.Lerp(start, moveZore, leftClearAnimation.Evaluate(check));
            yield return null;
        }
    }

    public IEnumerator LeftRightAnimation()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 movePos = startPos + new Vector3(3, 0, 0);

        float timer = 0.0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localPosition = Vector3.Lerp(startPos, movePos, leftClearAnimation.Evaluate(check));
            yield return null;
        }

        timer = 0.0f;
        startPos = transform.localPosition;
        Vector3 rightPos = new Vector3(-3, 0, 0);

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localPosition = Vector3.Lerp(startPos, rightPos, rightClearAnimationCurve.Evaluate(check));
            //transform.localPosition = startPos;
            yield return null;
        }

        timer = 0.0f;
        startPos = transform.localPosition;
        Vector3 orignPos = Vector3.zero;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float check = timer / moveTime;
            transform.localPosition = Vector3.Lerp(startPos, orignPos, leftClearAnimation.Evaluate(check));
            yield return null;
        }
    }

    void OtherMove()
    {
        curTime += Time.deltaTime;
        if (curTime >= moveTime)
        {
            curTime -= curTime;
        }

        float xValue = leftClearAnimation.Evaluate(curTime);
        float yValue = rightClearAnimationCurve.Evaluate(curTime);

        transform.position = new Vector3(xValue, yValue, 0);
    }
}
