using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Design", menuName = "Scriptable/Design/Squeezer")]
public class ScriptableSqueezerDesign : ScriptableObject
{
    [SerializeField]
    protected SqueezerDesignStruct[] sizes;
    public SqueezerDesignStruct GetSize(int _size)
    {
        return sizes[_size];
    }
    public int GetSizeCount()
    {
        return sizes.Length;
    }
}
[System.Serializable]
public struct SqueezerDesignStruct
{
    public Sprite beam, stick, press;
    public Vector2 stickSize, pressCollider, pressTrigger, pressOffset;
}