using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionReference movement, jump, shoot;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator anim = null;
    [SerializeField] private GameObject bubblePrefab = null;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    

    private float movementInput;

    private bool isFacingRight = true;
    private bool isJumping = false;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int ShootTrigger = Animator.StringToHash("Shoot");

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

        if (IsOnGround() && !isJumping)
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
            Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        }
    }
    
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
