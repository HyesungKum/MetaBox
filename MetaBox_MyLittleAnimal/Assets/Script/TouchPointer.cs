using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPointer : MonoBehaviour
{
    [SerializeField] PlayerMove player1 = null;
    [SerializeField] PlayerMove player2 = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGaming == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 movePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePoint.z = 0;
            if ((movePoint - player1.transform.position).magnitude > (movePoint - player2.transform.position).magnitude)
            {
                player2.Move(movePoint);
            }
            else
            {
                player1.Move(movePoint);
            }
        }
    }
}
