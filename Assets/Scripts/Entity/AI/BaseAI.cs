using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    protected bool isActive = true;
    public virtual bool IsActive {get => isActive; set => isActive = enabled = value;}
}