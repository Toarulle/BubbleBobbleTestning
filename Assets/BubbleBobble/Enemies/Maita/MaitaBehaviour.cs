using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaitaBehaviour : EnemyBehaviour
{
    [SerializeField] private float scanWidth = 0;
    [SerializeField] private float scanHeight = 4f;
    [Range(0,0.5f)][SerializeField] private float aggroHeightDiff = 0;
    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private Transform boulderOrigin;
    [SerializeField] private float reloadTime;
    [SerializeField] public float distanceToKeepFromPlayer;

    private bool reloading = false;
    private float attackTimer = 0f;

    public override void Update()
    {
        if (!isCaught && !isSpawning)
        {
            timer += Time.deltaTime;
            TargetBehaviour playerTarget = LookForTarget();
            if (playerTarget != null)
            {
                var targetDir = GetDirection(playerTarget.transform.position);
                var distToPlayer = Math.Abs(playerTarget.transform.position.x - transform.position.x);
                if (distToPlayer > distanceToKeepFromPlayer)
                {
                    rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
                }
                else if (distToPlayer < distanceToKeepFromPlayer-0.3f)
                {
                    rb.velocity = new Vector2(isFacingRight ? -speed : speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }

                if (!reloading && targetDir.y < aggroHeightDiff)
                {
                    Shoot();
                    reloading = true;
                }
                else
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= reloadTime)
                    {
                        reloading = false;
                        attackTimer = 0f;
                    }
                }

                if (!isFacingRight && playerTarget.transform.position.x > transform.position.x + 0.2f)
                {
                    Flip();
                }
                else if (isFacingRight && playerTarget.transform.position.x < transform.position.x - 0.2f)
                {
                    Flip();
                }
            }
            else
            {
                Patrolling();
            }
        }
        else
        {
            base.Update();
        }
        CheckAnger();
    }
    
    public void Shoot()
    {
        var boulder = Instantiate(boulderPrefab, boulderOrigin.position, Quaternion.identity).GetComponent<BoulderBehaviour>();
        boulder.BoulderStartingMovement(isFacingRight);
    }
    private Vector2 GetDirection(Vector3 targetPosition)
    {
        return targetPosition - transform.position;
    }
    
    private TargetBehaviour LookForTarget()
    {
        int layerMask = LayerMask.GetMask("Player");
        Collider2D playerCollider = Physics2D.OverlapBox(transform.position,
            new Vector2(scanWidth*2,scanHeight), 0, layerMask);
        if(playerCollider != null){
            return playerCollider.GetComponent<TargetBehaviour>();
        }
        return null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector2(scanWidth*2,scanHeight));
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