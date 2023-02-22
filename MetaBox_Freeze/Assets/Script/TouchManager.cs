using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] Camera myCamera = null;
    [SerializeField] Police police = null;
    
    Vector3 touchedToScreen;
    Touch myTouch;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0) return;
        
        myTouch = Input.GetTouch(0);

        if (myTouch.phase == TouchPhase.Began)
        {
            touchedToScreen = myCamera.ScreenToWorldPoint(myTouch.position);
            touchedToScreen.z = 0;
            if (GameManager.Instance.IsGaming == false || Time.timeScale == 0) return;
            if (touchedToScreen.x > 4.6) touchedToScreen.x = 4.6f;
            police.Move(touchedToScreen);
        }
    }
}
