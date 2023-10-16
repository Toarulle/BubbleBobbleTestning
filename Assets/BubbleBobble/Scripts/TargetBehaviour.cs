using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetBehaviour : MonoBehaviour
{
    public UnityAction<TargetBehaviour> onAttack = delegate{};

    public void Attack()
    {
        onAttack(this);
    }
}
