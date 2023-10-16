using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartEnemyMovement : MonoBehaviour
{
    [SerializeField]private Animator animator = null;
    private AnimatorStateInfo animStateInfo;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
