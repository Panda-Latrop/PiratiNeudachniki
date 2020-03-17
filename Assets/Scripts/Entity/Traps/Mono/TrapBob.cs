using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBob : BaseTrap
{
    [SerializeField]
    protected DamageDealer dealer;
    [SerializeField]
    protected BobMovement movement;
    [SerializeField]
    protected BobDesign design;

    public IDamageDealer Dealer { get => dealer; }
    public IBobMovement Movement { get => movement; }
    public IBobDesign Design { get => design; }

    public override void Hide()
    {
        IsActive = false;
    }
    public override void Unhide()
    {
        IsActive = true;
    }
    public void OnTriggerHandler(Collider2D _collider)
    {
        dealer.HurtHandler(_collider);
    }
    #region Mono
    protected void Start()
    {
        dealer.Team = TeamEnum.team0;
        dealer.SetInstantlyKill(true);
        movement.Initialize(rig);
    }
    protected void FixedUpdate()
    {
        movement.OnFixedUpdate();
    }
    protected void LateUpdate()
    {
       
    }
    protected void OnTriggerEnter2D(Collider2D _collider)
    {
        dealer.HurtHandler(_collider);
    }
    protected void OnDrawGizmosSelected()
    {
        if (rig != null)
        {
            movement.OnGizmos(rig.position, design.GetSphere().localPosition.magnitude);
            Gizmos.DrawLine(rig.position, rig.position + (Vector2)design.GetSphere().localPosition);
            design.OnGizmos((Vector3)rig.position + (Quaternion.Euler(0.0f,0.0f, movement.Angle/2.0f)*design.GetSphere().localPosition));
            design.OnGizmos((Vector3)rig.position + (Quaternion.Euler(0.0f, 0.0f, -movement.Angle / 2.0f) * design.GetSphere().localPosition));
            Gizmos.DrawSphere((Vector3)rig.position + design.GetSphere().localPosition, 0.1f);
            if (movement.Angle < 360)
            {
                float step = movement.Angle / 11.0f;
                Quaternion start = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                Vector2 spherePos = design.GetSphere().localPosition;
                Vector2 sphereRadius = new Vector2(0.0f, design.GetSphereRadius());
                for (int i = -6; i < 6; i++)
                {
                    Gizmos.DrawLine((Vector3)rig.position + Quaternion.Euler(0.0f, 0.0f, step * i) * start * (spherePos + sphereRadius), (Vector3)rig.position + Quaternion.Euler(0.0f, 0.0f, step * (i + 1)) * start * (spherePos + sphereRadius));
                    Gizmos.DrawLine((Vector3)rig.position + Quaternion.Euler(0.0f, 0.0f, step * i) * start * (spherePos - sphereRadius), (Vector3)rig.position + Quaternion.Euler(0.0f, 0.0f, step * (i + 1)) * start * (spherePos - sphereRadius));
                }
            }
            else
            {
                Vector2 spherePos = design.GetSphere().localPosition;
                Vector2 sphereRadius = new Vector2(0.0f, design.GetSphereRadius());
                Gizmos.DrawWireSphere(rig.position,spherePos.y - sphereRadius.y);
                Gizmos.DrawWireSphere(rig.position , spherePos.y + sphereRadius.y);
            }

        }
    }
    #endregion
}
public enum BobState
{
    left,right
}
public interface IBobMovement
{
    float Angle { get; set; }
    float MoveTime { get; }
    BobState State { get; set; }
}
[System.Serializable]
public class BobMovement : IBobMovement
{
    [SerializeField]
    protected float angle, time;
    [SerializeField]
    protected AnimationCurve smooth;
    [SerializeField]
    protected BobState state = BobState.right;
    protected Rigidbody2D root;
    protected float halfAngle;

    #region Properties
    public float Angle { get => angle; set => angle = value; }
    public float MoveTime { get => smooth[smooth.length].time;}
    public BobState State { get => state; set => state = value; }
    #endregion

    public void Initialize(Rigidbody2D _parent)
    {
        root = _parent;
        halfAngle = angle / 2.0f;     
    }
    public void OnFixedUpdate()
    {
        switch (state)
        {
            case BobState.left:
                MoveLeft();
                break;
            case BobState.right:
                MoveRight();
                break;
            default:
                break;
        }
    }
    protected void MoveLeft()
    {
        float currentTime = Time.time - time;
        if (root.rotation <= -halfAngle)
        {
            root.MoveRotation(-halfAngle);
            time = Time.time;
            state = BobState.right;
        }
        else
        {
            root.MoveRotation(Mathf.Lerp(-halfAngle, halfAngle, 1 - smooth.Evaluate(currentTime)));
        }
    }
    protected void MoveRight()
    {
        float currentTime = Time.time - time;
        if (root.rotation >= halfAngle)
        {
            root.MoveRotation(halfAngle);
            time = Time.time;
            state = BobState.left;
        }
        else
        {
            root.MoveRotation(Mathf.Lerp(-halfAngle, halfAngle, smooth.Evaluate(currentTime)));
        }
    }
    public void OnGizmos(Vector3 _pos,float _length)
    {
        Vector3 left = Quaternion.Euler(0.0f,0.0f,-angle/2.0f - 90.0f)* Vector2.right, right = Quaternion.Euler(0.0f, 0.0f, angle / 2.0f - 90.0f) * Vector2.right;        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_pos, _pos + left * _length);
        Gizmos.DrawLine(_pos, _pos + right * _length);
    }
}
public interface IBobDesign
{
    int Size { get; set; }
    Transform GetSphere();
    float GetSphereRadius();
    void SetStickLenght(float _lenght);
    Vector2 GetStickLenght();
    void SetDesign(ScriptableBobDesign _design);
    ScriptableBobDesign GetDesign();
    void AppyDesign();
}
[System.Serializable]
public class BobDesign : IBobDesign
{
    [SerializeField]
    protected int size = 0;
    [SerializeField]
    protected SpriteRenderer beam, stick, sphere;
    [SerializeField]
    protected Transform sphereTransform;
    [SerializeField]
    protected CircleCollider2D sphereCollider, sphereTrigger;
    [SerializeField]
    protected ScriptableBobDesign design;

    public int Size { get => size; set => size = value; }
    public Transform GetSphere()
    {
        return sphereTransform;
    }
    public float GetSphereRadius()
    {
        return sphere.size.x/2.0f;
    }
    public void SetStickLenght(float _lenght)
    {
        Vector2 sizeS = stick.size;
        sizeS.y = _lenght;
        stick.size = sizeS;
    }
    public Vector2 GetStickLenght()
    {
        return stick.size;
    }
    public void SetDesign(ScriptableBobDesign _design)
    {
        design = _design;
    }
    public ScriptableBobDesign GetDesign()
    {
        return design;
    }
    public void AppyDesign()
    {
        beam.sprite = design.GetSize(size).beam;
        stick.sprite = design.GetSize(size).stick;
        Vector2 stickSize = design.GetSize(size).stickSize;
        stickSize.y = stick.size.y;
        stick.size = stickSize;
        sphere.sprite = design.GetSize(size).sphere;
        sphereCollider.radius = design.GetSize(size).sphereTrigger - 0.1f;
        sphereTrigger.radius = design.GetSize(size).sphereTrigger;
    }
    public void OnGizmos(Vector2 _endPos)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_endPos, sphereCollider.radius);
        Gizmos.DrawWireSphere(_endPos, sphereTrigger.radius);
    }
}