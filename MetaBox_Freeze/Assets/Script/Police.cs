using System.Collections;
using UnityEngine;

public class Police : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleArea = null;
    [SerializeField] Animator animatorPC = null;
    
    float playerSpeed = 0;
    float playerArea = 0;
    bool isMoving = false;
    Vector3 rightFlip = new Vector3(-1, 1, 1);
    Vector3 leftFlip = new Vector3(1, 1, 1);

    private void Awake()
    {
        if(circleArea == null) this.gameObject.TryGetComponent<CircleCollider2D>(out circleArea);
        GameManager.Instance.freezeDataSetting = Setting;
        GameManager.Instance.policeReset = () => animatorPC.SetTrigger("Init");
    }

    void Setting()
    {
        this.transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-3f, 1f), 0);
        playerSpeed = GameManager.Instance.FreezeData.playerSpeed * 0.02f;
        playerArea = GameManager.Instance.FreezeData.playerArea * 0.8f;
        circleArea.radius *= playerArea;
    }


    public void Move(Vector3 movepoint)
    {
        isMoving = false;  //If police change direction while moving, terminate the previous coroutine.
        if (movepoint.x >= this.transform.position.x) this.transform.localScale = rightFlip;
        else this.transform.localScale = leftFlip;
        movepoint.y -= circleArea.offset.y;
        StartCoroutine(_Move(movepoint));
    }

    IEnumerator _Move(Vector3 movepoint)
    {
        yield return null; //If police change direction while moving, terminate the previous coroutine.
        isMoving = true;
        SoundManager.Instance.PlaySFX(SFX.MovePC);
        animatorPC.SetBool("Move", true);
        while (isMoving && Time.timeScale != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movepoint, playerSpeed);
            //transform.position = Vector3.SmoothDamp(transform.position, movepoint, ref speed, 0.1f);

            if (transform.position.Equals(movepoint))
            {
                animatorPC.SetBool("Move", false);
                yield break;
            }
            yield return null;
        }
        
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.rigidbody == null)
        {
            isMoving = false; //If police collides with the play area, stops. Moves while pushing when colliding with an NPC.
            animatorPC.SetBool("Move", false);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 100, 100, 10);
        Gizmos.DrawSphere(this.transform.position + new Vector3(circleArea.offset.x, circleArea.offset.y, 0), circleArea.radius);
    }
#endif
}
