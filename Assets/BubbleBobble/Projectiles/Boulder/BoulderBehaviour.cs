using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoulderBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private float shootSpeed;
    
    private Collider2D collider = null;

    private static readonly int Hit = Animator.StringToHash("Hit");

    public bool Popped { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy"))
        {
            Destroy(gameObject);
        }
    }

    
    public void BoulderStartingMovement(bool moveRight)
    { 
        Vector3 localScale = transform.localScale;
        localScale.x *= moveRight ? 1 : -1;
        transform.localScale = localScale;        
        rb.velocity = new Vector2(moveRight ? shootSpeed : -shootSpeed, 0);
    }
    
    private void Bonk()
    {
        Popped = true;
        animator.SetTrigger(Hit);
        rb.isKinematic = true;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.GetComponent<TargetBehaviour>().Attack();
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Bonk();
        }
    }
}
