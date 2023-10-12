using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenChanBehaviour : MonoBehaviour
{
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float speed = 0;
    private TargetBehaviour target = null;
    private Rigidbody2D rb = null;

    void Start()
    {
        target = GetComponent<TargetBehaviour>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TargetBehaviour playerTarget = LookForTarget();
        if (playerTarget != null)
        {
            var targetDir = GetDirection(playerTarget.transform.position);
            rb.position += Time.deltaTime * speed * targetDir;
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
}
