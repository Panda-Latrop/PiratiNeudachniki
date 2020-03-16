using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField]
    protected bool isActive = true;
    public bool IsActive { get => isActive; set => isActive = value; }
}
