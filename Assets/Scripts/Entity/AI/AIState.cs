using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    void OnStateEnter();
    void OnStateUpdate();
    void OnStateExit();
}

public abstract class AIState : IAIState
{
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}

[System.Serializable]
public class AIPirateFollow : AIState
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    [Range(0.5f, 5.0f)]
    protected float padding = 0.5f;
    [SerializeField]
    protected float frictionSpeed = 1.0f;
    protected float currentMove = 0.0f;
    protected IJumpAccel movement;

    public void Initialize(IJumpAccel _movement)
    {
        movement = _movement;
    }
    public float Padding { get => padding; set => padding = value; }
    public Transform Target { get => target; set => target = value; }

    public override void OnStateEnter()
    {
        return;
    }
    public override void OnStateUpdate()
    {
        if (movement.IsWalled())
        {
            movement.Jump(1.0f);
        }
        if (movement.GetRigidbody().position.x >= target.position.x - padding && movement.GetRigidbody().position.x <= target.position.x + padding)
        {
            if (currentMove != 0.0f)
            {
                if (currentMove > 0.0f)
                {
                    currentMove -= frictionSpeed * Time.deltaTime;
                    if (currentMove <= 0.0f)
                    {
                        currentMove = 0.0f;
                    }
                }
                else
                {
                    currentMove += frictionSpeed * Time.deltaTime;
                    if (currentMove >= 0.0f)
                    {
                        currentMove = 0.0f;
                    }
                }
            }
            movement.Move(currentMove);
        }
        else
        {
            if (movement.GetRigidbody().position.x < target.position.x)
            {
                currentMove = 1.0f;
                movement.Move(currentMove);
            }
            else
            {
                currentMove = -1.0f;
                movement.Move(currentMove);
            }
        }

    }
    public override void OnStateExit()
    {
        return;
    }
}