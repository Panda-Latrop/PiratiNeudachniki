using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    [SerializeField]
    protected PoolBullet bullet;
    [SerializeField]
    protected DamageType damageType;
    [SerializeField]
    protected float damage,power,fireRate, spread;
    [SerializeField]
    protected Vector3 shootPoint;

    public string BulletTag => bullet.PoolTag;
    public DamageType DamageType => damageType;
    public float Damage => damage;
    public float Power => power;
    public float FireRate => fireRate;
    public float TimeToShoot => 1.0f/ fireRate;
    public float Spread => spread;
    public Vector2 ShootPoint => shootPoint;
}

public struct WeaponParameters
{
    public string bulletTag;
    public DamageType damageType;
    public float damage, power, timeToShoot, spread;

    public void SetParameters()
    {
        bulletTag = "GB";
        damageType = DamageType.physic;
        damage = 1.0f;
        power = 1.0f;
        timeToShoot = 1.0f;
        spread = 0.0f;
    }
    public  void SetParameters(ScriptableWeapon _splWpn)
    {
        bulletTag = _splWpn.BulletTag;
        damageType = _splWpn.DamageType;
        damage = _splWpn.Damage;
        power = _splWpn.Power;
        timeToShoot = _splWpn.TimeToShoot;
        spread = _splWpn.Spread;
    }

}