using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaitaBehaviour : EnemyBehaviour
{
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float aggroHeightDiff = 0;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private Transform groundCheck;

    // public override void Update()
    // {
    //     TargetBehaviour playerTarget = LookForTarget();
    //     if (playerTarget != null)
    //     {
    //         var targetDir = GetDirection(playerTarget.transform.position);
    //         //rb.position += Time.deltaTime * speed * targetDir;
    //         if (targetDir.y > transform.position.y+aggroHeightDiff)
    //         {
    //             RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.up, 4f, LayerMask.NameToLayer("Platforms"));
    //             Debug.Log(ray + " - " + ray.distance);
    //             if (IsOnGround() && ray.distance < jumpForce)
    //             {
    //                 JumpToPlatform();
    //             }
    //             else
    //             {
    //                 rb.position += Time.deltaTime * speed * targetDir;
    //             }
    //         }
    //         else if (targetDir.y < transform.position.y-aggroHeightDiff)
    //         {
    //             
    //         }
    //         
    //         if (!isFacingRight && rb.velocity.x > 0f)
    //         {
    //             Flip();
    //         }
    //         else if (isFacingRight && rb.velocity.x < 0f)
    //         {
    //             Flip();
    //         }
    //     }
    //     else
    //     {
    //         base.Update();
    //     }
    // }

    private void JumpToPlatform()
    {
        rb.velocity = new Vector2(0, jumpForce);
    }
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.NameToLayer("Platforms"));
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

    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<TargetBehaviour>().Attack();
        }
    }
}