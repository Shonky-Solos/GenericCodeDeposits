using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    public GameObject TacoCharacter;
    public GameObject TerryBody;
    public GameObject shellBody;
    public BoxCollider2D mainCol;
    public BoxCollider2D offFloorCol;
    SpriteRenderer shellCol;
    public GameControl gc;
    public Rigidbody2D rb;
    Animator anim;
    public Button deathText;
    public Button slideButton;
    public float jumpSpeed;
    public float slideDuration;
    public ParticleSystem slideDust;
    public ParticleSystem salsaSplat;
    AudioSource terrySound;
    public PlayerHealth ph;
    public bool shellArm;

    bool death;
    //Sets if the player can jump instead of spamming the button
    private bool shouldJump;
    private bool canJump;
    //Detects if slide button is held
    public bool isSliding;
    public GameObject jumpHolder;
    AudioSource jumpSound;
    public Animator tongAnim;
	
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = TacoCharacter.GetComponent<Animator>();
        mainCol = TacoCharacter.GetComponent<BoxCollider2D>();
        offFloorCol = TerryBody.GetComponent<BoxCollider2D>();
        shellCol = shellBody.GetComponent<SpriteRenderer>();
        offFloorCol.enabled = false;
        shellCol.enabled = false;
        terrySound = gameObject.GetComponent<AudioSource>();
        terrySound.volume = 200f;
        death = false;
        jumpSound = jumpHolder.GetComponent<AudioSource>();
        shellArm = false;
	}
	
	
	void Update ()
    {
        if (canJump)
        {
            canJump = false;
            shouldJump = true;
        }
        TacoCharacter.transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (isSliding)
        {
            TacoCharacter.transform.tag = "Sliding";
            slideDust.Play(true);
        }
        else
        {
            terrySound.Stop();
            slideDust.Stop(false);
        }

        if(TacoCharacter.tag == "Sliding")
        {
            slideDust.Play(true);
        }
        else
        { 
            slideDust.Stop(true);
        }

        if (death)
        {
            gc.PlayerTong();
        }

        shellArm = ph.shellArmour;
    }

    public void Jump()
    {
        if(TacoCharacter.transform.tag == "onFloor")
        {
            canJump = true;
            anim.SetTrigger("Jump");
            jumpSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (shouldJump)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            slideButton.gameObject.SetActive(false);
            shouldJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && !anim.GetBool("Sliding"))
        {
            TacoCharacter.transform.tag = "onFloor";
            anim.SetTrigger("Land");
            slideButton.gameObject.SetActive(true);
        }

        if (collision.collider.tag == "Instakill" && shellArm == false)
        {
            gc.tongHit = collision.gameObject;
            tongAnim = collision.gameObject.GetComponent<Animator>();
            gc.tongX = collision.gameObject.transform.position.x;
            gc.tongY = collision.gameObject.transform.position.y;
            death = true;
            gc.canSpawn = false;
        }
        else if (collision.collider.tag == "Instakill" && shellArm == true)
        {
            ph.shellArmour = false;
            ph.shellSprite.enabled = false;
            ph.normalSprite.enabled = true;
            ph.shellIndicator.SetActive(false);
            Destroy(collision.collider.gameObject);
            StartCoroutine(ph.SpawnPause());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TacoCharacter.transform.tag = "Jumping";
    }

    public void OnHoldSlide()
    {
        if(TacoCharacter.transform.tag == "onFloor")
        {
            isSliding = true;
            anim.SetBool("Sliding", true);
            offFloorCol.enabled = true;
            mainCol.enabled = false;
            terrySound.Play();
            slideDust.Play(true);
        }
    }

    public void OnLetGoSlide()
    {
        if (TacoCharacter.transform.tag != "Jumping" && isSliding)
        {
            isSliding = false;
            anim.SetBool("Sliding", false);
            TacoCharacter.transform.tag = "onFloor";
            Debug.Log("Stopped sliding");
            offFloorCol.enabled = false;
            mainCol.enabled = true;
            //rb.AddForce(Vector2.up * jumpSpeed / 3, ForceMode2D.Impulse);
            terrySound.Stop();
        }
    }

    public void OnPressSlide()
    {
        if(TacoCharacter.transform.tag != "Jumping")
        {
            terrySound.Play();
            slideDust.Play(true);
            Debug.Log("play sound");
        }
    }
}
