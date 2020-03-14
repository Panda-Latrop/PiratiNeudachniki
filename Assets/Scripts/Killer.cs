using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PirateEntity g;
        if ((g = collision.GetComponent<PirateEntity>()) != null)
        {
            g.Damageable.Hurt(new DamageInfo(9999.0f));
        }
    }
}
