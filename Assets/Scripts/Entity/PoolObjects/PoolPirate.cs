using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolPirate : IPoolObject
{

    bool IsAlive { get; }
    void SetMove(float _direction);
    void SetJump(float _direction);
    void AddJumpSpeed(float _min, float _max);
    void SetRotation(float _angle, Quaternion _quaternion);
    void SetCanRotation(bool _active);
    void SetWeapon(ScriptableWeapon _splWpn);
    void AddTimeToShoot(float _min, float _max);
    void AddTimeToShootProcent(float _min, float _max);
    void Shoot();
    void SetAIActive(bool _active);
    void AddPadding(float _min, float _max);
    void SetTarget(Transform _target);
    void SetGroupMaster(IGroupMaster _master);
    //void SetDeathHandler(System.Action<DamageInfo> _deathHandler);
    //PirateEntity Entity { get; }
    //PirateAI EntityAI { get; }
}
public class PoolPirate : PoolObject, IPoolPirate
{
    //[SerializeField]
    //protected int id;
    [SerializeField]
    protected PirateEntity pirate;
    [SerializeField]
    protected PirateAI pirateAI;
    protected float jumpCashe, paddingCashe, timeToShootCashe;
    protected IGroupMaster master;

    public void Awake()
    {
        jumpCashe = pirate.Movement.JumpSpeed;
        paddingCashe = pirateAI.Follow.Padding;
        pirate.Damageable.SetDeathHandler(OnPirateDeath);
    }

    public bool IsAlive => pirate.Damageable.IsAlive;
    public void SetMove(float _direction)
    {
        pirate.Movement.Move(_direction);
    }
    public void SetJump(float _direction)
    {
        pirate.Movement.Jump(_direction);
    }
    public void AddJumpSpeed(float _min, float _max)
    {
        pirate.Movement.JumpSpeed = jumpCashe + Random.Range(_min, _max);
    }
    public void SetRotation(float _angle, Quaternion _quaternion)
    {
        pirate.Rotation.SetRotation(_angle, _quaternion);
    }
    public void SetCanRotation(bool _active)
    {
        pirate.Rotation.IsCanRotation = _active;
    }
    public void SetWeapon(ScriptableWeapon _splWpn)
    {
        timeToShootCashe = _splWpn.TimeToShoot;
        pirate.Shooter.SetUp(_splWpn);
    }
    public void AddTimeToShoot(float _min, float _max)
    {
        pirate.Shooter.SetTimeToShoot(timeToShootCashe + Random.Range(_min, _max));
    }
    public void AddTimeToShootProcent(float _min, float _max)
    {
        pirate.Shooter.SetTimeToShoot(timeToShootCashe + timeToShootCashe * Random.Range(_min, _max));
    }
    public void Shoot()
    {
        pirate.Shooter.Shoot();
    }
    public void SetAIActive(bool _active)
    {
        pirateAI.IsActive = _active;
    }
    public void AddPadding(float _min, float _max)
    {
        pirateAI.Follow.Padding = paddingCashe + Random.Range(_min, _max);
    }
    public void SetTarget(Transform _target)
    {
        pirateAI.Follow.Target = _target;
    }
    public void SetGroupMaster(IGroupMaster _master)
    {
        master = _master;
    }
    //public void SetDeathHandler(System.Action<DamageInfo> _deathHandler)
    //{
    //    pirate.Damageable.SetDeathHandler(_deathHandler);
    //}
    protected void OnPirateDeath(DamageInfo _damageInfo)
    {
        master.UpdateGroup();
    }

    //public PirateEntity Entity => pirate;
    //public PirateAI EntityAI => pirateAI;
}