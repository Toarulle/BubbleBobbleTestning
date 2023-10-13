using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootRange;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float bounceLength;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask bubbleLayer;

    
    private float movedAmount = 0f;
    private Vector2 previousLocation = new Vector2();
    private Collider2D collider = null;
    private bool firstMovementDone = false;
    private bool floatingDown = false;
    private bool containEnemy = false;
    
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!containEnemy && col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            CatchEnemyInBubble(col.GetComponent<TargetBehaviour>());
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
        }

        if (trigger != "")
        {
            animator.SetTrigger(trigger);
        }
    }
}