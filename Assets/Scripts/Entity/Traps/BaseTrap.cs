using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITrap
{
    bool IsActive { get; set; }
    Vector2 GetPosition();
    void SetPosition(Vector2 _position);
    float GetRotation();
    void SetRotation(float _angle);
    void Hide();
    void Unhide();
}

public abstract class BaseTrap : MonoBehaviour, ITrap
{

    protected bool isActive;
    [SerializeField]
    protected Rigidbody2D rig;

    public virtual bool IsActive { get => isActive; set { isActive = enabled = value; } }
    public virtual Vector2 GetPosition()
    {
       return rig.position;
    }
    public virtual void SetPosition(Vector2 _position)
    {
        rig.position = _position;
    }
    public virtual float GetRotation()
    {
        return rig.rotation;
    }
    public virtual void SetRotation(float _angle)
    {
        rig.rotation = _angle;
    }
    public abstract void Hide();
    public abstract void Unhide();

}
