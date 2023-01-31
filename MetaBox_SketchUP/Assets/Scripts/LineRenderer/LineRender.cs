using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    private LineRenderer lineRender = null;
    [SerializeField] private BoxCollider2D collider = null;
    public const float resolutio = 0.1f;

    void Awake()
    {
        TryGetComponent<LineRenderer>(out lineRender);
        //lineRender.positionCount = 2;
    }

    public void SetPosition(int index, Vector3 pos)
    {
        if (!CanAppend(pos)) return;
        lineRender.positionCount = 2;
        lineRender.SetPosition(index, pos);
    }

    public Vector3 GetPosition(int index) => lineRender.GetPosition(index);

    public void SetCurvePosition(Vector3 pos)
    {
        if (!CanAppend(pos)) return;
        lineRender.positionCount += 1;
        lineRender.SetPosition(lineRender.positionCount - 1, pos);
    }

    private bool CanAppend(Vector3 pos)
    {
        if (lineRender.positionCount == 0) return true;
        return Vector2.Distance(lineRender.GetPosition(lineRender.positionCount - 1), pos) > resolutio;
    }
}
