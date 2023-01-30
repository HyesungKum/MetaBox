using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BeltZone : MonoBehaviour
{
    public List<GameObject> BeltIngred = new(); 
    public float beltSpeed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.position += beltSpeed * Time.deltaTime * this.transform.up;
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 1f, 0f, 0.4f);
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, this.transform.GetComponent<BoxCollider2D>().size);
    }
#endif
    #endregion
}
