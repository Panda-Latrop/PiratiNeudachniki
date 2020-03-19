using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolParticle : IPoolObject
{
    void Play();
    void Stop();
    void Clear();
}
public class PoolParticle : PoolObject, IPoolParticle
{
    [SerializeField]
    protected ParticleSystem particle;
    public void Play()
    {
        particle.Play(true);
    }
    public void Stop()
    {
        particle.Stop(true);
    }
    public void Clear()
    {
        particle.Stop(true);
        particle.Clear(true);
    }
}
