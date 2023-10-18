using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootRange;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float bounceLength;
    [SerializeField] private float holdEnemyTime;
    [SerializeField] private float popCloseBubblesRange;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask bubbleLayer;

    
    private float movedAmount = 0f;
    private Vector2 previousLocation = new Vector2();
    private Collider2D collider = null;
    [HideInInspector]public bool firstMovementDone = false;
    private bool floatingDown = false;
    private bool containEnemy = false;

    private float containTimer = 0f;
    private static readonly int Pop = Animator.StringToHash("Pop");

    public bool Popped { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        previousLocation = transform.position;
    }

    private void FixedUpdate()
    {
        if (!firstMovementDone && (movedAmount > shootRange || Math.Abs(rb.velocity.x) <= 0.1f))
        {
            TurnToBigBubble();
        }
        else if (!firstMovementDone)
        {
            RecordDistance();
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, floatingSpeed);
        }

        if (containEnemy)
        {
            containTimer += Time.deltaTime;
            if (containTimer >= holdEnemyTime)
            {
                ReleaseEnemy();
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy"))
        {
            Destroy(gameObject);
        }
    }

    private void BounceAtCeiling()
    {
        if (floatingDown)
        {
            rb.velocity = new Vector2(0, -floatingSpeed);
        }

        if (movedAmount >= bounceLength)
        {
            floatingDown = false;
            rb.velocity = new Vector2(0, floatingSpeed);
        }
    }

    private void PopCloseBubbles()
    {
        var bubbles = Physics2D.OverlapCircleAll(transform.position, popCloseBubblesRange, bubbleLayer);
        foreach (var bubble in bubbles)
        {
            var bubBeh = bubble.GetComponent<BubbleBehaviour>();
            if (!bubBeh.Popped)
            {
                bubBeh.PopBubble();
            }
        }
    }
    
    public void PopBubble()
    {
        Popped = true;
        animator.SetTrigger(Pop);
        rb.isKinematic = true;
        floatingSpeed = 0;
        collider.isTrigger = true;
        containTimer = 0;
        if (containEnemy)
        {
            GameBehaviour.Instance.PopBubbleWithEnemySound();
        }
        PopCloseBubbles();
    }
    
    private void TurnToBigBubble()
    {
        rb.velocity = new Vector2(0, floatingSpeed);
        collider.isTrigger = false;
        firstMovementDone = true;
        movedAmount = 0f;
    }
    public void BubbleStartingMovement(bool moveRight)
    {
        rb.velocity = new Vector2(moveRight ? shootSpeed : -shootSpeed, 0);
    }

    private void RecordDistance()
    {
        var currentPos = transform.position;
        movedAmount += Vector2.Distance(currentPos, previousLocation);
        previousLocation = currentPos;
    }

    private void CatchEnemyInBubble(TargetBehaviour target)
    {
        target.transform.SetParent(transform);
        target.Attack();
        containEnemy = true;
        TurnToBigBubble();
    }

    private void ReleaseEnemy()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.DetachChildren();
        containEnemy = false;
        PopBubble();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,popCloseBubblesRange);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!firstMovementDone && !containEnemy && col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            CatchEnemyInBubble(col.GetComponent<TargetBehaviour>());
            col.gameObject.SetActive(false);
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            TurnToBigBubble();
        }

        string trigger = "";
        switch (col.tag)
        {
            case "ZenChan":
                trigger = "HoldingZenChan";
                break;
            case "Maita":
                trigger = "HoldingMaita";
                break;
        }

        if (trigger != "")
        {
            animator.SetTrigger(trigger);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (firstMovementDone && col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PopBubble();
        }
    }
}
