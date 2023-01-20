using UnityEngine;
using Kum;
using System.Collections;

public delegate void TouchDelegate(int id, Vector3 pos);

public class TouchEventGenerator : MonoSingleTon<TouchEventGenerator>
{
    //=================================10 Touch Struct==============================
    [Header("Touch OverView")]
    [SerializeField] private int touchCount = 0;
    [SerializeField] private Touch[] touches = new Touch[10];

    public TouchDelegate[] touchBegan      = new TouchDelegate[10];
    public TouchDelegate[] touchStationary = new TouchDelegate[10];
    public TouchDelegate[] touchMoved      = new TouchDelegate[10];
    public TouchDelegate[] touchEnded      = new TouchDelegate[10];
    public TouchDelegate[] touchCancled    = new TouchDelegate[10];

    private void Update()
    {
        touchCount = Input.touchCount;

        if (touchCount == 0) return;

        for (int i = 0; i < touchCount; i++)
        {
            touches[i] = Input.GetTouch(i);

            Vector3 pos = touches[i].position;
            TouchPhase phase = touches[i].phase;

            if (phase == TouchPhase.Began) touchBegan[i]?.Invoke(i, pos);
            else if (phase == TouchPhase.Stationary) touchStationary[i]?.Invoke(i, pos);
            else if (phase == TouchPhase.Moved) touchMoved[i]?.Invoke(i, pos);
            else if (phase == TouchPhase.Ended) touchEnded[i]?.Invoke(i, pos);
            else if (phase == TouchPhase.Canceled) touchCancled[i]?.Invoke(i, pos);
        }
    }
}
