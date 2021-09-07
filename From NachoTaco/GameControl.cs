using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
//using GooglePlayGames;
//using UnityEngine.iOS;


public class GameControl : MonoBehaviour {

    public static GameControl instance = null;

    public static float moveSpeed = -9f;

    [SerializeField]
    GameObject restartButton;

    [SerializeField]
    Text highScoreText;

    [SerializeField]
    Text yourScoreText;

    [SerializeField]
    public GameObject[] obstacles;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float spawnDelay;
    float nextSpawn;

    [SerializeField]
    float timeToBoost = 3f;
    float nextBoost;

    int highScore = 0, yourScore = 0;

    public static bool gameStopped;
    public Button deathText;
    public float speedIncrease = 0.25f;
    public int salsaCount;
    public PlayerHealth ph;
    public float currentMoveSpeed;
    public bool canSpawn;
    public float moveStop;
    public float oldMoveSpeed;
    //public Scroll scrollScript;
    public float handX;
    public float handY;
    public float tongX;
    public float tongY;
    public float tongToX;
    public float tongToY;
    public GameObject terry;
    public GameObject terrySquish;
    public GameObject hand;
    public GameObject handSoundHolder;
    public Scroll scrollScrip;
    AudioSource handSound;
    bool die = false;
    bool tong = false;
    public GameObject tongHit;
    public CharacterController ccScrip;
    public GameObject deathParticle;

    float nextScoreIncrease = 0f;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        restartButton.SetActive(false);
        canSpawn = true;
        yourScore = 0;
        moveSpeed = -9f;
        highScore = PlayerPrefs.GetInt("highScore");
        nextSpawn = Time.time + spawnDelay;
        nextBoost = Time.unscaledTime + timeToBoost;
        moveStop = 1;
        var spawnStop = ph.SpawnPause();
        StartCoroutine(spawnStop);
        oldMoveSpeed = moveSpeed;
        handSound = handSoundHolder.GetComponent<AudioSource>();
        handSound.volume = 200f;
        handSoundHolder.SetActive(false);
        die = false;
        tong = false;
        salsaCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStopped)
            IncreaseYourScore();

        highScoreText.text = "HighScore: " + highScore + "s";
        yourScoreText.text = "Score: " + yourScore + "s";

        if (Time.time > nextSpawn && canSpawn)
            SpawnObstacle();

        if (Time.unscaledTime > nextBoost && !gameStopped)
            BoostTime();

        salsaCount = ph.salsaCount;
        currentMoveSpeed = moveSpeed;

        if (salsaCount == 1)
        {
            StartCoroutine(HandLerp());
        }

        if (salsaCount != 2 && salsaCount != 3)
        {
            if (moveSpeed > -1 && moveSpeed != 0)
            {
                moveSpeed = -1;
            }
            //if (moveSpeed > -5 && moveSpeed!=0)
            //{
            //    speedIncrease = 4;
            //}
            //else
            //{
            //    speedIncrease = 1;
            //}
        }
        else if (salsaCount == 2)
        {
            moveSpeed = 0;
            moveStop = 0;
        }
        if (die)
        {
            die = !die;
            StartCoroutine(HandLerp2());
            StartCoroutine(CrunchSound());
        }
        if (tong)
        {
            tong = !tong;
            StartCoroutine(TongSnap());
            StartCoroutine(CrunchSound());
        }
        hand.transform.position = new Vector2(handX, handY);
        tongHit.transform.position = new Vector2(tongX, tongY);
    }


    public void PlayerHit()
    {
        ph.salsaCount = 2;
        if (yourScore > highScore)
            PlayerPrefs.SetInt("highScore", yourScore);
        moveSpeed = 0f;
        gameStopped = true;
        //restartButton.SetActive(true);
        deathText.gameObject.SetActive(true);
        die = true;
    }

    public void PlayerTong()
    {
        //ph.salsaCount = 2;
        if (yourScore > highScore)
            PlayerPrefs.SetInt("highScore", yourScore);
        moveSpeed = 0f;
        gameStopped = true;
        deathText.gameObject.SetActive(true);
        tong = true;
    }

    void SpawnObstacle()
    {
        spawnDelay = 2f;
        nextSpawn = Time.time + spawnDelay;
        int randomObstacle = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[randomObstacle], spawnPoint.position, Quaternion.identity);
    }

    void BoostTime()
    {
        nextBoost = Time.unscaledTime + timeToBoost;
        moveSpeed -= speedIncrease;
        //scrollScrip.increase += 0.25f;
    }

    void IncreaseYourScore()
    {
        if (Time.unscaledTime > nextScoreIncrease)
        {
            yourScore += 1;
            nextScoreIncrease = Time.unscaledTime + 1;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Backtomenu()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator HandLerp()
    {
        handX = Mathf.Lerp(handX, 0, 3.5f*Time.deltaTime);
        handY = Mathf.Lerp(handY, -4.12f, 3.5f * Time.deltaTime);
        yield return null;
    }

    public IEnumerator HandLerp2()
    {
        handX = Mathf.Lerp(handX, 2.11f, 2f * Time.deltaTime);
        handY = Mathf.Lerp(handY, -5.67f, 2f*Time.deltaTime);
        yield return new WaitForSeconds(1);
        StartCoroutine(DragAway());
        yield return null;
    }

    public IEnumerator TongSnap()
    {
        tongX = Mathf.Lerp(tongX, tongToX, 2f * Time.deltaTime);
        tongY = Mathf.Lerp(tongY, tongToY, 2f * Time.deltaTime);
        yield return new WaitForSeconds(1);
        ccScrip.tongAnim.SetTrigger("Tong");
        deathParticle.SetActive(true);
        terry.SetActive(false);
        StartCoroutine(TongDrag());
    }

    public IEnumerator TongDrag()
    {
        tongY = Mathf.Lerp(tongY, 20f, 3f * Time.deltaTime);
        yield return null;
    }

    public IEnumerator DragAway()
    {
        terry.SetActive(false);
        terrySquish.SetActive(true);
        handX = Mathf.Lerp(handX, -15, 3f * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        //handSound.Play();
        yield return null;
    }

    public IEnumerator CrunchSound()
    {
        yield return new WaitForSeconds(1.5f);
        handSoundHolder.SetActive(true);
        Debug.Log("MakeSound");
        yield return null;
    }
}
