using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    //=========================components===============================
    private Rigidbody2D rigidbody2d;
    [SerializeField] SimpleAnimator customAnimator;
    [SerializeField] Animator builtInAimator;

    //==========================pc settings=============================
    [Header("player Settings")]
    [SerializeField] AudioClip jumpSfx;
    [Space]
    [SerializeField] float jumpFoce = 10f;
    [SerializeField] float mass = 1f;
    [SerializeField] float gravity = 1f;
    [Space]
    [SerializeField] float playerLastSpeed;

    //=======================inner variables============================
    private bool isGrounded;
    private bool isFall;
    private bool isDone;
    private bool isBuiltIn;

    private void Awake()
    {
        //component
        TryGetComponent(out rigidbody2d);
        if(customAnimator == null) isBuiltIn = true;

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

        EventReceiver.ButtonClicked += MoveRight;
    }
    
    //============================player Controll==============================
    void Jump(int index, Vector3 pos)
    {
        if (!isGrounded || isFall) return;

        //state controll
        isGrounded = false;

        //jump sound out
        SoundManager.Inst.CallSfx(jumpSfx);

        //apply jump animation
        if (isBuiltIn) builtInAimator.SetTrigger("Jump");
        else customAnimator.ChangeAnimation("Jump");

        rigidbody2d.AddForce(Vector3.up * jumpFoce, ForceMode2D.Impulse);
    }
    void Run(int index, Vector3 pos)
    {
        if (!isFall) return;

        isFall = false;

        if (isBuiltIn) builtInAimator.SetBool("Idle", true);
        else customAnimator.ChangeAnimation("Idle");
        EventReceiver.CallPlayerRise();
    }

    //============================End Scene Move===============================
    void MoveRight()
    {
        isDone = true;
        isFall = false;
        isGrounded = true;
        rigidbody2d.velocity = Vector2.right * playerLastSpeed;
        if (isBuiltIn) builtInAimator.SetBool("Idle", true);
        else customAnimator.ChangeAnimation("Idle");

        //delegate unchain
        TouchEventGenerator.Inst.touchBegan[0] -= Jump;
        TouchEventGenerator.Inst.touchBegan[0] -= Run;

        EventReceiver.ButtonClicked -= MoveRight;
    }

    //===========================platform & obstacle===========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        if (isFall) return;

        if (isBuiltIn) builtInAimator.SetBool("Idle",true);
        else customAnimator.ChangeAnimation("Idle");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone) return;

        if (collision.gameObject.name == "Rock")
        {
            isFall = true;
            collision.enabled = false;

            EventReceiver.CallPlayerFall();

            if (isBuiltIn)
            {
                builtInAimator.SetTrigger("Die");
                builtInAimator.SetBool("Idle", false);
            }
            else customAnimator.ChangeAnimation("Die");
        }
    }
}
