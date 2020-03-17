using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSqueezer : BaseTrap
{
    [SerializeField]
    protected DamageDealer dealer;
    [SerializeField]
    protected SqueezerMovement movement;
    [SerializeField]
    protected SqueezerAnimation anim;
    [SerializeField]
    protected SqueezerDesign design;
    [SerializeField]
    protected SqueezerTriggerHandler triggerHandler;
    public IDamageDealer Dealer{get => dealer;}
    public ISqueezerMovement Movement { get => movement;}
    public ISqueezerAnimation Animation { get => anim; }
    public ISqueezerDesign Design { get => design; }

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
        anim.Initialize(movement.GetPress());
    }
    protected void FixedUpdate()
    {
        movement.OnFixedUpdate();
    }
    protected void LateUpdate()
    {
        anim.OnLateUpdate();
        if (movement.MoveState == SqueezerMovementState.up)
            triggerHandler.IsActive = false;
        else
            triggerHandler.IsActive = true;
    }
    protected void OnDrawGizmosSelected()
    {
        if (rig != null)
        {
            movement.OnGizmos(rig.position);
            design.OnGizmos(movement.EndPoint + rig.position);
        }
    }
    #endregion
}

public enum SqueezerMovementState
{
    up, down
}
public enum SqueezerState
{
    move, idle
}
public interface ISqueezerMovement
{
    Vector2 StartPoint { get; set; }
    Vector2 EndPoint { get; set; }
    float MoveUpTime { get; set; }
    float MoveDownTime { get; set; }
    float IdleUpTime { get; set; }
    float IdleDownTime { get; set; }
    SqueezerState State { get; set; }
    SqueezerMovementState MoveState { get; set; }
    Transform GetPress();
}
[System.Serializable]
public class SqueezerMovement : ISqueezerMovement
{

    [SerializeField]
    protected Vector2 startPoint, endPoint;
    [SerializeField]
    protected float moveUpTime, moveDownTime, idleUpTime, idleDownTime;
    [SerializeField]
    protected SqueezerState state = SqueezerState.move;
    [SerializeField]
    protected SqueezerMovementState moveState = SqueezerMovementState.down;
    [SerializeField]
    protected Rigidbody2D press;
    protected Rigidbody2D parent;
    protected float upSeed, downSpeed, nextIdle; //distance,
    protected Vector2 direction;

    #region Properties
    public Vector2 StartPoint { get => startPoint; set => startPoint = value; }
    public Vector2 EndPoint { get => endPoint; set => endPoint = value; }
    public float MoveUpTime { get => moveUpTime; set => moveUpTime = value; }
    public float MoveDownTime { get => moveDownTime; set => moveDownTime = value; }
    public float IdleUpTime { get => idleUpTime; set => idleUpTime = value; }
    public float IdleDownTime { get => idleDownTime; set => idleDownTime = value; }
    public SqueezerState State { get => state; set => state = value; }
    public SqueezerMovementState MoveState { get => moveState; set => moveState = value; }
    public Transform GetPress()
    {
        return press.transform;
    }
    #endregion

    public void Initialize(Rigidbody2D _parent)
    {
        parent = _parent;
        direction = endPoint - startPoint;
        float distance = direction.magnitude;

        direction.Normalize();
        upSeed = distance / moveUpTime;
        downSpeed = distance / moveDownTime;
    }
    public void OnFixedUpdate()
    {
        switch (state)
        {
            case SqueezerState.move:
                switch (moveState)
                {
                    case SqueezerMovementState.up:
                        if (MoveUp())
                        {

                            nextIdle = Time.time + idleUpTime;
                            state = SqueezerState.idle;
                        }
                        break;
                    case SqueezerMovementState.down:
                        if (MoveDown())
                        {
                            nextIdle = Time.time + idleDownTime;
                            state = SqueezerState.idle;
                        }
                        break;
                }
                break;
            case SqueezerState.idle:
                if (Time.time >= nextIdle)
                {
                    moveState = moveState == SqueezerMovementState.up ? SqueezerMovementState.down : SqueezerMovementState.up;
                    state = SqueezerState.move;
                }
                break;
        }
    }
    protected bool MoveDown()
    {
        Vector2 displacement =  direction * downSpeed * Time.fixedDeltaTime;
        if ((press.position + displacement).y > (endPoint + parent.position).y)
        {
            press.MovePosition(press.position + displacement);
            return false;
        }
        else
        {
            press.MovePosition(endPoint + parent.position);
            return true;
        }
    }
    protected bool MoveUp()
    {
        Vector2 displacement = -direction * upSeed * Time.fixedDeltaTime;
        if ((press.position + displacement).y < (startPoint + parent.position).y)
        {
            press.MovePosition(press.position + displacement);
            return false;
        }
        else
        {
            press.MovePosition(startPoint + parent.position);
            return true;
        }
    }
    public void OnGizmos(Vector3 _pos)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_pos + (Vector3)startPoint, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_pos + (Vector3)endPoint, 0.1f);
    }
}
public interface ISqueezerAnimation
{
    void SetSize(float _y);
}
[System.Serializable]
public class SqueezerAnimation : ISqueezerAnimation
{ 
    [SerializeField]
    protected SpriteRenderer stick;
    protected Transform press;
    protected Vector2 dinamicSize;
    public void Initialize(Transform _press)
    {
        press = _press;
        dinamicSize = stick.size;
    }
    public void SetSize(float _y)
    {
        dinamicSize.x = stick.size.x;
        dinamicSize.y = _y ;
        stick.size = dinamicSize;
    }
    public void OnLateUpdate()
    {
        SetSize(-press.localPosition.y);
    }
}
public interface ISqueezerDesign
{
    int Size { get ; set ; }
    void SetDesign(ScriptableSqueezerDesign _design);
    ScriptableSqueezerDesign GetDesign();
    void AppyDesign();
}
[System.Serializable]
public class SqueezerDesign : ISqueezerDesign
{
    [SerializeField]
    protected int size = 0;
    [SerializeField]
    protected SpriteRenderer beam, stick, press;
    [SerializeField]
    protected BoxCollider2D pressCollider, pressTrigger;
    [SerializeField]
    protected ScriptableSqueezerDesign design;

    public int Size { get => size; set => size = value; }
    public void SetDesign(ScriptableSqueezerDesign _design)
    {     
        design = _design;      
    }
    public ScriptableSqueezerDesign GetDesign()
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
        press.sprite = design.GetSize(size).press;
        pressCollider.size = design.GetSize(size).pressCollider;
        Vector2 _pressTrigger = design.GetSize(size).pressTrigger;
        _pressTrigger.x = design.GetSize(size).pressCollider.x - 0.1f;
        pressTrigger.size = _pressTrigger;
        pressTrigger.offset = design.GetSize(size).pressOffset;
    }
    public void OnGizmos(Vector2 _endPos)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_endPos, pressCollider.size);
    }
}