using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezerTriggerHandler : TriggerHandler
{
    [SerializeField]
    protected TrapSqueezer squeezer;

    protected void OnTriggerEnter2D(Collider2D _collider)
    {
        if(isActive)
        squeezer.OnTriggerHandler(_collider);
    }
}
