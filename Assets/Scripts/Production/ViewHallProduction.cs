using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHallProduction : Production
{
    [SerializeField] private GameObject viewHallObj;

    public sealed override void DoProduction() => StartCoroutine(nameof(ViewHallShrink)); 
    public sealed override void UndoProduction() => StartCoroutine(nameof(ViewHallExtension));

    IEnumerator ViewHallExtension()
    {
        timer = 0f;
        while (viewHallObj.transform.localScale.x <= 45)
        {
            timer += Time.deltaTime / 15f;

            viewHallObj.transform.localScale = Vector3.Lerp(viewHallObj.transform.localScale, Vector3.one * 46f, timer);
            yield return null;
        }

        viewHallObj.transform.localScale = Vector3.one * 45f;
    }
    IEnumerator ViewHallShrink()
    {
        timer = 0f;
        while (viewHallObj.transform.localScale.x >= 0.8f)
        {
            timer += Time.deltaTime / 15f;

            viewHallObj.transform.localScale = Vector3.Lerp(viewHallObj.transform.localScale, Vector3.zero, timer);
            yield return null;
        }

        viewHallObj.transform.localScale = Vector3.zero;
    }
}
