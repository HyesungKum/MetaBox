using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeadZone: MonoBehaviour
{
    private void Awake()
    {
        TryGetComponent(out BoxCollider2D collider);
        collider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            collision.TryGetComponent<Ingredient>(out Ingredient ingred);

            if (!ingred.IsCliked)
            {
                GameObject instVfx = ingred.IngredData.delVfx;
                PoolCp.Inst.BringObjectCp(instVfx).transform.position = collision.transform.position;
                PoolCp.Inst.DestoryObjectCp(ingred.gameObject);
            }
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
