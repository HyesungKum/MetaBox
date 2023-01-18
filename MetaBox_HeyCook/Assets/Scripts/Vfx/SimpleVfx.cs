using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleVfx : MonoBehaviour
{
    [SerializeField] float duration;

    private void OnEnable()
    {
        StartCoroutine(nameof(LifeCycle));
    }

    IEnumerator LifeCycle()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        PoolCp.Inst.DestoryObjectCp(this.gameObject);
    }
}
