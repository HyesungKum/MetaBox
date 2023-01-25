using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleArea = null;
    [SerializeField] Sprite playerImg; //PC 모델링은 미니게임 마을에서 유저가 커스터마이징한 이미지로 사용
    [SerializeField] float playerSpeed;
    [SerializeField] float playerArea;

    bool isMoving = false;

    private void Awake()
    {
        if(circleArea == null) circleArea = this.gameObject.GetComponent<CircleCollider2D>();
        GameManager.Instance.FreezeDataSetting += DataSetting;
    }

    void Start()
    {
        this.transform.position = new Vector3(Random.Range(-2f, 5f), Random.Range(-3f, 2f), 0); //단 모든 캐릭터는 겹치지 않게 배치
    }

    void DataSetting()
    {
        playerSpeed = GameManager.Instance.FreezeData.playerSpeed * 0.02f;
        playerArea = GameManager.Instance.FreezeData.playerArea * 0.6f;
        circleArea.radius *= playerArea;
    }

    public void Move(Vector3 movepoint)
    {
        isMoving = false;
        StartCoroutine(_Move(movepoint));
    }

    IEnumerator _Move(Vector3 movepoint)
    {
        yield return new WaitForSeconds(0.05f);
        isMoving = true;
        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, movepoint, playerSpeed);
            //transform.position = Vector3.SmoothDamp(transform.position, movepoint, ref speed, 0.1f);

            if (transform.position.Equals(movepoint)) yield break;
            if (Time.timeScale == 0) yield break;
            yield return null;
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        isMoving = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 100, 100, 10);
        Gizmos.DrawSphere(this.transform.position, circleArea.radius);
    }
}
