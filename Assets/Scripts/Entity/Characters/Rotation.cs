using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotationHandler
{
    IRotation Rotation { get; }
}

public interface IRotation
{
    bool IsCanRotation { get; set; }
    float Angle { get; }
    Quaternion Quaternion { get; }
    void SetRotation(float _angle);
    void SetRotation(Quaternion _quaternion);
    void SetRotation(float _angle, Quaternion _quaternion);
}
[System.Serializable]
public class PirateRotation : IRotation
{
    [SerializeField]
    protected bool isCanRotation = true;
    [SerializeField]
    protected Transform weapon;
    protected float angle;
    protected Quaternion quaternion,defaultQ = Quaternion.identity, defaultQFlipped = new Quaternion(0.0f,0.0f,1.0f, 0.0f);
    public bool IsCanRotation { get => isCanRotation; set => isCanRotation = value; }
    public float Angle => angle;
    public Quaternion Quaternion => quaternion;
    public void SetRotation(float _angle)
    {
        if (isCanRotation)
        {
            angle = _angle;
            quaternion = Quaternion.Euler(0.0f, 0.0f, _angle);
            weapon.rotation = quaternion;
        }
    }
    public void SetRotation(Quaternion _quaternion)
    {
        if (isCanRotation)
        {
            angle = _quaternion.eulerAngles.z;
            quaternion = _quaternion;
            weapon.rotation = quaternion;
        }
    }
    public void SetRotation(float _angle, Quaternion _quaternion)
    {
        if (isCanRotation)
        {
            angle = _angle;
            quaternion = _quaternion;
            weapon.rotation = quaternion;
        }
    }
    public void SetDefaultRotation(bool _fliped)
    {
        if (!_fliped)
        {
            weapon.rotation = defaultQ;
        }
        else
        {
            weapon.rotation = defaultQFlipped;
        }
    }
}
