using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleArea = null;
    [SerializeField] Sprite playerImg;
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
        this.transform.position = new Vector3(Random.Range(-2f, 5f), Random.Range(-3f, 2f), 0);
    }

    void DataSetting()
    {
        playerSpeed = GameManager.Instance.FreezeData.playerSpeed * 0.02f;
        playerArea = GameManager.Instance.FreezeData.playerArea * 0.6f;
        circleArea.radius *= playerArea;
    }

    void Update()
    {
        if (GameManager.Instance.IsGaming == false || Time.timeScale == 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 movePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePoint.z = 0;
            if (movePoint.y < -3.2) movePoint.y = -3.2f;
            isMoving = false;
            StartCoroutine(_Move(movePoint));
        }
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
