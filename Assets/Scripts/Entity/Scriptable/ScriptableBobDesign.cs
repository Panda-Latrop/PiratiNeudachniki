using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Design", menuName = "Scriptable/Design/Bob")]
public class ScriptableBobDesign : ScriptableObject
{
    [SerializeField]
    protected BobDesignStruct[] sizes;
    public BobDesignStruct GetSize(int _size)
    {
        return sizes[_size];
    }
    public int GetSizeCount()
    {
        return sizes.Length;
    }
}
[System.Serializable]
public struct BobDesignStruct
{
    public Sprite beam, stick, sphere;
    public Vector2 stickSize;
    public float sphereTrigger;
}
