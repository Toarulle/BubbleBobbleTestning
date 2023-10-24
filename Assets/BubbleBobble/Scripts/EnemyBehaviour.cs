using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public int points  = 0;
    [SerializeField] public float speed = 0;
    [SerializeField] public float angrySpeed = 0;
    [SerializeField] public float angryTime = 0;
    [SerializeField] public float maxFallSpeed = 0;
    [SerializeField] public TargetBehaviour target = null;
    [SerializeField] public DeathPortObject deathPort = null;
    [SerializeField] public Animator anim = null;
    [SerializeField] public Transform groundCheckFront = null;
    [SerializeField] private LayerMask groundLayer;

    public Rigidbody2D rb = null;

    public bool isFacingRight = false;
    [HideInInspector] public bool isCaught = false;
    [HideInInspector] public bool isDying = false;
    [HideInInspector] public bool isSpawning = true;
    [HideInInspector] public bool onPlatform = false;
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
        isCaught = false;
    }

    public virtual void Update()
    {
        if (isCaught || isSpawning || isDying)
        {
            if (transform.parent == null && !isDying)
            {
                anim.SetBool(Angry, isAngry);
                GetComponent<Collider2D>().isTrigger = false;
                GetComponent<Collider2D>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                rb.isKinematic = false;
                rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
                isCaught = false;
                isSpawning = false;
                return;
            }

            if (transform.parent.CompareTag("Bubble"))
            {

            }
        }
        else
        {
            timer += Time.deltaTime;

            Patrolling();
        }

        CheckAnger();
    }

    protected void Patrolling()
    {
        if (EndOfPlatformCheck() && onPlatform)
        {
            if (Random.Range(0,10) < 8)
            {
                Flip();
            }
            else
            {
                onPlatform = false;
            }
        }
        
        rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
        if (rb.velocity.y < 0f)
        {
            rb.velocity = new Vector2 (0, maxFallSpeed);
            onPlatform = false;
        }
    }
    
    protected void CheckAnger()
    {
        if (timer >= angryTime)
        {
            MakeAngry();
        }
    }

    private void CaughtInBubble()
    {
        MakeAngry();
        isCaught = true;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
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

    protected bool EndOfPlatformCheck()
    {
        var col = Physics2D.OverlapCircle(groundCheckFront.position, 0.2f, groundLayer);
        if (col == null)
        { 
            return true;
        }

        onPlatform = true;
        return false;
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Flip();
        }
    }

    private void GetCaughtOrKilled(TargetBehaviour target)
    {
        Debug.Log(isCaught);
        if (!isCaught)
        {
            CaughtInBubble();
        }
        else if (isCaught)
        {
            isDying = true;
            IsDying();
        }
    }

    private void IsDying()
    {
        deathPort.Killed(points);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        target.onAttack += GetCaughtOrKilled;
    }
    
    private void OnDisable()
    {
        target.onAttack -= GetCaughtOrKilled;
    }
}