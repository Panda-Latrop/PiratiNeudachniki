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
public class PirateAnimation : AnimationController
{
    protected bool isRun;
    protected int yDir;
    protected IMovement movement;
    public void Initialize(IMovement _movement)
    {
        movement = _movement;
    }
    public override void LateUpdate()
    {
        Vector2 move;
        move.x = movement.GetConstant().x;
        move.y = movement.GetVelocity().y;
        if (move.x != 0 && !isRun)
        {
            animator.SetBool("Run", true);
            isRun = true;
        }
        if (move.x == 0 && isRun)
        {
            animator.SetBool("Run", false);
            isRun = false;
        }


        if ((move.y == 0 || movement.IsGrounded()) && yDir != 0)
        {
            yDir = 0;
            animator.SetInteger("YDir", yDir);
        }
        else
        {
            if (move.y > 0 && yDir != 1)
            {
                yDir = 1;
                animator.SetInteger("YDir", yDir);
            }
            else
            {
                if (move.y < 0 && yDir != -1)
                {
                    yDir = -1;
                    animator.SetInteger("YDir", yDir);
                }
            }
        }
    }
}