using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayableNote : MonoBehaviour
{
    public delegate void DelegatePlayableNote(GameObject myPos);
    public static DelegatePlayableNote myDelegatePlayableNote;


    bool isMoving = false;
    float movingSpeed;

    Vector2 targetPos;
    Vector2 originPos;

    Inventory myInventory;


    private void Start()
    {
        myInventory = this.GetComponentInParent<Inventory>();
    }


    private void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Time.deltaTime * movingSpeed);


            if (Vector2.Distance(this.transform.position, targetPos) <= 0.05f)
            {

                isMoving = false;
            }
        }
    }


    public void StartToMove()
    {
        originPos = this.transform.position;
    }



    public void MoveNote(Vector3 target, float speed)
    {
        movingSpeed = speed;
        isMoving = true;
        targetPos = target;
    }


    // destroy note! 
    public void DestroyNote()
    {
        myInventory.DestoyedPlayableNote(this.gameObject);
    }
      
    // use note!
    public void UseNote()
    {
        myInventory.UseNote(this.gameObject);
    }


    public void Dropped()
    {
        RaycastHit2D hit = shootRay(this.transform.position);

        // layer 7 is MusicSheet
        if (hit && hit.transform.gameObject.layer == 7)
        {
            hit.transform.TryGetComponent<MusicSheet>(out MusicSheet landingPoint);
            landingPoint.CheckPlayableNotePos(this.gameObject.transform);
            return;
        }


        transform.position = originPos;
        this.TryGetComponent<Collider2D>(out Collider2D myCollider);
        myCollider.enabled = true;

    }


    RaycastHit2D shootRay(Vector2 targetT)
    {
        RaycastHit2D hit;
        Ray2D ray;

        ray = new Ray2D(targetT, Vector2.zero);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit;
    }

}
