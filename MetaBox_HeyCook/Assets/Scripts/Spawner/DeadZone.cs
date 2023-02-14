using MongoDB.Driver;
using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeadZone: MonoBehaviour
{
    private Ingredient targetIngred;

    private void Awake()
    {
        TryGetComponent(out BoxCollider2D collider);
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ingredient ingred))
        {
            targetIngred = ingred;

            StartCoroutine(nameof(DeleteRoutine));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetIngred.gameObject)
        {
            targetIngred = null;
        }
    }

    IEnumerator DeleteRoutine()
    {
        while (targetIngred != null)
        {
            if (!targetIngred.IsCliked)
            {
                GameObject instVfx = targetIngred.IngredData.delVfx;
                PoolCp.Inst.BringObjectCp(instVfx).transform.position = targetIngred.transform.position;
                PoolCp.Inst.DestoryObjectCp(targetIngred.gameObject);
                targetIngred = null;
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
