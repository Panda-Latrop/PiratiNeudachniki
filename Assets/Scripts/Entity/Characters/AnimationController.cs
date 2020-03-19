using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAnimationController
{
    Animator Animator { get; }
}
public abstract class AnimationController : IAnimationController
{
    [SerializeField]
    protected Animator animator;
    public Animator Animator => animator;
    public abstract void LateUpdate();
}

[System.Serializable]
public class MoveAnimation : AnimationController
{
    protected bool isRun;
    protected IMovement movement;
    public void Initialize(IMovement _movement)
    {
        movement = _movement;
    }
    public override void LateUpdate()
    {
        float move;
        move = movement.GetVelocity().x;
        if (move != 0 && !isRun)
        {
            animator.SetBool("Run", true);
            isRun = true;
        }
        if (move == 0 && isRun)
        {
            animator.SetBool("Run", false);
            isRun = false;
        }       
    }
}

    [System.Serializable]
public class MoveJumpAnimation : MoveAnimation
{
    protected int yDir;
    public override void LateUpdate()
    {

        base.LateUpdate();
        float moveY = movement.GetVelocity().y;

        if ((moveY == 0 || movement.IsGrounded()) && yDir != 0)
        {
            yDir = 0;
            animator.SetInteger("YDir", yDir);
        }
        else
        {
            if (moveY > 0 && yDir != 1)
            {
                yDir = 1;
                animator.SetInteger("YDir", yDir);
            }
            else
            {
                if (moveY < 0 && yDir != -1)
                {
                    yDir = -1;
                    animator.SetInteger("YDir", yDir);
                }
            }
        }
    }
}