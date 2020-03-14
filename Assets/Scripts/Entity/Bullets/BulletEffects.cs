using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleAction
{
    play,
    stop,
    clear
}
[System.Serializable]
public class BulletEffects
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected ParticleSystem trail, impact;

    public void SetTrailAction(ParticleAction _action)
    {
        switch (_action)
        {
            case ParticleAction.play:
                trail.Play(true);
                break;
            case ParticleAction.stop:
                trail.Stop(true);
                break;
            case ParticleAction.clear:
                trail.Stop(true);
                trail.Clear(true);
                break;
            default:
                break;
        }
    }
    public void SetImpactAction(ParticleAction _action)
    {
        switch (_action)
        {
            case ParticleAction.play:
                impact.Play(true);
                break;
            case ParticleAction.stop:
                impact.Stop(true);
                break;
            case ParticleAction.clear:
                impact.Stop(true);
                impact.Clear(true);
                break;
            default:
                break;
        }
    }
    public void SetSpriteActive(bool _active)
    {
        spriteRenderer.enabled = _active;
    }
}