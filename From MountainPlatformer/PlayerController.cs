using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Player's rigidbody
    private Rigidbody2D rb;
    public Transform firePointRight;
    public Transform firePointLeft;
    public GameObject fireball;
    public GameObject bombPotion;
    public float fireCooldown;
    private bool canFire;
    private bool firePowerup;
    public GameObject infoObject;
    public GameObject deathPanel;
    public bool lookingRight;
    private WorldInfo worldInfo;
    private bool canChangeMusic;
    // Variables for player movement speed
    public float speed;
    public float ladderSpeed;
    public float jumpForce;
    // Just for maths for movement
    private float moveInput;
    private Vector2 ladderInput;

    // Managest hitpoints, powerup etc
    private int health;
    private bool hasBomb;

    // This one determines if you're in the ladder hitbox, allowing for ladder shenanigans
    private bool inLadder;
    // This one is how you do the proper controls.                   
    private bool onLadder;

    // Gets and handles player anim
    public GameObject FullHealthSprites;
    private Animator FHAnim;
    public GameObject LowHealthSprites;
    private Animator LHAnim;
    public GameObject fireSprites;
    private Animator FireAnim;
    public GameObject bombIcon;

    //This number detects if all gems are collected; resets at start of map.
    private int gems;

    // Ground checks and all that crap
    private bool grounded;
    public Transform grounder;
    public LayerMask groundLayer;
    private bool canJump;
    // Checks for sublevel
    public bool inSubLevel;
    // Stuff for interacting with doors
    private bool inDoor;
    private GameObject tempDoor;
    public bool hasKey;

    // For reloading the scene coz it complained in editor.
    private int levelNumber;
    #endregion

    //Basically defining things, making sure health and stuff is correct
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gems = 0;
        canChangeMusic = true;
        FHAnim = FullHealthSprites.GetComponent<Animator>();
        LHAnim = LowHealthSprites.GetComponent<Animator>();
        FireAnim = fireSprites.GetComponent<Animator>();
        health = 2;
        inSubLevel = false;
        worldInfo = infoObject.GetComponent<WorldInfo>();
        canFire = true;
        firePowerup = false;
        lookingRight = true;
        hasKey = false;
        PlayerPrefs.SetInt("LevelPosition", levelNumber);
    }

    void Update()
    {
        //Debug.Log(canJump);

        levelNumber = SceneManager.GetActiveScene().buildIndex;

        if(gems == 4)
        {
            PlayerPrefs.SetInt(levelNumber.ToString(), 1);
            StartCoroutine(AllGems());
        }

        if(health == 2)
        {
            LowHealthSprites.GetComponent<SpriteRenderer>().enabled = false;
            if (firePowerup)
            {
                fireSprites.GetComponent<SpriteRenderer>().enabled = true;
                FullHealthSprites.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                FullHealthSprites.GetComponent<SpriteRenderer>().enabled = true;
                fireSprites.GetComponent<SpriteRenderer>().enabled = false;
            }
            
        }
        if (health == 1)
        {
            LowHealthSprites.GetComponent<SpriteRenderer>().enabled = true;
            FullHealthSprites.GetComponent<SpriteRenderer>().enabled = false;
            fireSprites.GetComponent<SpriteRenderer>().enabled = false;
        }

        if(health <= 0)
        {
            Destroy(rb);
            StartCoroutine(DeathAnim());
        }

        if (hasBomb)
        {
            bombIcon.SetActive(true);
        }
        else if (!hasBomb)
        {
            bombIcon.SetActive(false);
        }

        // Handles movement when not on a ladder
        moveInput = Input.GetAxis("Horizontal");
        if (!onLadder)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            rb.gravityScale = 1;
        }

        // Jump if grounded (or on ladder lol)
        if (Input.GetButtonDown("Jump") && canJump || Input.GetButtonDown("Jump") && onLadder)
        {
            canJump = false;
            onLadder = false;
            GetComponent<PlayerAudio>().jumped = true;
            FHAnim.SetTrigger("Jump");
            LHAnim.SetTrigger("Jump");
            FireAnim.SetTrigger("Jump");
            if(rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }

        if (Input.GetButtonDown("Fire1") && firePowerup || Input.GetKeyDown(KeyCode.RightShift) && firePowerup)
        {
            if (canFire && !hasBomb)
            {
                Shoot(fireball);
            }

        }

        if(hasBomb && Input.GetButtonDown("Fire1"))
        {
            Shoot(bombPotion);
        }

        #region Grounded Check & Jump ability (For Coyote Time)
        grounded = Physics2D.OverlapCircle(grounder.transform.position, 0.1f, groundLayer);
        if (grounded)
        {
            canJump = true;
        }
        if (!grounded)
        {
            StartCoroutine(CoyoteTimer());
        }
        #endregion

        #region Anim stuff
        if(rb.velocity.x != 0)
        {
            FHAnim.SetBool("Moving", true);
            LHAnim.SetBool("Moving", true);
            FireAnim.SetBool("Moving", true);
        }
        if(rb.velocity.x == 0)
        {
            FHAnim.SetBool("Moving", false);
            LHAnim.SetBool("Moving", false);
            FireAnim.SetBool("Moving", false);
        }
        if(rb.velocity.y < -0.1)
        {
            FHAnim.SetBool("Falling", true);
            LHAnim.SetBool("Falling", true);
            FireAnim.SetBool("Falling", true);
        }
        if(rb.velocity.y >= -0.1)
        {
            FHAnim.SetBool("Falling", false);
            LHAnim.SetBool("Falling", false);
            FireAnim.SetBool("Falling", false);
        }
        if(rb.velocity.x < 0)
        {
            lookingRight = false;
            FullHealthSprites.GetComponent<SpriteRenderer>().flipX = true;
            LowHealthSprites.GetComponent<SpriteRenderer>().flipX = true;
            fireSprites.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(rb.velocity.x > 0)
        {
            lookingRight = true;
            FullHealthSprites.GetComponent<SpriteRenderer>().flipX = false;
            LowHealthSprites.GetComponent<SpriteRenderer>().flipX = false;
            fireSprites.GetComponent<SpriteRenderer>().flipX = false;
        }
        #endregion

        // Handles movement when, duh, on a ladder
        ladderInput = new Vector2(Input.GetAxis("Horizontal") * ladderSpeed, Input.GetAxis("Vertical") * ladderSpeed);

        // When the player is on a ladder block, they can press UP to actually start climbing
        if (inLadder)
        {
            if(Input.GetAxis("Vertical") != 0)
            {
                onLadder = true;
            }
        }
        // Physics/control replacement while climbing.
        if (onLadder && rb != null)
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            gameObject.transform.Translate(ladderInput * Time.deltaTime);
        }

        if (inDoor)
        {
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (tempDoor.gameObject.name != "Exit")
                {
                    StartCoroutine(DoorMove());
                }
                else
                {
                    StartCoroutine(FinishLevel());
                }
            }
        }

        if (inSubLevel && canChangeMusic)
        {
            Debug.Log("Changed to Sub");
            worldInfo.PlaySubMusic();
            canChangeMusic = false;
        }
        if(!inSubLevel && canChangeMusic)
        {
            Debug.Log("Changed to Main");
            worldInfo.PlayMainMusic();
            canChangeMusic = false;
        }
    }

    public IEnumerator CoyoteTimer()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = false;
    }

    public void Stomp()
    {
        onLadder = false;
        FHAnim.SetTrigger("Jump");
        LHAnim.SetTrigger("Jump");
        FireAnim.SetTrigger("Jump");
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void TakeDamage(int dmg)
    {
        firePowerup = false;
        if (health > 0)
        {
            GetComponent<PlayerAudio>().tookDamage = true;
        }
        FHAnim.SetTrigger("Damaged");
        LHAnim.SetTrigger("Damaged");
        FireAnim.SetTrigger("Damaged");
        health -= dmg;
    }

    public void Shoot(GameObject projectile)
    {
        FireAnim.SetTrigger("Shoot");
        if (lookingRight)
        {
            Instantiate(projectile, firePointRight.transform.position, firePointRight.transform.rotation);
        }
        else
        {
            Instantiate(projectile, firePointLeft.transform.position, firePointLeft.transform.rotation);
        }
        StartCoroutine(ShootCooldown());
    }

    public IEnumerator ShootCooldown()
    {
        canFire = false;
        if (hasBomb)
        {
            hasBomb = false;
        }
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    public IEnumerator DeathAnim()
    {
        deathPanel.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelNumber);
    }

    public IEnumerator AllGems()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<AudioSource>().PlayOneShot(GetComponent<PlayerAudio>().allCoins);
    }

    IEnumerator DoorMove()
    {
        deathPanel.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        deathPanel.GetComponent<Animator>().SetTrigger("FadeOut");
        inSubLevel = !inSubLevel;
        canChangeMusic = true;
        gameObject.transform.position = tempDoor.GetComponent<DoorController>().exitPositionV;
    }
    public IEnumerator FinishLevel()
    {
        deathPanel.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MapScreen");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
            TakeDamage(1);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Stomp();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("FirePotion"))
        {
            health = 2;
            firePowerup = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Sign"))
        {
            StartCoroutine(DisplayText(collision.GetComponent<SignScript>().textObject));
        }
        if(collision.gameObject.CompareTag("BombPotion"))
        {
            hasBomb = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name.Contains("Gem"))
        {
            gems += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ladder"))
        {
            inLadder = true;
        }
        if(other.gameObject.CompareTag("Door"))
        {
            inDoor = true;
            tempDoor = other.gameObject;
        }
    }

    private IEnumerator DisplayText(GameObject text)
    {
        text.SetActive(true);
        yield return new WaitForSeconds(3);
        text.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladder"))
        {
            inLadder = false;
            onLadder = false;
        }
        if(collision.gameObject.CompareTag("Door"))
        {
            inDoor = false;
        }
    }
}
