using System.Collections;
using System.Collections.Generic;
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
    
    int id;
    float speed;
    bool wantedThief;
    bool runningAway;
    bool arrest;
    Vector3 dir;

    void OnEnable()
    {
        Debug.Log("Ȱ��ȭ");
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
        Debug.Log("��ŸƮ");
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
        if (callbackArrest != null) callbackArrest(this);
        callbackArrest = null;
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
        Debug.Log("�����ڷ�ƾ��ŸƮ");
        yield return GameStart;
        Debug.Log("�����ڷ�ƾ��");
        while (arrest == false)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = Random.Range(-0.5f, 1f);
            yield return waitMoveTime;
        }
    }

    IEnumerator RandomMove()
    {
        Debug.Log("�����ڷ�ƾ��ŸƮ");
        yield return GameStart;
        Debug.Log("�����ڷ�ƾ��");
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
        if (callbackArrest != null) callbackArrest(this);
        callbackArrest = null;

        if (wantedThief) GameManager.Instance.Catch();
        else GameManager.Instance.Penalty();
    }

    private void OnCollisionEnter2D(Collision2D collision) //����, �� �浹�� ���⺯��
    {
        if(collision.rigidbody == false)
        {
            dir.x = -collision.contacts[0].point.x;
            dir.y = -collision.contacts[0].point.y;
            dir.Normalize();
        }
        //�̵� ���⿡ �ٸ� NPC�� ���� ��� �и鼭 �̵�
        //���� �ٸ� NPC�� ���� �ݴ� �������� �̵� �� �浹�� ��� �������� ���� �̵��ϴ� ������ �ƴ� �ٸ� �������� �̵�
        //���� �� NPC�� PC�� ���� �̵��ϴ� ���⿡ �ٸ� NPC�� �̵��ؼ� �̵��Ұ�� �ش� NPC�� �ٸ� �������� �̵�
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Prison")
        {
            Debug.Log("���� ������ ������");
            if (runningAway)
            {
                Debug.Log("���� �ᱹ...");
                arrest = true;
                dir = (collision.transform.position - this.transform.position).normalized;
                speed *= 0.7f;
                GameManager.Instance.ShowImg();
                StartCoroutine(nameof(Destroy));
            }
            else
            {
                Debug.Log("���� �� Ƣ����");
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
