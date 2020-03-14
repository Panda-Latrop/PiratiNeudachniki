using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    string PoolTag { get; }
    void Push();
    void OnPush();
    void OnPop();
    GameObject GetGameObject();
    Transform GetTransform();
    void SetPosition(Vector3 _position);
    void SetRotation(Quaternion _rotation);
    void SetParent(Transform _parent);
}