using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeadZone: MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(nameof(Ingredient)))
        {
            //Instantiate(vfx, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(collision.gameObject);
        }
    }
}
