using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadProduction : Production
{
    [SerializeField] private GameObject[] spreadObjs;

    public sealed override void DoProduction() => StartCoroutine(nameof(Spread));
    public sealed override void UndoProduction() => StartCoroutine(nameof(Store));

    //objects spread to center
    IEnumerator Spread()
    {
        timer = 0f;

        while (this.gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < spreadObjs.Length; i++)
            {
                Vector3 moveDir = spreadObjs[i].transform.position - center;
                spreadObjs[i].transform.position += prodSpeed * Time.deltaTime * moveDir.normalized;
            }

            if (timer > prodTime)
            {
                CallProdEnd();
                yield break;
            }

            yield return null;
        }

        CallProdEnd();
    }
    //objects store to center
    IEnumerator Store()
    {
        timer = 0f;

        while (this.gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < spreadObjs.Length; i++)
            {
                Vector3 moveDir = center - spreadObjs[i].transform.position;
                spreadObjs[i].transform.position += moveDir.normalized * prodSpeed * Time.deltaTime;
            }

            if (timer > prodTime)
            {
                CallProdEnd();
                yield break;
            }

            yield return null;
        }

        CallProdEnd();
    }

}
