using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPirateDesign
{
    void SetEnable(bool _enable);
    void SetArmSprite(Sprite _sprite);
    void SetBodyFlipX(bool _flip);
    void SetArmFlipY(bool _flip);
}
[System.Serializable]
public class PirateDesign : IPirateDesign
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected SpriteRenderer bodyS, armS;
    [SerializeField]
    protected Rigidbody2D rigidbody;
    [SerializeField]
    protected CapsuleCollider2D collider;

    public void SetEnable(bool _enable)
    {
        animator.enabled = bodyS.enabled = armS.enabled = rigidbody.simulated = _enable;
    }
    public void SetArmSprite(Sprite _sprite)
    {
        armS.sprite = _sprite;
    }
    public void SetBodySprite(Sprite _sprite)
    {
        armS.sprite = _sprite;
    }
    public void SetBodyFlipX(bool _flip)
    {
        bodyS.flipX = _flip;
    }
    public void SetArmFlipY(bool _flip)
    {
        armS.flipY = _flip;
    }
}
