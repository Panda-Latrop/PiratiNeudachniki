using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum PoolPopInfo
{
    failure = 0,
    done = 1,
    force = 2
}
internal class UnityPoolManager : DestroyableSingleton<UnityPoolManager>
{
#pragma warning disable 0649 
    [SerializeField]
    protected PoolObject[] poolsInfo;
#pragma warning restore 0649
    protected readonly Dictionary<string, PoolManager> pools = new Dictionary<string, PoolManager>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < poolsInfo.Length; i++)
            AddPool(poolsInfo[i]);
    }
    internal bool AddPool(PoolObject _poolObject)
    {
        if (_poolObject != null && !pools.ContainsKey(_poolObject.PoolTag))
        {
            GameObject prefab = _poolObject.GetGameObject();;
            Transform root = CreateRoot(prefab.name).transform;
            PoolManager pool = new PoolManager(prefab, root);
            pools.Add(_poolObject.PoolTag, pool);
            return true;
        }
        Debug.LogWarning("Can't Add To Pool");
        return false;
    }
    internal bool AddPool(GameObject _object)
    {
        IPoolObject poolObject;
        poolObject = _object.GetComponent<IPoolObject>();
        if (poolObject != null && !pools.ContainsKey(poolObject.PoolTag))
        {
            GameObject prefab = _object;
            Transform root = CreateRoot(prefab.name).transform;
            PoolManager pool = new PoolManager(prefab, root);
            pools.Add(poolObject.PoolTag, pool);
            return true;
        }
        Debug.LogWarning("Can't Add To Pool");
        return false;
    }
    internal bool AddPool(IPoolObject _poolObject, GameObject _prefab)
    {
        if (_poolObject != null && !pools.ContainsKey(_poolObject.PoolTag))
        {
            Transform root = CreateRoot(_prefab.name).transform;
            PoolManager pool = new PoolManager(_prefab, root);
            pools.Add(_poolObject.PoolTag, pool);
            return true;
        }
        Debug.LogWarning("Can't Add To Pool");
        return false;
    }
    internal bool ContainPool(string _tag)
    {
        return pools.ContainsKey(_tag);
    }
    internal void Push(IPoolObject _poolObject)
    {
        if (pools.ContainsKey(_poolObject.PoolTag))
        {
            _poolObject.SetPosition(pools[_poolObject.PoolTag].Root.position);
            _poolObject.SetRotation(Quaternion.identity);
            _poolObject.SetParent(pools[_poolObject.PoolTag].Root);
            pools[_poolObject.PoolTag].Push(_poolObject);
        }
    }
    internal IPoolObject Pop(string _poolType)
    {
        if (pools.ContainsKey(_poolType))
        {
            IPoolObject result = pools[_poolType].Pop() ?? ForcePop(pools[_poolType].Prefab, pools[_poolType].Root);
            return result;
        }
        return null;
    }
    internal IPoolObject Pop(string _poolType, out PoolPopInfo _info)
    {
        if (pools.ContainsKey(_poolType))
        {
            IPoolObject result = pools[_poolType].Pop();
            _info = PoolPopInfo.done;
            if (result == null)
            {
                result = ForcePop(pools[_poolType].Prefab, pools[_poolType].Root);
                _info = (PoolPopInfo.done|PoolPopInfo.force);
            }          
            return result;
        }
        _info = PoolPopInfo.failure;
        return null;
    }
    private IPoolObject ForcePop(GameObject _prefab, Transform _parent)
    {
        GameObject go = Instantiate(_prefab);
        go.transform.parent = _parent;
        IPoolObject result = go.GetComponent<IPoolObject>();
        result.OnPop();
        return result;
    }
    private GameObject CreateRoot(string _rootName)
    {
        GameObject root = new GameObject();
        root.transform.parent = transform;
        root.name = _rootName;
        return root;
    }
}

internal class PoolManager
{
    protected readonly Queue<IPoolObject> objects;
    protected GameObject prefab;
    protected Transform root ;
    public GameObject Prefab { get => prefab; }
    public Transform Root { get => root; }

    public PoolManager(GameObject _poolObjectInfo, Transform _root)
    {
        prefab = _poolObjectInfo;
        root = _root;
        objects = new Queue<IPoolObject>();
    }
    public void Push(IPoolObject _value)
    {
        _value.OnPush();
        objects.Enqueue(_value);
    }
    public IPoolObject Pop()
    {
        IPoolObject result = default;
        if (objects.Count > 0)
        {
            result = objects.Dequeue();
            result.OnPop();
        }
        return result;
    }
}