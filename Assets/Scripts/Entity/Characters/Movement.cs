using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRigidbody2DBaseHandler
{
    IRigidbody2DBase Rigidbody2DBase { get; }
}
public interface IMovementHandler
{
    IMovement Movement { get; }
}
public interface IJumpAccelHandler
{
    IJumpAccel Movement { get; }
}
public interface IRigidbody2DBase
{
    Rigidbody2D GetRigidbody();
    Vector2 GetVelocity();
    void SetVelocity(Vector2 _velocity);

}
public interface IRigidbody2DController : IRigidbody2DBase
{
    Vector2 GetConstant();
    void SetConstant(Vector2 _constant);
    Vector2 GetForce();
    void AddForce(Vector2 _force);
    bool IsGrounded();
    bool IsWalled();
}
public interface IMovement : IRigidbody2DController
{
    bool IsCanMove { get; set; }
    float MoveSpeed { get; set; }
    float JumpSpeed { get; set; }
    void Move(float _direction);
    void Jump(float _direction);
    void StopMovement();
}
public interface IJumpAccel : IMovement
{
    void Jump(float _direction,bool _block);
}
[System.Serializable]
public class SimpleMovement : IRigidbody2DBase
{
    [SerializeField]
    protected Rigidbody2D rigidbody2d;
    #region Properties
    public Rigidbody2D GetRigidbody()
    {
        return rigidbody2d;
    }
    public Vector2 GetVelocity()
    {
        return rigidbody2d.velocity;
    }
    public void SetVelocity(Vector2 _velocity)
    {
        rigidbody2d.velocity = _velocity;
    }
    #endregion
}
public class Rigidbody2DController : IRigidbody2DController
{
    [SerializeField]
    protected Rigidbody2D rigidbody2d;
    protected Vector2 velocity, constant, force;
    [SerializeField]
    protected float resistance = 1;
    protected bool isGrounded = false, isWalled;
    [SerializeField]
    protected ContactFilter2D contactFilter;
    public RaycastHit2D[] contactPoints = new RaycastHit2D[4];
    protected int contactCount;
    protected const float shellRadius = 0.1f;
    //public ContactPoint2D[] contactPoint = new ContactPoint2D[4];
    //protected const float minMoveDistance = 0.001f;

    #region Properties
    public Rigidbody2D GetRigidbody()
    {
        return rigidbody2d;
    }
    public Vector2 GetVelocity()
    {
        return velocity;
    }
    public void SetVelocity(Vector2 _velocity)
    {
        constant = _velocity;
        force = Vector2.zero;
    }
    public Vector2 GetConstant()
    {
        return constant;
    }
    public void SetConstant(Vector2 _constant)
    {
        constant = _constant;
    }
    public Vector2 GetForce()
    {
        return force;
    }
    public void AddForce(Vector2 _force)
    {
        force += _force;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsWalled()
    {
        return isWalled;
    }
    public RaycastHit2D[] GetContactPoints()
    {
        return contactPoints;
    }
    public int GetContactCount()
    {
        return contactCount;
    }
    #endregion
    protected void Friction()
    {
        if (force.x != 0.0f)
        {
            if (force.x < 0.0f)
            {
                force.x -= rigidbody2d.sharedMaterial.friction * Physics2D.gravity.y * rigidbody2d.gravityScale * Time.fixedDeltaTime;
                if (force.x >= 0f)
                    force.x = 0.0f;
            }
            else
            {
                force.x += rigidbody2d.sharedMaterial.friction * Physics2D.gravity.y * rigidbody2d.gravityScale * Time.fixedDeltaTime;
                if (force.x <= 0f)
                    force.x = 0.0f;
            }
        }
    }
    protected void ResistensMove()
    {
        if (force.x != 0.0f)
        {
            if (force.x < 0.0f && constant.x > 0.0f)
            {
                force.x += constant.x * resistance;
                if (force.x >= 0f)
                    force.x = 0.0f;
            }
            else
            {
                if (force.x > 0.0f && constant.x < 0.0f)
                {
                    force.x += constant.x * resistance;
                    if (force.x <= 0f)
                        force.x = 0.0f;
                }
            }
        }
    }
    protected void CheckDetection(ref Vector2 _displacement)
    {
        bool isGroundedAlready = false, isWalledAlready = false;
        Vector2 dir = new Vector2(0.707f, 0.707f) * shellRadius;
        if (_displacement.x < 0)
            dir.x *= -1.0f;
        if (_displacement.y <= 0)
            dir.y *= -1.0f;
        contactCount = rigidbody2d.Cast(dir, contactFilter, contactPoints, shellRadius);
        //count = rigidbody2d.GetContacts(contactFilter, contactPoint);
        if (contactCount == 0)
        {
            isGrounded = isWalled = false;
            return;
        }
        for (int i = 0; i < contactCount; i++)
        {
            Vector2 currentNormal = contactPoints[i].normal;
            if (currentNormal.y >= 0.5f)
            {
                isGroundedAlready = true;
                if (_displacement.y < 0.0f)
                    _displacement.y = force.y = 0.0f;
            }
            else
            {
                if (currentNormal.y <= -0.5f)
                {
                    if (_displacement.y > 0.0f)
                        _displacement.y = force.y = 0.0f;
                }
            }
            if (currentNormal.x >= 1.0f)
            {
                isWalledAlready = true;
                if (_displacement.x < 0.0f)
                    _displacement.x = force.x = 0.0f;
            }
            else
            {
                if (currentNormal.x <= -1.0f)
                {
                    isWalledAlready = true;
                    if (_displacement.x > 0.0f)
                        _displacement.x = force.x = 0.0f;
                }
            }
        }
        isGrounded = isGroundedAlready;
        isWalled = isWalledAlready;
    }


    protected virtual Vector2 DisplacementCalculator()
    {
        velocity = constant + force;
        return velocity * Time.fixedDeltaTime;
    }
    protected virtual void AfterMoveHandler()
    {
        if (isGrounded)
            Friction();
        else
            force.y += Physics2D.gravity.y * rigidbody2d.gravityScale * Time.fixedDeltaTime;
        if (constant.x != 0.0f)
            ResistensMove();
    }

    public void FixedUpdate()
    {
        Vector2 displacement = DisplacementCalculator();
        CheckDetection(ref displacement);
        rigidbody2d.MovePosition(rigidbody2d.position + displacement);
        AfterMoveHandler();
    }
}
[System.Serializable]
public class ForceMovement : Rigidbody2DController , IMovement
{
    protected bool isCanMove = true;
    [SerializeField]
    protected float moveSpeed, jumpSpeed;

