using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab = null;
    [SerializeField] private Transform bubbleOrigin = null;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask bubbleLayer;
    [SerializeField] private TargetBehaviour target = null;
    [SerializeField] private DeathPortObject deathPort = null;
    [SerializeField] private SFXPortObject sfxPort = null;
    
    private Rigidbody2D rb = null;
    private Animator anim = null;

    private float movementInput;

    public bool isFacingRight = true;
    private bool isJumping = false;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int ShootTrigger = Animator.StringToHash("Shoot");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y);

        if (rb.velocity.y < 0f)
        {
            rb.velocity = new Vector2 (rb.velocity.x, maxFallSpeed);
            anim.SetBool(IsFalling, true);
            anim.SetBool(IsJumping, false);
            isJumping = false;
        }
        else
        {
            anim.SetBool(IsFalling, false);
        }

        if (!isJumping && IsOnGround())
        {
            anim.SetBool(IsFalling, false);
            if (Math.Abs(rb.velocity.x) > 0f)
            {
                anim.SetBool(IsWalking, true);
            }
            else
            {
                anim.SetBool(IsWalking, false);
            }
        }
        if (!isFacingRight && movementInput > 0f)
        {
            Flip();
        }
        else if (isFacingRight && movementInput < 0f)
        {
            Flip();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool(IsJumping, true);
            isJumping = true;
            sfxPort.Play("PlayerJump");
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>().x;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger(ShootTrigger);
            var bubble = Instantiate(bubblePrefab, bubbleOrigin.position, Quaternion.identity).GetComponent<BubbleBehaviour>();
            bubble.BubbleStartingMovement(isFacingRight);
            sfxPort.Play("ShootBubble");
        }
    }
    
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer)||Physics2D.OverlapCircle(groundCheck.position, 0.2f, bubbleLayer);
    }
    
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void Attacked(TargetBehaviour targetBehaviour)
    {
        deathPort.Killed(-1);
        Destroy(gameObject);
    }
    
    private void OnEnable()
    {
        target.onAttack += Attacked;
    }
    private void OnDisable()
    {
        target.onAttack -= Attacked;
    }
}
