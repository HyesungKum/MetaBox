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

    Vector3 LookRight;
    Vector3 LookLeft;

    private void Reset()
    {
        mainCam = FindObjectOfType<Camera>();
    }

    private void Awake()
    {
        LookRight = this.transform.localScale;
        LookLeft = new Vector3(LookRight.x * -1, LookRight.y, LookRight.z);
        this.TryGetComponent(out agent);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            agent.SetDestination(worldPos);
            animator.SetBool("IsMove", agent.IsMove);
            CallMoveOrder();
        }

        if (!agent.IsLookRight) this.transform.localScale = LookRight;
        else this.transform.localScale = LookLeft;

        if (!agent.IsMove)
        {
            animator.SetBool("IsMove", agent.IsMove);
        }
    }
}