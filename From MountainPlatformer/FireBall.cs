using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float projectileSpeed;

    private void Start()
    {
        StartCoroutine(DestroyTimer());
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(projectileSpeed*Time.deltaTime, 0, 0);
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Snake")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
