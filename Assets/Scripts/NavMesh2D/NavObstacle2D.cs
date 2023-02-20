using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NavObstacle2D : MonoBehaviour
{
    public BoxCollider2D _collider;

    private void Reset()
    {
        TryGetComponent(out _collider);
    }

    #region gizmo
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, _collider.size);
    }
    #endif
    #endregion
}
