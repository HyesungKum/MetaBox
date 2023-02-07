using System.Collections;
using UnityEngine;

public delegate void CallbackArrest(Thief me);

public class Thief : MonoBehaviour
{
    public CallbackArrest callbackArrest = null;

    [SerializeField] ScriptableObj scriptableNPC = null;
    [SerializeField] CapsuleCollider2D npcCollider = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] ParticleSystem hideEff = null;

    Vector3 policeStation = new Vector3(7.6f, -1.8f, 0f);

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
    Vector3 rightFlip = new Vector3(1, 1, 1);
    Vector3 leftFlip = new Vector3(-1, 1, 1);

    void Awake()
    {
        police = FindObjectOfType<Police>();
        if (spriteRenderer == null) this.gameObject.TryGetComponent<SpriteRenderer>(out spriteRenderer);

        runnigTime = new WaitUntil(() => (police.transform.position - this.transform.position).magnitude > 1.5f);
        GameStart = new WaitUntil(() => GameManager.Instance.IsGaming);
        waitArrestTime = new WaitForSeconds(1.5f);
    }

    public void Setting(bool wantedThief, int id, int movespeed, int movetime)
    {
        this.id = id;
        this.speed = movespeed * 0.4f;
        this.wantedThief = wantedThief;
        waitMoveTime = new WaitForSeconds(movetime);
        if (wantedThief) spriteRenderer.sprite = scriptableNPC.ThiefNPC[0]; //µµµœ
        else spriteRenderer.sprite = scriptableNPC.CitizenNPC[0]; //Ω√πŒ

        GameManager.Instance.openThief += ImgShow;
        GameManager.Instance.hideThief += () => spriteRenderer.sprite = scriptableNPC.DefaultNPC[0]; //Ω«∑Áøß
        GameManager.Instance.removeThief += () => callbackArrest?.Invoke(this);
        GameManager.Instance.hideEff += () => hideEff.Play();

        npcCollider.enabled = true;
        runningAway = false;
        arrest = false;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        StartCoroutine(nameof(RandomDir));
        StartCoroutine(nameof(RandomMove));
    }

    void ImgShow()
    {
        if (wantedThief && arrest) spriteRenderer.sprite = scriptableNPC.ThiefNPC[1];
        else if (wantedThief == false) spriteRenderer.sprite = scriptableNPC.CitizenNPC[0];
        else spriteRenderer.sprite = scriptableNPC.ThiefNPC[0];
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
            if (arrest)
            {
                transform.position = Vector3.MoveTowards(transform.position, policeStation, speed);
                if(wantedThief) this.transform.localScale = rightFlip;
            }
            else
            {
                transform.Translate(dir * speed * Time.deltaTime);
                if (wantedThief && spriteRenderer.sprite == scriptableNPC.ThiefNPC[0])
                {
                    if (dir.x >= 0) this.transform.localScale = rightFlip;
                    else this.transform.localScale = leftFlip;
                }
            }
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
        yield return waitArrestTime;
        callbackArrest?.Invoke(this);
        callbackArrest = null;

        if (wantedThief) GameManager.Instance.Catch();
        else GameManager.Instance.Penalty();
    }

    private void OnCollisionEnter2D(Collision2D collision) //npc≥¢∏Æ √ÊµπΩ√
    {
        if (collision.rigidbody && runningAway == false)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-1f, 1f);
            dir.Normalize();
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //∫Æ √ÊµπΩ√ πÊ«‚∫Ø∞Ê
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
        if (collision.gameObject.name == "Prison")
        {
            if (runningAway)
            {
                arrest = true;
                npcCollider.enabled = false;
                StopCoroutine(nameof(RunAwayMode));
                speed *= 0.01f;
                GameManager.Instance.ShowImg();
                if (wantedThief) GameManager.Instance.CatchShow();
                StartCoroutine(nameof(Destroy));
            }
            else
            {
                dir.x = -1f;
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
        if(collision.gameObject.name == "Prison")
        {
            if (arrest == false && runningAway)
            {
                arrest = true;
                npcCollider.enabled = false;
                StopCoroutine(nameof(RunAwayMode));
                speed *= 0.01f;
                GameManager.Instance.ShowImg();
                if (wantedThief) GameManager.Instance.CatchShow();
                StartCoroutine(nameof(Destroy));
            }
            else if(arrest == false && runningAway == false)
            {
                dir.x = -1f;
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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawSphere(Vector3.zero, (float)(this.transform.localScale.x*0.5));
    }
#endif

}
