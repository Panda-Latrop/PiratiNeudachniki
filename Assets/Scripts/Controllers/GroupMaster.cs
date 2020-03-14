using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PirateParameter
{
    public float speedSpread;
    public float jumpSpread;
    public float timeToShootSpread;
    public float paddingSpread;
}
public interface IGroupMaster
{
    void MoveGroup(float _direction, int _id);
    void MoveGroup(float _direction);
    void JumpGroup(float _direction, int _id);
    void JumpGroup(float _direction);
    void RotationGroup(float _angle, Quaternion _quaternion, int _id);
    void RotationGroup(float _angle, Quaternion _quaternion);
    void StopRotationGroup(int _id);
    void StopRotationGroup();
    void ShootGroup(int _id);
    void ShootGroup();
    void UpdateGroup(int _id);
    void UpdateGroup();
}
public class GroupMaster : MonoBehaviour, IGroupMaster
{

    [SerializeField]
    protected string pirateTag;
    [SerializeField]
    protected ScriptableWeapon weapon;
    [SerializeField]
    protected PirateParameter parameter;
    [Range(1,10)]
    [SerializeField]
    protected int pirateCount = 1;
    protected List<IPoolPirate> pirates;
    protected int currentCount;


    public void SpawnGroup(Vector2 _position)
    {
        IPoolPirate pirate = null;
        if (currentCount == 0)
        {
            if (SpawnUnit(_position, ref pirate))
                pirate.SetAIActive(false);
            else
                return;
        }
        for (int i = currentCount; i <= pirateCount; i++)
        {
            if (SpawnUnit(_position, ref pirate))
            {
                pirate.SetAIActive(true);
                pirate.SetTarget(pirates[i - 1].GetTransform());
            }
            else
                return;
        }
    }

    public bool SpawnUnit(Vector2 _position, ref IPoolPirate _pirate)
    {
        if(currentCount <= pirateCount)
        {
            PoolPopInfo ppi;
            _pirate = (UnityPoolManager.Instance.Pop(pirateTag, out ppi) as IPoolPirate);

            if(ppi >= PoolPopInfo.force)
            {
                _pirate.SetGroupMaster(this);
            }     

            _pirate.SetPosition(_position);
            _pirate.AddJumpSpeed(0.0f, parameter.jumpSpread);
            _pirate.SetWeapon(weapon);
            _pirate.AddTimeToShoot(0.0f,parameter.timeToShootSpread);
            _pirate.AddPadding(0.0f, parameter.paddingSpread);
            pirates.Add(_pirate);
            currentCount++;
            return true;
        }
        return false;
    }
    public void MoveGroup(float _direction, int _id)
    {
        if (currentCount != 0)
            pirates[_id].SetMove(_direction);
    }
    public void MoveGroup(float _direction)
    {
        for (int i = 0; i < currentCount; i++)
        {
            pirates[i].SetMove(_direction);
        }
    }
    public void JumpGroup(float _direction, int _id)
    {
        if (currentCount != 0)
            pirates[_id].SetJump(_direction);
    }
    public void JumpGroup(float _direction)
    {
        for (int i = 0; i < currentCount; i++)
        {
            pirates[i].SetJump(_direction);
        }
    }
    public void RotationGroup(float _angle, Quaternion _quaternion, int _id)
    {
        if (currentCount != 0)
        {
            pirates[_id].SetCanRotation(true);
            pirates[_id].SetRotation(_angle, _quaternion);
        }
    }
    public void RotationGroup(float _angle, Quaternion _quaternion)
    {
        for (int i = 0; i < currentCount; i++)
        {
            pirates[i].SetCanRotation(true);
            pirates[i].SetRotation(_angle, _quaternion);
        }
    }
    public void StopRotationGroup(int _id)
    {
        if (currentCount != 0)
        {
            pirates[_id].SetCanRotation(false);
        }
    }
    public void StopRotationGroup()
    {
        for (int i = 0; i < currentCount; i++)
        {
            pirates[i].SetCanRotation(false);
        }
    }
    public void ShootGroup(int _id)
    {
        if (currentCount != 0)
        {
            pirates[_id].Shoot();
        }
    }
    public void ShootGroup()
    {
        for (int i = 0; i < currentCount; i++)
        {
            pirates[i].Shoot();
        }
    }
    public void UpdateGroup(int _id)
    {
        if (!pirates[_id].IsAlive)
        {
            pirates.RemoveAt(_id);
            if (_id == 0)
            {
                pirates[_id].SetAIActive(false);
            }
            else
            {               
                pirates[_id].SetTarget(pirates[_id - 1].GetTransform());
            }
            currentCount--;
        }
    }
    public void UpdateGroup()
    {
        for (int i = 0; i < currentCount; i++)
        {
            if (!pirates[i].IsAlive)
            {
                pirates.RemoveAt(i);
                if (i == 0)
                {
                    pirates[i].SetAIActive(false);
                }
                else
                {
                    pirates[i].SetTarget(pirates[i - 1].GetTransform());
                }
                currentCount--;
                //i--;
            }
        }
    }


    #region Mono
    protected void Awake()
    {
        pirates = new List<IPoolPirate>(pirateCount);
        currentCount = 0;
    }
    protected void Start()
    {
        for (int i = 0; i < pirateCount; i++)
        {
            Spaweed();
        }
    }
    #endregion



    [ContextMenu("Spawn")]
    public void Spaweed()      
    {
        SpawnGroup(Vector2.zero);
    }

}
