using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    protected bool isActive = true;
    [SerializeField]
    protected JoistickController left, right;
    [SerializeField]
    protected Vector2 sensitiveLeft;
    [SerializeField]
    protected GroupMaster group;

    protected void LateUpdate()
    {
        if (isActive)
        {
            if (left.IsActive)
            {
                Vector2 vector = left.Vector;
                if (vector.x <= -sensitiveLeft.x || vector.x >= sensitiveLeft.x)
                {
                    if (vector.x > 0.0f)
                        group.MoveGroup(1.0f,0);
                    else
                        group.MoveGroup(-1.0f,0);
                }
                if (vector.y <= -sensitiveLeft.y || vector.y >= sensitiveLeft.y)
                {
                    if (vector.y > 0.0f)
                        group.JumpGroup(1.0f);
                    else
                        group.JumpGroup(-1.0f);
                }
            }
            else
            {
                group.MoveGroup(0.0f,0);
            }
            if (right.IsActive)
            {
                group.RotationGroup(right.Angle, right.Quaternion);
                group.ShootGroup();
            }
            else
            {
                group.StopRotationGroup();
            }
        }

    }

}