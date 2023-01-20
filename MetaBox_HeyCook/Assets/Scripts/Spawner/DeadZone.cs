using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeadZone: MonoBehaviour
{
    #region Editor Gizmo
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(1f, 0f, 0f, 0.4f);
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(nameof(Ingredient)))
        {
            collision.collider.TryGetComponent<Ingredient>(out Ingredient ingred);
            GameObject instVfx = ingred.IngredData.delVfx;
            PoolCp.Inst.BringObjectCp(instVfx).transform.position = collision.contacts[0].point;
            PoolCp.Inst.DestoryObjectCp(collision.gameObject);
        }
    }
}
