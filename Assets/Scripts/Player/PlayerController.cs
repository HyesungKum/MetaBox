using UnityEngine;
using static PlayerController;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    private NavMesh2Agent agent;
    public Animator animator;

    //플레이어 이동 명령 이벤트
    public delegate void MoveOrder();

    public MoveOrder moveOrder;

    public void CallMoveOrder() => moveOrder?.Invoke();
    


    private void Reset()
    {
        mainCam = FindObjectOfType<Camera>();
    }

    private void Awake()
    {
        this.TryGetComponent(out agent);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            agent.SetDestination(worldPos);
            CallMoveOrder();
        }
    }
}