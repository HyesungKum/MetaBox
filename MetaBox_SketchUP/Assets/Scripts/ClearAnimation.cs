using System.Collections;
using UnityEngine;

public class ClearAnimation : MonoBehaviour
{
    public AnimationCurve leftClearAnimation; // ��
    public AnimationCurve rightClearAnimationCurve; // ��

    public float moveTime = 0.5f;

    void Start()
    {
        //StartCoroutine(LeftRightAnimation());
        //StartCoroutine(Moving());
    } 

    //[ContextMenu("TEST")]
    //public void Func()
    //{
    //    StartCoroutine(Moving());
    //}

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
}