using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    readonly Touch[] touches = new Touch[10];

    private void Update()
    {
        if (Input.touchCount >= 2)
        {
            for (int i = 0; i < Input.touchCount - 1; i++)
            {
                Debug.Log("hi");
                touches[i] = Input.GetTouch(i);
            }
        }  
    }
}
