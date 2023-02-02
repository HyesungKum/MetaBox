using System.Collections;
using UnityEngine;

public class Police : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleArea = null;
    [SerializeField] Sprite playerImg; //PC �𵨸��� �̴ϰ��� �������� ������ Ŀ���͸���¡�� �̹����� ���
    [SerializeField] float playerSpeed;
    [SerializeField] float playerArea;

    bool isMoving = false;

    private void Awake()
    {
        if(circleArea == null) this.gameObject.TryGetComponent<CircleCollider2D>(out circleArea);
        GameManager.Instance.freezeDataSetting = Setting;
    }

    void Setting()
    {
        this.transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0); //�� ��� ĳ���ʹ� ��ġ�� �ʰ� ��ġ
        playerSpeed = GameManager.Instance.FreezeData.playerSpeed * 0.02f;
        playerArea = GameManager.Instance.FreezeData.playerArea * 0.8f;
        circleArea.radius *= playerArea;
    }

    public void Move(Vector3 movepoint)
    {
        isMoving = false;
        StartCoroutine(_Move(movepoint));
    }

    IEnumerator _Move(Vector3 movepoint)
    {
        yield return null; //�̵��߿� ���� �����ϴ� ��� ������ ���ư��� �ڷ�ƾ ����
        isMoving = true;
        while (isMoving && Time.timeScale != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movepoint, playerSpeed);
            //transform.position = Vector3.SmoothDamp(transform.position, movepoint, ref speed, 0.1f);

            if (transform.position.Equals(movepoint)) yield break;
            yield return null;
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.rigidbody == null) isMoving = false; //npc�� �⵿ �� �и鼭 ����
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 100, 100, 10);
        Gizmos.DrawSphere(this.transform.position, circleArea.radius);
    }
}
