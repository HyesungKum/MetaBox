using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] GameObject TouchVfx = null;

    Touch[] touches = new Touch[10];

    int count = 0;

    private void Update()
    {
        if (GameManager.Inst.IsGameOver) return;

        if (Input.touchCount <= 0) return;
        count = Input.touchCount;

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        IngredMove(ray, hit);
        TableControll(hit);
    }

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
