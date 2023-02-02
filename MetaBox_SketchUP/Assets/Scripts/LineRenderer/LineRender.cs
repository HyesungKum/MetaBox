using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LineRender : MonoBehaviour
{
    private LineRenderer lineRender = null;
    public const float resolutio = 0.1f;

    public Stack<Vector3> posStack;

    void Awake()
    {
        posStack = new Stack<Vector3>();
        TryGetComponent<LineRenderer>(out lineRender);
    }

    public void SetPosition(int index, Vector3 pos)
    {
        if (!CanAppend(pos)) return;
        lineRender.positionCount = 2;
        lineRender.SetPosition(index, pos);
    }

    public Vector3 GetPosition(int index) => lineRender.GetPosition(index);

    public int GetPositionCount() => lineRender.positionCount;

    public int SetPositionCountDown() => lineRender.positionCount -= 1;

    public void SetCurvePosition(Vector3 pos)
    {
        if (!CanAppend(pos)) return;
        lineRender.positionCount += 1;
        lineRender.SetPosition(lineRender.positionCount -1, pos);
    }

    public void GetPositions()
    {
        Vector3[] vector3s = new Vector3[lineRender.positionCount];
        lineRender.GetPositions(vector3s);
    }

    private bool CanAppend(Vector3 pos)
    {
        if (lineRender.positionCount == 0) return true;
        return Vector2.Distance(lineRender.GetPosition(lineRender.positionCount - 1), pos) > resolutio;
    }

}
