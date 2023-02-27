using System.Collections;
using UnityEngine;

public delegate void DelegatePlayableNote(PlayableNote note, bool destory);

public class PlayableNote : MonoBehaviour
{
    public DelegatePlayableNote myDelegatePlayableNote;

    [SerializeField] CapsuleCollider2D myCollider = null;

    Vector2 originPos;
    Vector2 targetPos;
    float movingSpeed;

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
        targetPos = target;
        movingSpeed = speed;
        
        StartCoroutine(nameof(Move));
    }

    IEnumerator Move()
    {
        while (true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Time.deltaTime * movingSpeed);

            if (Vector2.Distance(this.transform.position, targetPos) <= 0.01f) yield break;
            yield return null;
        }
    }


    // use or destory note
    public void UseNote(bool destory)
    {
        myDelegatePlayableNote(this, destory);
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
        Ray2D ray = new Ray2D(targetT, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit;
    }

}
