using ObjectPoolCP;
using System.Collections;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] Camera myCamera = null;
    [SerializeField] Police police = null;
    [SerializeField] GameObject touchEff = null;

    WaitForSeconds playEff = null;
    Vector3 touchedToScreen;
    Touch myTouch;

    // Start is called before the first frame update
    void Start()
    {
        playEff = new WaitForSeconds(2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0) return;
        
        myTouch = Input.GetTouch(0);

        //if touched UI, no action required
        //if (EventSystem.current.IsPointerOverGameObject(myTouch.fingerId)) return;

        if (myTouch.phase == TouchPhase.Began)
        {
            touchedToScreen = myCamera.ScreenToWorldPoint(myTouch.position);
            touchedToScreen.z = 0;
            StartCoroutine(TouchEff(touchedToScreen));
            if (GameManager.Instance.IsGaming == false || Time.timeScale == 0) return;
            if (touchedToScreen.x > 4.6) touchedToScreen.x = 4.6f;
            police.Move(touchedToScreen);
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
