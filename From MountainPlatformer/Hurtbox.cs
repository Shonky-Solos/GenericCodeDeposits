using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private PlayerController pc;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            pc.TakeDamage(1);
        }
    }
}
