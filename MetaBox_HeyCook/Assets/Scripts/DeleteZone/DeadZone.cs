using MongoDB.Driver;
using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeadZone: MonoBehaviour
{
    [HideInInspector] public List<Ingredient> targetIngreds = new();

    private void Awake()
    {
        TryGetComponent(out BoxCollider2D collider);
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ingredient ingred))
        {
            targetIngreds.Add(ingred);

            StartCoroutine(nameof(DeleteRoutine));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out Ingredient ingred);
        if (targetIngreds.Contains(ingred))
        {
            targetIngreds.Remove(ingred);
        }
    }

    IEnumerator DeleteRoutine()
    {
        while (targetIngreds.Count != 0)
        {
            for (int i = 0; i < targetIngreds.Count; i++)
            {
                if (!targetIngreds[i].IsCliked)
                {
                    GameObject instVfx = targetIngreds[i].IngredData.delVfx;
                    PoolCp.Inst.BringObjectCp(instVfx).transform.position = targetIngreds[i].transform.position;
                    PoolCp.Inst.DestoryObjectCp(targetIngreds[i].gameObject);

                    SoundManager.Inst.CallSfx("Delete");
                }
            }
            yield return null;
        }
    }

    #region Editor
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(1f, 0f, 0f, 0.4f);
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, this.transform.GetComponent<BoxCollider2D>().size);
    }
    #endif
    #endregion
}
