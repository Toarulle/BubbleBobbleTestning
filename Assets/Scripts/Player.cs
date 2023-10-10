using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private InputActionReference movement, jump, shoot;
    [SerializeField] private Rigidbody2D rb = null;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    

    private float movementInput;
    public Vector2 currentVelocity;

    private bool isFacingRight = true;

    void Update()
    {
        rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y);

        if (rb.velocity.y < 0f) rb.velocity = new Vector2 (rb.velocity.x, maxFallSpeed);
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

    private bool IsOnGround()
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
