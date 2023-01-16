using ObjectPoolCP;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

delegate void TouchDelegate(Vector3 pos);

public class HandManager : MonoBehaviour
{
    //==================================Reference Object============================
    [Header("Reference Main Camera")]
    [SerializeField] Camera mainCam;

    [Header("Touch Vfx Prefabs")]
    [SerializeField] GameObject TouchVfx = null;

    //=================================10 Touch Struct==============================
    [Header("Touch OverView")]
    [SerializeField] private int touchCount = 0;
    [SerializeField] private Touch[] touches = new Touch[10];

    TouchDelegate[] touchBegan = new TouchDelegate[10];
    TouchDelegate[] touchMoved = new TouchDelegate[10];
    TouchDelegate[] touchEnded = new TouchDelegate[10];

    private void Awake()
    {
        //touch chain

        for (int i = 0; i < touchBegan.Length; i++)
        {
            touchBegan[i] += OnTouched;
        }
    }

    private void Update()
    {
        touchCount = Input.touchCount;

        if (touchCount == 0) return;

        for (int i = 0; i < touchCount; i++)
        {
            touches[i] = Input.GetTouch(i);

            int id = touches[i].fingerId;
            Vector3 pos = touches[i].position;
            TouchPhase phase = touches[i].phase;

            if (phase == TouchPhase.Began)
            {
                switch (id)
                {
                    case 0: touchBegan[id]?.Invoke(pos); break;
                    case 1: touchBegan[id]?.Invoke(pos); break;
                    case 2: touchBegan[id]?.Invoke(pos); break;
                    case 3: touchBegan[id]?.Invoke(pos); break;
                    case 4: touchBegan[id]?.Invoke(pos); break;
                    case 5: touchBegan[id]?.Invoke(pos); break;
                    case 6: touchBegan[id]?.Invoke(pos); break;
                    case 7: touchBegan[id]?.Invoke(pos); break;
                    case 8: touchBegan[id]?.Invoke(pos); break;
                    case 9: touchBegan[id]?.Invoke(pos); break;
                        default: throw new Exception();
                }
            }
            if (touches[i].phase == TouchPhase.Moved)
            {
                switch (id)
                {
                    case 0: touchMoved[id]?.Invoke(pos); break;
                    case 1: touchMoved[id]?.Invoke(pos); break;
                    case 2: touchMoved[id]?.Invoke(pos); break;
                    case 3: touchMoved[id]?.Invoke(pos); break;
                    case 4: touchMoved[id]?.Invoke(pos); break;
                    case 5: touchMoved[id]?.Invoke(pos); break;
                    case 6: touchMoved[id]?.Invoke(pos); break;
                    case 7: touchMoved[id]?.Invoke(pos); break;
                    case 8: touchMoved[id]?.Invoke(pos); break;
                    case 9: touchMoved[id]?.Invoke(pos); break;
                    default: throw new Exception();
                }
            }
            if (touches[i].phase == TouchPhase.Ended)
            {
                switch (id)
                {
                    case 0: touchEnded[id]?.Invoke(pos); break;
                    case 1: touchEnded[id]?.Invoke(pos); break;
                    case 2: touchEnded[id]?.Invoke(pos); break;
                    case 3: touchEnded[id]?.Invoke(pos); break;
                    case 4: touchEnded[id]?.Invoke(pos); break;
                    case 5: touchEnded[id]?.Invoke(pos); break;
                    case 6: touchEnded[id]?.Invoke(pos); break;
                    case 7: touchEnded[id]?.Invoke(pos); break;
                    case 8: touchEnded[id]?.Invoke(pos); break;
                    case 9: touchEnded[id]?.Invoke(pos); break;
                    default: throw new Exception();
                }
            }
        }
    }

    void OnTouched(Vector3 pos)
    {
        Vector3 fixedPos = mainCam.ScreenToWorldPoint (pos) + Vector3.forward;

        GameObject instVfx = PoolCp.Inst.BringObjectCp(TouchVfx);
        instVfx.transform.position = fixedPos;
    }

    //if (GameManager.Inst.IsGameOver || GameManager.Inst.IsPause) return;
    //
    //Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
    //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);
    //
    //IngredMove(ray, hit);
    //TableControll(hit);

    public void IngredMove(Ray ray, RaycastHit2D hit)
    {
        if (!hit || !hit.transform.CompareTag(nameof(Ingredient))) return;

        Ingredient touchedIngred = hit.transform.GetComponent<Ingredient>();
        
        if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {

            touchedIngred.IsCliked = true;

        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            touchedIngred.transform.position = ray.origin;

        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            touchedIngred.IsCliked = false;

        }
    }
    public void TableControll(RaycastHit2D hit)
    {
        if (!hit || !hit.transform.CompareTag("Table")) return;

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            hit.transform.SendMessage(nameof(TrimType.Slicing), SendMessageOptions.DontRequireReceiver);

        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            hit.transform.SendMessage(nameof(TrimType.Touching), SendMessageOptions.DontRequireReceiver);
            GameObject instVfx = PoolCp.Inst.BringObjectCp(TouchVfx);
            instVfx.transform.position = hit.point;
            instVfx.transform.position += Vector3.back;

        }
        else if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {

            hit.transform.SendMessage(nameof(TrimType.Pressing), SendMessageOptions.DontRequireReceiver);

        }
    }
}
