using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPoolBullet : IPoolObject
{
    void SetBullet(TeamEnum _team, float _damage, DamageType _type);
    void SetDirection(Vector2 _direction, float _power);
}
public class PoolBullet : PoolObject, IPoolBullet
{
    [SerializeField]
    protected BulletBase bullet;
    [SerializeField]
    protected BulletLifeCycle lifeCycle;
    #region Pool
    public override void OnPop()
    {
        base.OnPop();
        bullet.IsActive = true;
        lifeCycle.Restart();
    }
    public override void OnPush()
    {
        base.OnPush();
        bullet.IsActive = false;
    }
    public void SetBullet(TeamEnum _team, float _damage, DamageType _type)
    {
        bullet.SetDamage(_team, _damage, _type);
    }
    public void SetDirection(Vector2 _direction, float _power)
    {
        bullet.SetDirectiont(_direction, _power);
    }
    #endregion

    #region Mono
    //public void Start()
    //{
    //    lifeCycle.Restart();
    //}
    public void Update()
    {
        if (lifeCycle.IsActive)
        {
            switch (lifeCycle.UpdateCycle(bullet.IsActive))
            {
                case BulletLifeCycle.LifeCycleState.kick:
                    bullet.IsActive = false;
                    break;
                case BulletLifeCycle.LifeCycleState.death:
                    Push();
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}