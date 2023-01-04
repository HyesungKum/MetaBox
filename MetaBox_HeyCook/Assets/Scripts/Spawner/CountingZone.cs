using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CountingZone : MonoBehaviour
{
    public int Counting = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Counting++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Counting--;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif
}
