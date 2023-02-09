using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallbackArrest(Thief me);

public enum Mode
{
    Silhouette = 0,
    Citizen = 1,
    Thief = 2
}
public class Thief : MonoBehaviour
{
    public CallbackArrest callbackArrest = null;

    [SerializeField] CapsuleCollider2D npcCollider = null;
    [SerializeField] List<GameObject> npcMode = null;
    [SerializeField] List<Animator> npsAnimator = null;
    [SerializeField] ParticleSystem hideEff = null;

    Vector3 policeStation = new Vector3(7.6f, -1.2f, 0f);

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
    Mode myMode;
    Vector3 dir;
    Vector3 rightFlip = new Vector3(-1, 1, 1);
    Vector3 leftFlip = new Vector3(1, 1, 1);

    void Awake()
    {
        police = FindObjectOfType<Police>();

        runnigTime = new WaitUntil(() => (police.transform.position - this.transform.position).magnitude > 1.5f);
        GameStart = new WaitUntil(() => GameManager.Instance.IsGaming);
        waitArrestTime = new WaitForSeconds(1.5f);
    }

    private void OnEnable()
    {
        for (int i = 0; i < npcMode.Count; i++)
        {
            npsAnimator[(int)myMode].SetTrigger("Exit");
            npcMode[i].SetActive(false);
        }
    }

    public void Setting(bool wantedThief, int id, int movespeed, int movetime)
    {
        this.id = id;
        this.speed = movespeed * 0.4f;
        this.wantedThief = wantedThief;
        waitMoveTime = new WaitForSeconds(movetime);

        if (wantedThief) myMode = Mode.Thief;
        else myMode = Mode.Citizen;
        npcMode[(int)myMode].SetActive(true);
        

        GameManager.Instance.openThief += OpenImg;
        GameManager.Instance.hideThief += Hide;
        GameManager.Instance.removeThief += () => callbackArrest?.Invoke(this);
        GameManager.Instance.hideEff += () => hideEff.Play();

        npcCollider.enabled = true;
        runningAway = false;
        arrest = false;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        StartCoroutine(nameof(RandomDir));
        StartCoroutine(nameof(RandomMove));
    }

    void OpenImg()
    {
        npcMode[(int)Mode.Silhouette].SetActive(false);

        if (wantedThief) myMode = Mode.Thief;
        else myMode = Mode.Citizen;

        npcMode[(int)myMode].SetActive(true);
        if (arrest) npsAnimator[(int)myMode].SetInteger("Arrest", (int)myMode); //thief == 2 or citizen == 1
        else npsAnimator[(int)myMode].SetBool("Move", true);
    }

    void Hide()
    {
        npcMode[(int)myMode].SetActive(false); //thief or citizen

        myMode = Mode.Silhouette;

        npcMode[(int)myMode].SetActive(true); //silhouette
        npsAnimator[(int)myMode].SetBool("Move", true);
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
            }
            else
            {
                transform.Translate(dir * speed * Time.deltaTime);
                if (dir.x >= 0) this.transform.localScale = rightFlip;
                else this.transform.localScale = leftFlip;
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

    private void OnCollisionEnter2D(Collision2D collision) //npc끼리 충돌시
    {
        if (collision.rigidbody && runningAway == false)
        {
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
        if (collision.gameObject.name == "Prison")
        {
            if (runningAway)
            {
                arrest = true;
                npcCollider.enabled = false;
                StopCoroutine(nameof(RunAwayMode));
                speed *= 0.01f;
                this.transform.localScale = rightFlip;
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
                this.transform.localScale = rightFlip;
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
        Gizmos.DrawSphere(Vector3.zero + new Vector3(npcCollider.offset.x, npcCollider.offset.y, 0), (float)(this.transform.localScale.x*0.5));
    }
#endif

}
