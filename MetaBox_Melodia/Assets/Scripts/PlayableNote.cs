using System.Collections;
using UnityEngine;


public class PlayableNote : MonoBehaviour
{
    public delegate void DelegatePlayableNote(PlayableNote note, bool destory);
    public DelegatePlayableNote myDelegatePlayableNote;

    [SerializeField] CapsuleCollider2D myCollider = null;

    float movingSpeed;

    Vector2 targetPos;
    Vector2 originPos;

    private void OnEnable()
    {
        myCollider.enabled = true;
    }

    private void OnDisable()
    {
        myDelegatePlayableNote = null;
    }
    public void StartToMove()
    {
        originPos = this.transform.position;
    }


    public void MoveNote(Vector3 target, float speed)
    {
        movingSpeed = speed;
        targetPos = target;
        StartCoroutine(nameof(Move));
    }

    IEnumerator Move()
    {
        while (true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Time.deltaTime * movingSpeed);

            if (Vector2.Distance(this.transform.position, targetPos) <= 0.05f) yield break;
            yield return null;
        }
        
    }

    // destroy note! 
    public void DestroyNote()
    {
        myDelegatePlayableNote(this, true);
    }
      
    // use note!
    public void UseNote()
    {
        myDelegatePlayableNote(this, false);
    }


    public void Dropped()
    {
        RaycastHit2D hit = shootRay(this.transform.position);

        // layer 7 is MusicSheet
        if (hit && hit.transform.gameObject.layer == 7)
        {
            hit.transform.TryGetComponent<MusicSheet>(out MusicSheet landingPoint);
            landingPoint.CheckPlayableNotePos(this);
            return;
        }


        transform.position = originPos;
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
