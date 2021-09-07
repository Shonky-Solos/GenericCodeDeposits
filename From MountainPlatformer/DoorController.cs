using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject exitPosition;
    public Vector2 exitPositionV;

    private void Update()
    {
        exitPositionV = exitPosition.transform.position;
    }
}
