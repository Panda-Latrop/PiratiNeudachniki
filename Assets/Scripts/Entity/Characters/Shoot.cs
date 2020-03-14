using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooterHandler
{
    IShooter Shooter { get; }
}
public interface IShooter
{
    bool IsCanShoot {get; set;}
    void Shoot();
    void SetUp(ScriptableWeapon _splWpn);
    void SetTimeToShoot(float _timeToShoot);
}
[System.Serializable]
public class Shooter : IShooter
{
    protected bool isCanShoot = true;
    [SerializeField]
    protected WeaponParameters weapon;
    [SerializeField]
    protected Transform shootPoint;
    protected IDamageable owner;
    protected float nextShoot = 0.0f;

    public void Initialize(IDamageable _owner)
    {
        owner = _owner;
        weapon.SetParameters();
    }

    public bool IsCanShoot
    {
        get => isCanShoot;
        set
        {
            isCanShoot = value;
            if (!value)
            {
                nextShoot = Time.time;
            }
        }
    }
    public void SetTimeToShoot(float _timeToShoot)
    {
        weapon.timeToShoot = _timeToShoot;
    }
    public void Shoot()
    {
        if (isCanShoot && Time.time >= nextShoot)
        {
            IPoolBullet bullet = (UnityPoolManager.Instance.Pop(weapon.bulletTag) as IPoolBullet);
            Quaternion angle = shootPoint.rotation * Quaternion.Euler(0.0f, 0.0f, Random.Range(-weapon.spread, weapon.spread));
            bullet.SetPosition(shootPoint.position);
            bullet.SetRotation(angle);
            bullet.SetBullet(owner.Team, weapon.damage, weapon.damageType);
            bullet.SetDirection(angle * Vector2.right, weapon.power);
            nextShoot = Time.time + weapon.timeToShoot;
        }
    }
    public void SetUp(ScriptableWeapon _splWpn)
    {
        weapon.SetParameters(_splWpn);
        shootPoint.localPosition = _splWpn.ShootPoint; 
    }
}