using ObjectPoolCP;
using System.Collections;
using UnityEngine;

public class SimpleVfx : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] bool loop;

    private void OnEnable()
    {
        if (loop) duration = float.MaxValue; 
        StartCoroutine(nameof(LifeCycle));
    }

    IEnumerator LifeCycle()
    {
        float timer = 0f;

        while (this.gameObject.activeSelf && timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        PoolCp.Inst.DestoryObjectCp(this.gameObject);
    }
}
