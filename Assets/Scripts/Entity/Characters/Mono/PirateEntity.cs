using System.Collections;
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
    protected PirateFlip flip;
    [SerializeField]
    protected PirateAnimation anim;
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
        enabled = false;
    }
    [ContextMenu("Kill")]
    protected void Kill()
    {
        damageable.Hurt(new DamageInfo(damageable.MaxHealth));
    }
    #region Mono 
    protected void Awake()
    {
        damageable.SetDeathHandler(OnDeath);        
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
        flip.Checker(movement.GetConstant());
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
    protected void OnDrawGizmosSelected()
    {
        RaycastHit2D[] contact = movement.GetContactPoints();
        for (int i = 0; i < movement.GetContactCount(); i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(contact[i].point, 0.15f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(contact[i].point, contact[i].point + contact[i].normal * 0.4f);
        }
    }
    #endregion 
}
