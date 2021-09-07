using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour {

    [SerializeField]
    float moveSpeed = -5f;

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime * -GameControl.moveSpeed/3,
            transform.position.y, transform.position.z);

        if (transform.position.x < -13f)
            Destroy(gameObject);

	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Player"))
            GameControl.instance.PlayerHit();
    }
}