    public bool IsCanMove { get => isCanMove; set => isCanMove = value; }

    public virtual float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public virtual float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public virtual void Move(float _direction)
    {
        if (isCanMove)
        {
            constant.x = _direction * moveSpeed;
        }
    }
    public virtual void Jump(float _direction)
    {
        if (isCanMove)
        {
            if (isGrounded && force.y == 0.0f && _direction > 0.0f)
            {
                force.y += _direction * jumpSpeed;
            }
            if (!isGrounded && _direction < 0.0f)
            {
                force.y += _direction * jumpSpeed;
            }
        }
    }
    public void StopMovement()
    {
        force = constant = Vector2.zero;
    }
}

//public class MovementWithSave : ForceMovement
//{
//    protected float moveSpeedSave, jumpSpeedSave;
//    public void Initialize()
//    {
//        moveSpeedSave = moveSpeed;
//        jumpSpeedSave = jumpSpeed;
//    }
//    public override float MoveSpeed { get => moveSpeed; set => moveSpeed = moveSpeedSave + value; }
//    public override float JumpSpeed { get => jumpSpeed; set => jumpSpeed = jumpSpeedSave + value; }
//}

[System.Serializable]
public class JumpAccelMovement : ForceMovement , IJumpAccel
{
    protected bool jumpAccel;
    [SerializeField]
    protected float jumpAccelFactor;

    public override void Move(float _direction)
    {
        if (isCanMove)
        {

            if (jumpAccel)
                constant.x = _direction * moveSpeed * jumpAccelFactor;
            else
                constant.x = _direction * moveSpeed;
        }
    }
    public override void Jump(float _direction)
    {
        if (isCanMove)
        {
            if (isGrounded && force.y == 0.0f && _direction > 0.0f)
            {
                force.y += _direction * jumpSpeed;
                jumpAccel = true;
            }
            if (!isGrounded && _direction < 0.0f)
            {
                force.y += _direction * jumpSpeed;
                jumpAccel = false;
            }
        }
    }
    public void Jump(float _direction,bool _block)
    {
        if (isCanMove)
        {
            if (isGrounded && force.y == 0.0f && _direction > 0.0f)
            {
                force.y += _direction * jumpSpeed;            
            }
            if (!isGrounded && _direction < 0.0f)
            {
                force.y += _direction * jumpSpeed;
            }
        }
    }
    protected override void AfterMoveHandler()
    {
        if (isGrounded)
        {
            Friction();
            jumpAccel = false;
        }
        else
            force.y += Physics2D.gravity.y * rigidbody2d.gravityScale * Time.fixedDeltaTime;
        if (constant.x != 0.0f)
            ResistensMove();
        //else
        //    jumpAccel = false;
    }
}