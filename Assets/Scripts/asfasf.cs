using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asfasf : MonoBehaviour
{
    public Rigidbody2D rig;
    public Vector2 f;
    [ContextMenu("Jump")]
    public void Jump()
    {
        rig.velocity = f;
    }
    public void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                rig.velocity = f;
            }
        }
            
    }
}
