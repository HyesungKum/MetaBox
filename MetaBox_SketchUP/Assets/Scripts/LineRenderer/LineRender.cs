using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    private LineRenderer lineRender = null;
    private EdgeCollider2D edgeCollider = null;
    Transform edge = null;
    public const float resolutio = 0.1f;

    public Stack<Vector3> posStack;

    Color newColor;

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

    public void SetPositionCountDownforTwo()
    {
        lineRender.positionCount -= 1;
        if (lineRender.positionCount == 1)
            return;
    }

    public void SetCurvePosition(Vector3 pos)
    {
        if (!CanAppend(pos)) return;
        lineRender.positionCount += 1;
        lineRender.SetPosition(lineRender.positionCount - 1, pos);
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

    public Color GetLineColor()
    {
        newColor = lineRender.startColor;
        //Debug.Log("newColor :" + newColor);
        return newColor;
    }

    public void SetColor(Color newColors)
    {
        //newColor = newColors;
        lineRender.startColor = newColors;
        //lineRender.endColor = newColor;
        //lineRender.SetColors(newColor, newColor);
    }


    public void setEdgeCollider()
    {
        edge = this.transform.GetChild(0);

        if (edge != null)
        {
            edge.TryGetComponent<EdgeCollider2D>(out edgeCollider);
        }

        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRender.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRender.GetPosition(point);
            edges.Add(Vector2.right * lineRendererPoint.x + Vector2.up * lineRendererPoint.y);
        }

        edgeCollider.SetPoints(edges);
    }
}