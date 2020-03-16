using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageDealer
{
    TeamEnum Team { get; set; }
    float Damage { get; set; }
    DamageType Type { get; set; }
    HurtReturn HurtHandler(Collider2D _target);
}


[System.Serializable]
public class DamageDealer : IDamageDealer
{
    protected enum TagCheck
    {
        noneTag,
        wallTag,
        damageableTag
    }
    protected TeamEnum team;
    protected bool instKill = false;
    protected float damage;
    protected DamageType type;
    [SerializeField]
    protected string[] wallTags;
    [SerializeField]
    protected string[] damageableTags;

    public TeamEnum Team { get => team; set => team = value; }
    public float Damage { get => damage; set => damage = value; }
    public DamageType Type { get => type; set => type = value; }
    public void SetInstantlyKill(bool _instantly)
    {
        instKill = _instantly;
    }
    public HurtReturn HurtHandler(Collider2D _target)
    {

        switch (TagEquals(_target.tag))
        {
            case TagCheck.noneTag:
                return HurtReturn.miss;
            case TagCheck.wallTag:
                return HurtReturn.hit;
            case TagCheck.damageableTag:
                {
                    IDamageable damageable = _target.GetComponent<IDamageableHandler>().Damageable;
                    if (damageable.Team == team)
                        return HurtReturn.miss;
                    if (damageable.Hurt(new DamageInfo(instKill ? damageable.MaxHealth : damage, type)))
                        return HurtReturn.kill;
                    else
                        return HurtReturn.enemy;
                }
        }
        return HurtReturn.miss;
    }
    protected TagCheck TagEquals(string _tag)
    {
        for (int i = 0; i < wallTags.Length; i++)
        {
            if (wallTags[i].Equals(_tag))
                return TagCheck.wallTag;
        }
        for (int i = 0; i < damageableTags.Length; i++)
        {
            if (damageableTags[i].Equals(_tag))
                return TagCheck.damageableTag;
        }
        return TagCheck.noneTag;
    }
}