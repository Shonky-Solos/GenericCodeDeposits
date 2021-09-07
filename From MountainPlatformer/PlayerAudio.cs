using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource playerAudio;
    private PlayerController pc;
    public AudioClip jump;
    public AudioClip damage;
    public AudioClip powerup;
    public AudioClip getCoin;
    public AudioClip allCoins;
    public bool tookDamage;
    public bool jumped;
    public bool gotCoin;

    private void Start()
    {
        jumped = false;
        tookDamage = false;
        playerAudio = gameObject.GetComponent<AudioSource>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(jumped)
        {
            playerAudio.PlayOneShot(jump);
            jumped = false;
        }
        if (tookDamage)
        {
            playerAudio.PlayOneShot(damage);
            tookDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Gem"))
        {
            playerAudio.PlayOneShot(getCoin);
        }
        if(collision.gameObject.CompareTag("FirePotion"))
        {
            playerAudio.PlayOneShot(powerup);
        }
    }
}
