using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolObject : MonoBehaviour, IPoolObject
{
    [SerializeField]
    protected string poolTag;
    [SerializeField]
    protected Transform transformSelf;
    [SerializeField]
    protected GameObject gameObjectSelf;

    public Transform GetTransform()
    {
        return transformSelf;
    }
    public GameObject GetGameObject()
    {
        return gameObjectSelf;
    }
    #region Pool
    public string PoolTag => poolTag;
    public virtual void OnPop()
    {
        gameObjectSelf.SetActive(true);
    }
    public virtual void OnPush()
    {
        gameObjectSelf.SetActive(false);
    }
    public void Push()
    {
        UnityPoolManager.Instance.Push(this);
    }
    public void SetParent(Transform _parent)
    {
        transformSelf.SetParent(_parent);
    }
    public void SetPosition(Vector3 _position)
    {
        transformSelf.position = _position;
    }
    public void SetRotation(Quaternion _rotation)
    {
        transformSelf.rotation = _rotation;
    }
    #endregion
}