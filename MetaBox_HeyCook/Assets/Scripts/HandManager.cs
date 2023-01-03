using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private void Update()
    {

        if (Input.touchCount <= 0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        IngredMove(ray, hit);
        TrimTableControll(hit);
    }

    public void IngredMove(Ray ray, RaycastHit2D hit)
    {
        if (!hit || !hit.transform.CompareTag(nameof(Ingredient))) return;

        Ingredient touchedIngred = hit.transform.GetComponent<Ingredient>();
        
        if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            touchedIngred.transform.position = Vector3.Lerp(hit.transform.position, ray.origin, Time.deltaTime * 2f);
            touchedIngred.IsCliked = true;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchedIngred.transform.position = ray.origin;
            touchedIngred.IsCliked = true;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchedIngred.IsCliked = false;
        }
    }
    public void TrimTableControll(RaycastHit2D hit)
    {
        if (!hit || !hit.transform.CompareTag("Table")) return;

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log(nameof(TrimType.Slicing));
            hit.transform.SendMessage(nameof(TrimType.Slicing), SendMessageOptions.DontRequireReceiver);
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log(nameof(TrimType.Touching));
            hit.transform.SendMessage(nameof(TrimType.Touching), SendMessageOptions.DontRequireReceiver);
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            Debug.Log(nameof(TrimType.Pressing));
            hit.transform.SendMessage(nameof(TrimType.Pressing), SendMessageOptions.DontRequireReceiver);
        }
    }
}
