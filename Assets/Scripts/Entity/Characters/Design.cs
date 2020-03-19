using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterDesign
{
    
    void SetEnable(bool _enable);
}

[System.Serializable]
public class MobBodyDesign : ICharacterDesign
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected SpriteRenderer bodyS;
    [SerializeField]
    protected Rigidbody2D rigidbody;

    public void SetEnable(bool _enable)
    {
        animator.enabled = bodyS.enabled = rigidbody.simulated = _enable;
        animator.
    }
    public void SetBodyFlipX(bool _flip)
    {
        bodyS.flipX = _flip;
    }
}
[System.Serializable]
public class PirateDesign : ICharacterDesign
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected SpriteRenderer bodyS, armS;
    [SerializeField]
    protected Rigidbody2D rigidbody;

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
