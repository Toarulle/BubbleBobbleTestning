using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DeathPort", menuName = "Bubble Bobble/Death Port", order = 1)]
public class DeathPortObject : ScriptableObject
{
    public UnityAction<int> OnDeath = delegate{};

    public void Killed(int pts)
    {
        OnDeath(pts);
    }
}
