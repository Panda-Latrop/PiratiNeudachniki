using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlipChecker
{
    public static void Check(IFlip _flip, Vector2 _direction)
    {
        if (_direction.x > 0.0f && _flip.IsFlipped != false)
        {
            _flip.Flip(false);
        }
        if (_direction.x < 0.0f && _flip.IsFlipped != true)
        {
            _flip.Flip(true);
        }
    }
    public static void Check(IFlip _flip, float _angle)
    {
        if (_angle <= 90 && _angle >= -90 && _flip.IsFlipped != false)
        {
            _flip.Flip(false);
        }
        if ((_angle > 90 || _angle < -90) && _flip.IsFlipped != true)
        {
            _flip.Flip(true);
        }
    }
}

public interface IFlip
{
    bool IsFlipped { get; }
   void Flip(bool _true);
}
[System.Serializable]
public class BodyFlip : IFlip
{
    protected bool isFlipped;
    [SerializeField]
    protected SpriteRenderer body;

    public bool IsFlipped => isFlipped;
    public void Initialize(SpriteRenderer _body)
    {
        body = _body;
    }
    public void Flip(bool _true)
    {
        if (isFlipped != _true)
        {
            isFlipped = _true;
            body.flipX = _true;
        }
    }
}
    [System.Serializable]
public class PirateFlip : IFlip
{
    protected bool isFlipped;
    [SerializeField]
    protected Transform weaponTr;
    protected Vector2 weaponPos, weaponFlipedPos;
    [SerializeField]
    protected SpriteRenderer body,arm;

    public bool IsFlipped => isFlipped;
    public void Initialize()
    {
        weaponFlipedPos = weaponPos = weaponTr.localPosition;
        weaponFlipedPos.x *= -1;
    }
    public void Flip(bool _true)
    {
        if (isFlipped != _true)
        {
            isFlipped = _true;
            body.flipX = _true;
            arm.flipY = _true;
            if (_true)
            {
                weaponTr.localPosition = weaponFlipedPos;
            }
            else
            {
                weaponTr.localPosition = weaponPos;
            }
        }
    }
}
