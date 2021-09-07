using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompControl : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = GetComponentInParent<PlayerController>().gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Player.GetComponent<PlayerController>().Stomp();
            Destroy(collision.gameObject);
        }
    }
}
