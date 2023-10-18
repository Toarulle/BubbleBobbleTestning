using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public int points  = 0;
    [SerializeField] public float speed = 0;
    [SerializeField] public float angrySpeed = 0;
    [SerializeField] public float angryTime = 0;
    [SerializeField] public float maxFallSpeed = 0;
    [SerializeField] public TargetBehaviour target = null;
    [SerializeField] public Animator anim = null;
    public Rigidbody2D rb = null;

    public bool isFacingRight = false;
    [HideInInspector] public bool isCaught = true;
    [HideInInspector] public bool isAngry = false;
    [HideInInspector] public float timer = 0f;
    public static readonly int Angry = Animator.StringToHash("angry");


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isFacingRight)
        {
            Flip();
            isFacingRight = true;
        }

        GetComponent<Collider2D>().isTrigger = true;
        rb.isKinematic = true;
    }

    public virtual void Update()
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
            if (transform.parent == null)
            {
                anim.SetBool(Angry, isAngry);
                GetComponent<Collider2D>().isTrigger = false;
                rb.isKinematic = false;
                rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
                isCaught = false;
                return;
            }

            if (transform.parent.CompareTag("Bubble"))
            {

            }
        }
        else
        {
            timer += Time.deltaTime;
            rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
            if (rb.velocity.y < 0f)
            {
                rb.velocity = new Vector2 (0, maxFallSpeed);
            }
        }

        if (timer >= angryTime)
        {
            MakeAngry();
        }
    }

    // private Vector2 GetDirection(Vector3 targetPosition)
    // {
    //     Vector2 direction = (targetPosition - transform.position).normalized;
    //     return direction;
    // }
    //
    // private TargetBehaviour LookForTarget()
    // {
    //     int layerMask = LayerMask.GetMask("Player");
    //     Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, aggroRange, layerMask);
    //     if(playerCollider != null){
    //         return playerCollider.GetComponent<TargetBehaviour>();
    //     }
    //     else
    //     {
    //         return null;
    //     }
    // }
    //
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawWireSphere(transform.position, aggroRange);
    // }

    public void CaughtInBubble(TargetBehaviour target)
    {
        MakeAngry();
        isCaught = true;
        rb.velocity = Vector2.zero;
    }

    private void MakeAngry()
    {
        isAngry = true;
        speed = angrySpeed;
        anim.SetBool(Angry, isAngry);
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
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