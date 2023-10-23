using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SFXPort", menuName = "Bubble Bobble/SFX Port", order = 2)]
public class SFXPortObject : ScriptableObject
{
    public UnityAction<string> OnPlaySound = delegate{};

    public void Play(string sfx)
    {
        OnPlaySound(sfx);
    }
}
