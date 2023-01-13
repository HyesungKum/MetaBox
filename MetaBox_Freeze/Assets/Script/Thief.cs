using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallbackArrest(Thief me);

public class Thief : MonoBehaviour
{
    public CallbackArrest callbackArrest = null;

    [SerializeField] ScriptableObj thiefImages = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    ThiefSpawner thiefSpawner = null;
    Police police = null;
    WaitUntil runnigTime = null;
    WaitUntil GameStart = null;
    WaitForSeconds waitMoveTime = null;
    
    int id;
    float speed;
    bool wantedThief;
    bool runningAway;
    bool arrest;
    Vector3 dir;

    void OnEnable()
    {
        Debug.Log("활성화");
        runnigTime = new WaitUntil(() => (police.transform.position - this.transform.position).magnitude > 2f);
        GameStart = new WaitUntil(() => GameManager.Instance.IsGaming);
        runningAway = false;
        arrest = false;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.4f, 1f), 0);
        StartCoroutine(nameof(RandomDir));
        StartCoroutine(nameof(RandomMove));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("스타트");
        thiefSpawner = this.transform.parent.GetComponent<ThiefSpawner>();
        police = FindObjectOfType<Police>();
        if (spriteRenderer == null) spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void OpenImage()
    {
        if (wantedThief) spriteRenderer.sprite = thiefImages.wantedThief;
        else spriteRenderer.sprite = thiefImages.nomalThief;
    }

    public void HideImage()
    {
        spriteRenderer.sprite = thiefImages.hideThief;
    }

    public void Remove()
    {
        if(this.gameObject.activeSelf == false) return;
        thiefSpawner.Release(this);
    }

    public void Setting(bool wantedThief, int id, int movespeed, int movetime)
    {
        this.id = id;
        this.speed = movespeed * 0.4f;
        this.wantedThief = wantedThief;
        waitMoveTime = new WaitForSeconds(movetime);
    }

    IEnumerator RandomDir()
    {
        Debug.Log("방향코루틴스타트");
        yield return GameStart;
        Debug.Log("방향코루틴교");
        while (arrest == false)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-0.5f, 1f);
            yield return waitMoveTime;
        }
    }

    IEnumerator RandomMove()
    {
        Debug.Log("무브코루틴스타트");
        yield return GameStart;
        Debug.Log("무브코루틴교");
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
        yield return waitMoveTime;
        thiefSpawner.Hide();
        if (wantedThief) GameManager.Instance.Catch();
        else GameManager.Instance.Penalty();
        
        if(callbackArrest != null) callbackArrest(this);
    }

    private void OnCollisionEnter2D(Collision2D collision) //동물, 벽 충돌시 방향변경
    {
        if (runningAway) return;
        dir.x = -collision.contacts[0].point.x;
        dir.y = -collision.contacts[0].point.y;
        dir.Normalize();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Prison")
        {
            Debug.Log("나는 함정에 빠졌다");
            if (runningAway)
            {
                Debug.Log("나는 결국...");
                arrest = true;
                dir = (collision.transform.position - this.transform.position).normalized;
                speed *= 0.7f;
                thiefSpawner.Open();
                StartCoroutine(nameof(Destroy));
            }
            else
            {
                Debug.Log("나는 잘 튀었다");
                dir.y = 1f;
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
        if(collision.tag != "Prison")
        {
            dir = (this.transform.position - collision.transform.position).normalized;
        }
    }
}
