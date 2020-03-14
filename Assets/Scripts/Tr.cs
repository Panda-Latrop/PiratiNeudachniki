using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tr : MonoBehaviour
{
    public float f,f2;
    public Transform a;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PirateEntity g;
        if ((g = collision.GetComponent<PirateEntity>()) != null)
        {
            Vector2 v = (a.rotation * Vector2.right).normalized;
            v.x*= f;
            v.y*= f2;
            v.y -= g.Movement.GetForce().y;
            g.Movement.AddForce( v);
        }  
    }
}
