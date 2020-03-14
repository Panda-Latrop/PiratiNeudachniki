using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IFlip
{
    bool IsFlipped { get; }
   void Flip(bool _true);
}

[System.Serializable]
public class PirateFlip : IFlip
{
    protected bool isFlipped;
    [SerializeField]
    protected SpriteRenderer bodySpr, weaponSpr;
    [SerializeField]
    protected Transform weaponTr;
    protected Vector2 weaponPos, weaponFlipedPos;
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
            bodySpr.flipX = _true;
            weaponSpr.flipY = _true;
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
    public void Checker(float _angle)
    {
        if (_angle <= 90 && _angle >= -90 && isFlipped != false)
        {
            Flip(false);
        }
        if ((_angle > 90 || _angle < -90 ) && isFlipped != true)
        {
              Flip(true);
        }
    }
    public void Checker(Vector2 _direction)
    {
        if (_direction.x > 0.0f && isFlipped != false)
        {
            Flip(false);
        }
        if (_direction.x < 0.0f && isFlipped != true)
        {
            Flip(true);
        }
    }
}
