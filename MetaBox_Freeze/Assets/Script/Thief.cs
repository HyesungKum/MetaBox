using System.Collections;
using UnityEngine;

public delegate void CallbackArrest(Thief me);

public class Thief : MonoBehaviour
{
    public CallbackArrest callbackArrest = null;

    [SerializeField] ScriptableObj thiefImages = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    Police police = null;
    WaitUntil runnigTime = null;
    WaitUntil GameStart = null;
    WaitForSeconds waitMoveTime = null;
    WaitForSeconds waitArrestTime = null;
    
    int id;
    float speed;
    bool wantedThief;
    bool runningAway;
    bool arrest;
    Vector3 dir;

    void Awake()
    {
        police = FindObjectOfType<Police>();
        if (spriteRenderer == null) this.gameObject.TryGetComponent<SpriteRenderer>(out spriteRenderer);

        runnigTime = new WaitUntil(() => (police.transform.position - this.transform.position).magnitude > 2f);
        GameStart = new WaitUntil(() => GameManager.Instance.IsGaming);
        waitArrestTime = new WaitForSeconds(1f);
    }

    public void Setting(bool wantedThief, int id, int movespeed, int movetime)
    {
        this.id = id;
        this.speed = movespeed * 0.4f;
        this.wantedThief = wantedThief;
        waitMoveTime = new WaitForSeconds(movetime);
        if (wantedThief) spriteRenderer.sprite = thiefImages.Thief[2];
        else spriteRenderer.sprite = thiefImages.Thief[0];

        GameManager.Instance.openThief += ImgShow;
        GameManager.Instance.hideThief += () => spriteRenderer.sprite = thiefImages.Thief[1];
        GameManager.Instance.removeThief += () => callbackArrest?.Invoke(this);

        runningAway = false;
        arrest = false;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        StartCoroutine(nameof(RandomDir));
        StartCoroutine(nameof(RandomMove));
    }

    void ImgShow()
    {
        if (wantedThief) spriteRenderer.sprite = thiefImages.Thief[2];
        else spriteRenderer.sprite = thiefImages.Thief[0];
    }
    IEnumerator RandomDir()
    {
        yield return GameStart;
        while (arrest == false)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-1f, 1f);
            dir.Normalize();
            yield return waitMoveTime;
        }
    }

    IEnumerator RandomMove()
    {
        yield return GameStart;
        while (GameManager.Instance.IsGaming)
        {
            transform.Translate(dir * speed * Time.deltaTime);
            yield return null;
        }
    }
    
    IEnumerator RunAwayMode()
    {
        yield return runnigTime;
        runningAway = false;
    }

    IEnumerator Destroy()
    {
        //UIManager.Instance.Catch(id);
        yield return waitArrestTime;
        callbackArrest?.Invoke(this);
        callbackArrest = null;

        if (wantedThief) GameManager.Instance.Catch();
        else GameManager.Instance.Penalty();
    }

    private void OnCollisionEnter2D(Collision2D collision) //npc끼리 충돌시
    {
        if (collision.rigidbody)
        {
            if (runningAway) return;
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-1f, 1f);
            dir.Normalize();
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //벽 충돌시 방향변경
    {
        if(collision.rigidbody == false && arrest == false)
        {
            dir.x = -collision.contacts[0].point.x;
            dir.y = -collision.contacts[0].point.y;
            dir.Normalize();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Prison")
        {
            if (runningAway)
            {
                arrest = true;
                StopCoroutine(nameof(RunAwayMode));
                dir = (collision.transform.position - this.transform.position).normalized;
                if (dir.x > 0) transform.localScale = new Vector3(-1, 1);
                speed *= 0.8f;
                GameManager.Instance.ShowImg();
                StartCoroutine(nameof(Destroy));
            }
            else
            {
                dir.y = 1f;
                dir.Normalize();
            }
        }
        else
        {
            runningAway = true;
            StartCoroutine(nameof(RunAwayMode));
            dir = (this.transform.position - collision.transform.position).normalized;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Prison")
        {
            if (arrest == false && runningAway)
            {
                arrest = true;
                StopCoroutine(nameof(RunAwayMode));
                dir = (collision.transform.position - this.transform.position).normalized;
                if (dir.x > 0) transform.localScale = new Vector3(-1, 1);
                speed *= 0.8f;
                GameManager.Instance.ShowImg();
                StartCoroutine(nameof(Destroy));
            }
            else if(arrest == false && runningAway == false)
            {
                dir.y = 1f;
                dir.Normalize();
            }
        }
        else
        {
            if(runningAway == false)
            {
                runningAway = true;
                StartCoroutine(nameof(RunAwayMode));
            }
            dir = (this.transform.position - collision.transform.position).normalized;
        }
    }
}
