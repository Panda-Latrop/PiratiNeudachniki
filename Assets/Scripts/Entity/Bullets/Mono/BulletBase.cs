using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField]
    protected bool isActive = true;
    [SerializeField]
    protected DamageDealer dealer;

    public virtual bool IsActive { get => isActive; set => isActive = value; }

    public virtual void SetDamage(TeamEnum _team, float _damage, DamageType _type)
    {
        dealer.Team = _team;
        dealer.Damage = _damage;
        dealer.Type = _type;
    }
    public abstract void SetDirectiont(Vector2 _direction, float _power);

    protected virtual void ImpactHandler(Collider2D _collider)
    {
       dealer.HurtHandler(_collider);
    }
    #region Mono
    public void OnTriggerEnter2D(Collider2D _collider)
    {
        ImpactHandler(_collider);
    }
    #endregion
}
