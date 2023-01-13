using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeltZone : MonoBehaviour
{
    public List<Ingredient> BeltIngred = new(); 
    public int Counting = 0;
    public float beltSpeed = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            BeltIngred.Add(collision.GetComponent<Ingredient>());
            BeltIngred.ForEach(ingrd => { ingrd.Rigidbody2D.velocity = Vector2.up; });
            Counting++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient ingred = collision.GetComponent<Ingredient>();
            ingred.Rigidbody2D.velocity = Vector2.zero;
            BeltIngred.Remove(ingred);
            Counting--;
        }
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 1f, 0f, 0.4f);
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif
    #endregion
}
