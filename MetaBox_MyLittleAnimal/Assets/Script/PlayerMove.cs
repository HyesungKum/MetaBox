using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed;
    bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1/speed;
    }

    public void Move(Vector3 movepoint)
    {
        isMoving = false;
        StartCoroutine(_Move(movepoint));
    }
    IEnumerator _Move(Vector3 movepoint)
    {
        yield return new WaitForSeconds(0.05f);
        isMoving = true;
        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, movepoint, speed);
            //transform.position = Vector3.SmoothDamp(transform.position, movepoint, ref speed, 0.1f);

            if (transform.position.Equals(movepoint)) yield break;
            yield return null;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        isMoving = false;
    }

}
