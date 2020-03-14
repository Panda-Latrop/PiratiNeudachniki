using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletLifeCycle
{
    public enum LifeCycleState
    {
       life,
       kick,
       death,
    }

    protected bool isActive = true;
    [SerializeField]
    protected float timeToDeactive = 1.0f, timeToPush = 1.0f;
    protected float nextTime = 0.0f;
    protected bool toPush = false;

    public bool IsActive { get => isActive; set => isActive = value; }

    public void Restart()
    {
        nextTime = Time.time + timeToDeactive;
        toPush = false;
        isActive = true;
    }
    public LifeCycleState UpdateCycle(bool _toPush)
    {
        //Debug.Log(Time.time - nextTime);
        if (!_toPush && !toPush)
        {
            toPush = true;
            nextTime = Time.time + timeToPush;
        }      
        if(Time.time >= nextTime)
        {
            if (toPush)
            {
                isActive = false;
                return LifeCycleState.death;
            }
            else
            {
                toPush = true;
                nextTime = Time.time + timeToPush;
                return LifeCycleState.kick;
            }
            
        }
        return LifeCycleState.life;
    }
}
