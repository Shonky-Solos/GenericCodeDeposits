using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float explosionDuration;
    public GameObject parent;
    private void Awake()
    {
        parent.GetComponent<GrenadeScript>().projectileSpeed = 0;
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDuration);
        Destroy(parent);
    }

}
