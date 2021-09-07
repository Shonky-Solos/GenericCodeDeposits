using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public GameObject lCheck;
    private bool hitLeft;
    private bool hitLeft2;
    public GameObject rCheck;
    private bool hitRight;
    private bool hitRight2;
    private int direction;
    public float speed;
    private bool canTurn;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        direction = -1;
        canTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        hitLeft = Physics2D.OverlapCircle(lCheck.transform.position, 0.1f, groundLayer);
        hitLeft2 = Physics2D.OverlapCircle(lCheck.transform.position, 0.01f, playerLayer);
        hitRight = Physics2D.OverlapCircle(rCheck.transform.position, 0.1f, groundLayer);
        hitRight2 = Physics2D.OverlapCircle(rCheck.transform.position, 0.01f, playerLayer);

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (hitLeft && canTurn || hitLeft2 && canTurn)
        {
            StartCoroutine(TurnCooldown());
            direction = 1;
        }
        if (hitRight && canTurn || hitRight2 && canTurn)
        {
            StartCoroutine(TurnCooldown());
            direction = -1;
        }
    }

    private IEnumerator TurnCooldown()
    {
        canTurn = false;
        sprite.flipX = !sprite.flipX;
        yield return new WaitForSeconds(0.2f);
        canTurn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Grenade")
        {
            Destroy(gameObject);
        }
    }
}
