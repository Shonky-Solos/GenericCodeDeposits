using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer spriteRen;
    public Sprite leftSnake;
    public Sprite rightSnake;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRen = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x <= gameObject.transform.position.x)
        {
            spriteRen.sprite = leftSnake;
        }
        else
        {
            spriteRen.sprite = rightSnake;
        }
    }
}
