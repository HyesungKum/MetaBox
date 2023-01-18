using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BeltZone : MonoBehaviour
{
    public List<GameObject> BeltIngred = new(); 
    public float beltSpeed = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.position += Vector3.up * Time.deltaTime;
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 1f, 0f, 0.4f);
        Gizmos.DrawCube(this.transform.position, this.transform.GetComponent<BoxCollider2D>().size *  this.transform.lossyScale);
    }
#endif
    #endregion
}
