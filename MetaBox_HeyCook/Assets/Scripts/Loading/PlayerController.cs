using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //=========================components===============================
    private Rigidbody2D rigidbody2d;
    private SimpleAnimator animator;

    //==========================pc settings=============================
    [Header("player Settings")]
    [SerializeField] float jumpFoce = 10f;
    [SerializeField] float mass = 1f;
    [SerializeField] float gravity = 1f;
    [Space]
    [SerializeField] float playerLastSpeed;
    //=======================inner variables============================
    private bool isGrounded;
    private bool isFall;
    private bool isDone;

    private void Awake()
    {
        //component
        TryGetComponent(out rigidbody2d);
        TryGetComponent(out animator);

        //apply player settings
        rigidbody2d.mass = mass;
        rigidbody2d.gravityScale = gravity;

        //inner variables
        isGrounded = true;
        isFall = false;
        isDone = false;

        //delegate chain
        TouchEventGenerator.Inst.touchBegan[0] += Jump;
        TouchEventGenerator.Inst.touchBegan[0] += Run;

        EventReciver.ButtonClicked += MoveRight;
    }

    private void OnDisable()
    {
        //application quit exception
        if (TouchEventGenerator.Inst == null) return;

        //delegate unchain
        TouchEventGenerator.Inst.touchBegan[0] -= Jump;
        TouchEventGenerator.Inst.touchBegan[0] -= Run;

        EventReciver.ButtonClicked -= MoveRight;
    }
    
    //============================player Controll==============================
    void Jump(int index, Vector3 pos)
    {
        if (!isGrounded || isFall) return;

        isGrounded = false;
        animator.ChangeAnimation("Jump");
        rigidbody2d.AddForce(Vector3.up * jumpFoce, ForceMode2D.Impulse);
    }
    void Run(int index, Vector3 pos)
    {
        if (!isFall) return;

        isFall = false;
        animator.ChangeAnimation("Idle");
        EventReciver.CallPlayerRise();
    }

    //============================End Scene Move===============================
    void MoveRight()
    {
        isDone = true;
        isFall = false;
        isGrounded = true;
        rigidbody2d.velocity = Vector2.right * playerLastSpeed; 
        animator.ChangeAnimation("Idle");
    }

    //===========================platform & obstacle===========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        if (isFall) return;

        animator.ChangeAnimation("Idle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone) return;

        if (collision.gameObject.name == "Rock")
        {
            isFall = true;
            collision.enabled = false;

            EventReciver.CallPlayerFall();
            animator.ChangeAnimation("Die");
        }
    }
}
