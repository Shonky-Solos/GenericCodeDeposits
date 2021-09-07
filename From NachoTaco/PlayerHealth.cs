using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerHealth : MonoBehaviour {

    public float playerCurrentSpeed;
    public float worldSpeed;
    public GameControl gameControlScript;
    public CharacterController cc;
    public ParticleSystem salsaSplat;
    public ParticleSystem guacSplat;
    public GameObject shellBody;
    public GameObject normalBody;
    public GameObject shellIndicator;
    public bool shellArmour;
    public string shellArmourName;
    public string cleanName;
    // Counts how many salsas the player has touched in a while
    public int salsaCount;
    public bool crunch;
    public GameObject terryRig;
    public SpriteRenderer normalSprite;
    public SpriteRenderer shellSprite;
    GameObject[] spawns;
    GameObject temp;
    public GameObject toSwitch;
    public GameObject powerup;

	void Start ()
    {
        worldSpeed = GameControl.moveSpeed;
        playerCurrentSpeed = worldSpeed;
        shellArmour = false;
        shellIndicator.SetActive(false);
        salsaCount = 0;
        crunch = false;
        normalSprite = normalBody.GetComponent<SpriteRenderer>();
        shellSprite = shellBody.GetComponent<SpriteRenderer>();
        spawns = gameControlScript.obstacles;
        temp = spawns[10];
        Debug.Log(temp);
	}

	void Update ()
    {
		if(salsaCount != 0 && salsaCount != 2)
        {
            
        }
        if(salsaCount == 2)
        {
            gameControlScript.PlayerHit();
            gameControlScript.canSpawn = false;
            crunch = true;
            Destroy(terryRig.GetComponent<Animator>());
        }
        if (crunch)
        {
            Debug.Log(crunch);
            StartCoroutine(gameControlScript.CrunchSound());
            crunch = false;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == shellArmourName)
        {
            shellArmour = true;
            cc.shellArm = true;
            shellSprite.enabled = true;
            normalSprite.enabled = false; 
            shellIndicator.SetActive(true);
            collision.gameObject.SetActive(false);
            spawns[10] = toSwitch;
        }
        if (!shellArmour && collision.gameObject.name != shellArmourName)
        {
            if(salsaCount != 2)
            {
                salsaCount++;
                Destroy(collision.gameObject);
                StartCoroutine(SpawnPause());
            }
        }
        if (shellArmour && collision.gameObject.name != shellArmourName)
        {
            spawns[10] = powerup;
            shellArmour = false;
            cc.shellArm = false;
            shellSprite.enabled = false;
            normalSprite.enabled = true;
            shellIndicator.SetActive(false);
            StartCoroutine(SpawnPause());
        }

        if(collision.gameObject.name == "Mild Salsa(Clone)" || collision.gameObject.name == "Medium Salsa(Clone)" || collision.gameObject.name == "Hot Salsa(Clone)")
        {
            salsaSplat.gameObject.SetActive(true);
        }
        else if(collision.gameObject.name == "LeGuac")
        {
            guacSplat.gameObject.SetActive(true);
        }
    }

    public IEnumerator SpawnPause()
    {
        gameControlScript.canSpawn = false;
        yield return new WaitForSeconds(3.5f);
        gameControlScript.canSpawn = true;
        salsaSplat.gameObject.SetActive(false);
        guacSplat.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == cleanName)
        {
            if(salsaCount != 0)
            {
                salsaCount = 0;
            }
            Destroy(collision.collider.gameObject);
        }
    }
}
