using System.Collections;
using System.Collections.Generic;
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

        //delegate chain
        TouchEventGenerator.Inst.touchBegan[0] += Jump;

        EventReciver.ButtonClicked += MoveRight;
    }

    private void OnDisable()
    {
        //application quit exception
        if (TouchEventGenerator.Inst == null) return;

        //delegate unchain
        TouchEventGenerator.Inst.touchBegan[0] -= Jump;

        EventReciver.ButtonClicked -= MoveRight;
    }
    
    //============================player Controll==============================
    void Jump(int index, Vector3 pos)
    {
        if (!isGrounded) return;

        animator.ChangeAnimation("Jump");
        isGrounded = false;
        rigidbody2d.AddForce(Vector3.up * jumpFoce, ForceMode2D.Impulse);
    }

    void MoveRight()
    {
        rigidbody2d.velocity = Vector2.right * playerLastSpeed;
    }

    //===========================platform & obstacle===========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.ChangeAnimation("Idle");
        isGrounded = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Rock")
        {
            animator.ChangeAnimation("Die");
        }
    }
}
