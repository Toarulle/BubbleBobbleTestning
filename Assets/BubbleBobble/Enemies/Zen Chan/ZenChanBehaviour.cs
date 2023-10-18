using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenChanBehaviour : EnemyBehaviour
{
    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<TargetBehaviour>().Attack();
        }
    }
}