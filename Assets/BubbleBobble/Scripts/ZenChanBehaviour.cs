using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenChanBehaviour : MonoBehaviour
{
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float speed = 0;
    [SerializeField] private float maxFallSpeed = 0;
    [SerializeField] private TargetBehaviour target = null;
    private Rigidbody2D rb = null;

    public bool isFacingRight = false;
    private bool isCaught = false;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isFacingRight)
        {
            Flip();
            isFacingRight = true;
        }
    }

    void Update()
    {
        // TargetBehaviour playerTarget = LookForTarget();
        // if (playerTarget != null)
        // {
        //     var targetDir = GetDirection(playerTarget.transform.position);
        //     rb.position += Time.deltaTime * speed * targetDir;
        // }
        // if (!isFacingRight && rb.velocity.x > 0f)
        // {
        //     Flip();
        // }
        // else if (isFacingRight && rb.velocity.x < 0f)
        // {
        //     Flip();
        // }
        if (isCaught)
        {
            
        }
        else
        {
            if (rb.velocity.y < 0f)
            {
                rb.velocity = new Vector2 (rb.velocity.x, maxFallSpeed);
            }
            rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
        }
    }

    private Vector2 GetDirection(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        return direction;
    }
    
    private TargetBehaviour LookForTarget()
    {
        int layerMask = LayerMask.GetMask("Player");
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, aggroRange, layerMask);
        if(playerCollider != null){
            return playerCollider.GetComponent<TargetBehaviour>();
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public void CaughtInBubble(TargetBehaviour target)
    {
        isCaught = true;
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Flip();
        }
    }

    private void OnEnable()
    {
        target.onAttack += CaughtInBubble;
    }
    
    private void OnDisable()
    {
        target.onAttack -= CaughtInBubble;
    }
}