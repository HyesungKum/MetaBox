using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    [SerializeField] Police police = null;
    [SerializeField] GameObject touchEff = null;

    WaitForSeconds playEff = null;
    
    // Start is called before the first frame update
    void Start()
    {
        playEff = new WaitForSeconds(3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 movePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.mousePosition 은 스크린좌표계
            movePoint.z = 0;
            StartCoroutine(TouchEff(movePoint));
            if (GameManager.Instance.IsGaming == false || Time.timeScale == 0) return;
            if (movePoint.y < -3.2) movePoint.y = -3.2f;
            police.Move(movePoint);
        }
    }

    IEnumerator TouchEff(Vector3 movepoint)
    {
        GameObject eff = PoolCp.Inst.BringObjectCp(touchEff);
        eff.transform.position = movepoint;

        yield return playEff;
        PoolCp.Inst.DestoryObjectCp(eff);
    }
}
