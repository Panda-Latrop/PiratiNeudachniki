using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHandEntity : MonoBehaviour, IMoveCharacter
{
    [SerializeField]
    protected DamageableCharacter damageable;
    [SerializeField]
    protected ForceMovement movement;
    [SerializeField]
    protected MobHandDesign design;
    [SerializeField]
    protected ParticleDeath death;
    [SerializeField]
    protected BodyFlip flip;
    [SerializeField]
    protected MoveAnimation anim;

    public IDamageable Damageable => damageable;
    public IMovement Movement => movement;

    protected void OnDeath(DamageInfo _deathInfo)
    {
        movement.IsCanMove = false;
        movement.StopMovement();
        death.Death(_deathInfo);
        enabled = false;
    }
    protected void OnRevive()
    {
        movement.IsCanMove = true;
        death.Revive();
        enabled = true;
    }

    #region Mono 
    protected void Awake()
    {
        damageable.SetDeathHandler(OnDeath);
        damageable.SetReviveHandler(OnRevive);
        death.Initialize(design);
        anim.Initialize(movement);
    }
    #region Update 
    protected void Update()
    {
        FlipChecker.Check(flip, movement.GetConstant());
    }
    protected void FixedUpdate()
    {
        movement.FixedUpdate();
    }
    protected void LateUpdate()
    {
        anim.LateUpdate();
    }
    #endregion
    protected void OnDestroy()
    {
        damageable.OnDestroy();
    }
    #endregion 
}
