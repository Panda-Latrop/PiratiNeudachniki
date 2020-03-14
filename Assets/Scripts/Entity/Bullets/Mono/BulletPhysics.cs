using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : BulletBase
{
    [SerializeField]
    protected SimpleMovement movement;
    [SerializeField]
    protected BulletEffects effects;

    public override bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if (value)
            {
                effects.SetSpriteActive(true);
                effects.SetImpactAction(ParticleAction.clear);
                effects.SetTrailAction(ParticleAction.play);
            }
            else
            {
                effects.SetSpriteActive(false);
                movement.SetVelocity(Vector2.zero);
                effects.SetTrailAction(ParticleAction.stop);
                effects.SetImpactAction(ParticleAction.play);
            }
        }
    }

    public override void SetDirectiont(Vector2 _direction, float _power)
    {
        movement.SetVelocity(_direction * _power);
    }

    protected override void ImpactHandler(Collider2D _collider)
    {
        if (dealer.HurtHandler(_collider) > HurtReturn.miss)
        {
            IsActive = false;
        }
    }
}