using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfsf : MonoBehaviour
{
    public PirateEntity pir;
    public Camera cam;
    public Transform tar;

    protected void Update()
    {
        Vector3 w = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 t = tar.position;
        w.z = 0.0f;
        t.z = 0.0f;
        Vector3 d = w - t;
        float a = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            pir.Movement.Move(Input.GetAxisRaw("Horizontal"));
            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    pir.Movement.Jump(1);
                }
                //else
                //{
                //    pir.Movement.Jump(-1);
                //}
            }
            pir.Rotation.SetRotation(a);
        if (Input.GetButton("Fire1"))
            pir.Shooter.Shoot();
    }
}
