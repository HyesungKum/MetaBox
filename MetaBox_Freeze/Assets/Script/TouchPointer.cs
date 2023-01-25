using UnityEngine;

public class TouchPointer : MonoBehaviour
{
    [SerializeField] PlayerMove player = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGaming == false || Time.timeScale == 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 movePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePoint.z = 0;
            player.Move(movePoint);
            
        }
    }
}
