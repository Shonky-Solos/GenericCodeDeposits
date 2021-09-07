using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockKeyScript : MonoBehaviour
{
    private GameObject doorCollision;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        doorCollision = GetComponentInChildren<DoorController>().gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        doorCollision.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().hasKey)
        {
            doorCollision.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
