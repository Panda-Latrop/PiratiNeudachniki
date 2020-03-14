using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSqueezer : BaseTrap
{
    public override void Hide()
    {
        IsActive = false;
    }
    public override void Unhide()
    {
        IsActive = true;
    }
    #region Mono
    protected void Start()
    {
        
    }
    #endregion
}

[System.Serializable]
public class SqueezerMovement
{
    public enum SqueezerMovementState
    {
        up, down
    }
    public enum SqueezerState
    {
        move, idle
    }

    [SerializeField]
    protected Vector2 startPoint, endPoint;
    [SerializeField]
    protected float moveUpTime, moveDownTime, idleUpTime, idleDownTime;
    [SerializeField]
    protected SqueezerState state;
    [SerializeField]
    protected SqueezerMovementState moveState;
    [SerializeField]
    protected Rigidbody2D press;
    protected Rigidbody2D parent;
    protected float distance, upSeed, downSpeed, nextIdle;
    protected Vector2 direction;

    public void Initialize(Rigidbody2D _parent)
    {
        parent = _parent;
        direction = endPoint - startPoint;
        distance = direction.magnitude;
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
        if (press.position.LessEqual(endPoint + parent.position))
        {
            press.MovePosition(press.position + direction * downSpeed * Time.fixedDeltaTime);
            return false;
        }
        else
        {
            return true;
        }
    }
    protected bool MoveUp()
    {
        if (press.position.MoreEqual(startPoint + parent.position))
        {
            press.MovePosition(press.position + direction * upSeed * Time.fixedDeltaTime);
            return false;
        }
        else
        {
            return true;
        }
    }
}
