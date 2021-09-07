using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    public Transform[] backgrounds; // Lists all backgrounds to parallax
    private float[] parallaxScales; // Proportion of camera movement to move them
    public float smoothing = 1f; // How smooth it'll be (>0)

    private Transform cam; // Ref to main camera
    private Vector3 previousCamPos; // position of camera in the previous frame

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Start()
    {
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * 1;
        }
    }

    private void Update()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        previousCamPos = cam.position;
    }
}
