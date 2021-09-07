using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float projectileSpeed;
    public Animator anim;
    public GameObject explosion;

    private void Start()
    {
        StartCoroutine(DestroyTimer());
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(projectileSpeed * Time.deltaTime, 0, 0);
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("Explode");
        explosion.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Explode");
        explosion.SetActive(true);
    }
}
