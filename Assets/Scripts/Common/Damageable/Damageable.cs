using System;
using UnityEngine;

public interface IDamageableHandler
{
    IDamageable Damageable { get; }
}
public interface IDeathHandler
{
    void OnDeath(DamageInfo _deathInfo);
}
public enum DamageType
{
    physic, 
    ice, 
    toxic, 
    fire, 
    explosion, 
    plasma,
}
public struct DamageInfo
{
    public float damage;
    public DamageType type;

    public DamageInfo(float _damage)
    {
        damage = _damage;
        type = DamageType.physic;
    }
    public DamageInfo(float _damage,DamageType _type)
    {
        damage = _damage;
        type = _type;
    }
}
public enum HurtReturn
{
    miss,
    hit,
    enemy,
    kill
}
[System.Serializable]
public struct ResistanceStruct
{
    public DamageType type;
    [Range(-1,1)]
    public float resist;
}
public enum TeamEnum
{
    team0,
    team1,
    team2,
    team3,
    team4
}
public interface IDamageable
{
    bool IsAlive { get; }
    float Health { get; }
    float MaxHealth { get; }
    TeamEnum Team { get; }
    void SetDeathHandler(Action<DamageInfo> _deathHandler);
    bool Hurt(DamageInfo _damageInfo);
    void Death(DamageInfo _damageInfo);
}
[System.Serializable]
public class DamageableCharacter : IDamageable
{
    [SerializeField]
    protected bool isAlive = true;
    [SerializeField]
    protected float maxHealth;
    protected float health;
    [SerializeField]
    protected TeamEnum team;
    [SerializeField]
    protected ResistanceStruct[] resists;

    protected event Action<DamageInfo> DeathEvent;

    public bool IsAlive => isAlive;
    public float Health => health;
    public float MaxHealth => maxHealth;
    public TeamEnum Team => team;

    public void SetDeathHandler(Action<DamageInfo> _deathHandler)
    {
        DeathEvent += _deathHandler;
    }

    public virtual bool Hurt(DamageInfo _damageInfo)
    {
        float damage = _damageInfo.damage;
        for (int i = 0; i < resists.Length; i++)
        {
            if (_damageInfo.type == resists[i].type)
            {
                damage *= 1 - resists[i].resist;
                break;
            }
        }
        health -= damage;
        if (health <= 0)
        {
            Death(_damageInfo);
            return true;
        }
        return false;
    }
    public virtual void Death(DamageInfo _damageInfo)
    {
        isAlive = false;
        DeathEvent?.Invoke(_damageInfo);
    }
    public void OnDestroy()
    {
        DeathEvent = null;
    }
}