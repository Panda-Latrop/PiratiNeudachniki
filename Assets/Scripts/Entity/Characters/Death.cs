using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeathHandler
{
    void Death(DamageInfo _damageInfo);
    void Revive();
}
public abstract class DeathHandler : IDeathHandler
{
    public abstract void Death(DamageInfo _damageInfo);
    public abstract void Revive();
}
[System.Serializable]
public class PirateDeath : DeathHandler
{
    protected IPirateDesign design;
    [SerializeField]
    protected ParticleSystem particle;
    public void Initialize(IPirateDesign _design)
    {
        design = _design;
    }
    public override void Death(DamageInfo _damageInfo)
    {
        design.SetEnable(false);
        particle.Play(true);
    }
    public override void Revive()
    {
        design.SetEnable(true);
        particle.Stop(true);
        particle.Clear(true);
    }
}