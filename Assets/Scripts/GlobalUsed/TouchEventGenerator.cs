using Kum;
using UnityEngine;

public delegate void TouchEvent(int id, Vector3 pos);

public class TouchEventGenerator : MonoSingleTon<TouchEventGenerator>
{
    //=================================10 Touch Struct==============================
    [Header("Touch OverView")]
    [SerializeField] private int touchCount = 0;
    [SerializeField] private Touch[] touches = new Touch[10];

    public TouchEvent[] touchBegan      = new TouchEvent[10];
    public TouchEvent[] touchStationary = new TouchEvent[10];
    public TouchEvent[] touchMoved      = new TouchEvent[10];
    public TouchEvent[] touchEnded      = new TouchEvent[10];
    public TouchEvent[] touchCancled    = new TouchEvent[10];

    private void Update()
    {
        touchCount = Input.touchCount;

        if (touchCount == 0) return;

        for (int i = 0; i < touchCount; i++)
        {
            touches[i] = Input.GetTouch(i);

            Vector3 pos = touches[i].position;
            TouchPhase phase = touches[i].phase;

            switch (phase)
            {
                case TouchPhase.Began: touchBegan[i]?.Invoke(i, pos); break;
                case TouchPhase.Stationary: touchStationary[i]?.Invoke(i, pos);break;
                case TouchPhase.Moved: touchMoved[i]?.Invoke(i, pos); break;
                case TouchPhase.Ended: touchEnded[i]?.Invoke(i, pos); break;
                case TouchPhase.Canceled: touchCancled[i]?.Invoke(i, pos); break;
            }
        }
    }
}
