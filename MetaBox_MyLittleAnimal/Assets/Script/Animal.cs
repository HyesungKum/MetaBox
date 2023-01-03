using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    bool arrest;
    Vector3 dir;
    float speed;
    public float RunAwayRange { get; set; }

    PlayerMove[] players = null;
    WaitForSeconds wait2 = null;
    WaitUntil GameStart = null;
    WaitUntil runAway1 = null;
    WaitUntil runAway2 = null;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerMove>();
        wait2 = new WaitForSeconds(2f);
        GameStart = new WaitUntil(() => GameManager.Instance.isGaming);
        runAway1 = new WaitUntil(() => Mathf.Abs((players[0].transform.position - this.transform.position).magnitude) <= RunAwayRange);
        runAway2 = new WaitUntil(() => Mathf.Abs((players[1].transform.position - this.transform.position).magnitude) <= RunAwayRange);

        arrest = false;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.4f, 1f), 0);
        speed = 2f;
        
        StartCoroutine(nameof(RandomDir));
        StartCoroutine(nameof(RandomMove));
        StartCoroutine(nameof(RunAwayMode1));
        StartCoroutine(nameof(RunAwayMode2));
    }
    
    IEnumerator RandomDir()
    {
        while (arrest == false)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-0.4f, 1f);
            yield return wait2;
        }
    }

    IEnumerator RandomMove()
    {
        yield return GameStart;
        while (GameManager.Instance.isGaming)
        {
            transform.Translate(dir * speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RunAwayMode1()
    {
        while (arrest == false)
        {
            yield return runAway1;
            dir = (this.transform.position - players[0].transform.position).normalized;
        }
    }

    IEnumerator RunAwayMode2()
    {
        while (arrest == false)
        {
            yield return runAway2;
            dir = (this.transform.position - players[1].transform.position).normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //동물, 벽 충돌시 방향변경
    {
        Debug.Log("나는 충돌했다");
        dir.x = -collision.contacts[0].point.x;
        dir.y = -collision.contacts[0].point.y;
        dir.Normalize();
        if(dir.y < -0.5f)
        {
            dir.y += 0.5f;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("나는 함정에 빠졌다");
        arrest = true;
        dir = (collision.transform.position - this.transform.position).normalized;
        UIManager.Instance.Catch();
        Destroy(gameObject, 0.5f);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 100, 100, 10);
        Gizmos.DrawSphere(this.transform.position, RunAwayRange-0.5f);
    }

    
}
