using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameBuilding : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] string _gameName;

    private void Reset()
    {
        TryGetComponent(out _collider);
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventReceiver.CallGameIn(_gameName);
    }

    #region Editor
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 1f, 0f, 0.4f);
        Gizmos.matrix = _collider.transform.localToWorldMatrix;
        Vector3 fixedPos = new Vector3(_collider.offset.x, _collider.offset.y);

        Gizmos.DrawCube(fixedPos, this.transform.GetComponent<BoxCollider2D>().size);
    }
    #endif
    #endregion
}
