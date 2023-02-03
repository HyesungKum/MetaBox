using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class Wall : MonoBehaviour
{
    [SerializeField] List<Transform> transforms = new List<Transform>();
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < transforms.Count; i++)
        {
            Gizmos.matrix = transforms[i].localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, transforms[i].GetComponent<BoxCollider2D>().size);
        }
    }
}
#endif