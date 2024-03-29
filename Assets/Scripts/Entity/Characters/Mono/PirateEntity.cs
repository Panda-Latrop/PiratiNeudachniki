﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateEntity : MonoBehaviour, IPirateCharacter
{
    [SerializeField]
    protected DamageableCharacter damageable;
    [SerializeField]
    protected JumpAccelMovement movement;
    [SerializeField]
    protected PirateRotation rotation;
    [SerializeField]
    protected PirateDesign design;
    [SerializeField]
    protected ParticleDeath death;
    [SerializeField]
    protected PirateFlip flip;
    [SerializeField]
    protected MoveJumpAnimation anim;
    [SerializeField]
    protected Shooter shooter;

    public IDamageable Damageable => damageable;
    public IMovement Movement => movement;
    public IRotation Rotation => rotation;
    public IShooter Shooter => shooter;

   protected  void OnDeath(DamageInfo _deathInfo)
    {
        movement.IsCanMove = false;
        rotation.IsCanRotation = false;
        shooter.IsCanShoot = false;
        movement.StopMovement();
        death.Death(_deathInfo);
        enabled = false;
    }
    protected void OnRevive()
    {
        movement.IsCanMove = true;
        rotation.IsCanRotation = true;
        shooter.IsCanShoot = true;
        death.Revive();
        enabled = true;
    }
    #region Mono 
    protected void Awake()
    {
        damageable.SetDeathHandler(OnDeath);
        damageable.SetReviveHandler(OnRevive);
        death.Initialize(design);
        flip.Initialize();
        anim.Initialize(movement);
        shooter.Initialize(damageable);
    }
    protected void Start()
    {
        rotation.IsCanRotation = false;
    }
    #region Update 
    protected void Update()
    {
        FlipChecker.Check(flip, movement.GetConstant());
        if (!rotation.IsCanRotation)
            rotation.SetDefaultRotation(flip.IsFlipped);
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
