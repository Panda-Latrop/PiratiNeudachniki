using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAI : BaseAI
{
    [SerializeField]
    protected PirateEntity pirate;
    [SerializeField]
    protected AIPirateFollow follow;

    public AIPirateFollow Follow => follow;
    #region Mono
    protected void Awake()
    {
        IsActive = false;
        follow.Initialize((IJumpAccel)pirate.Movement);
    }
    protected void LateUpdate()
    {
        follow.OnStateUpdate();
    }
    #endregion
}
