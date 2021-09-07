using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float fallPos;
    public float endPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //Following the player around the map
        if(player.transform.position.y > fallPos && player.transform.position.x < endPos)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
        
        //Stops scrolling downwards when at bottom limit of camera
        if(player.transform.position.y <= fallPos && player.transform.position.x < endPos)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, fallPos, -10);
            if(player.transform.position.y <= (fallPos - 6f))
            {
                player.GetComponent<PlayerController>().TakeDamage(10);
            }
        }

        //Stops scrolling right if reaching the end of the level
        if(player.transform.position.x >= endPos && player.transform.position.y > fallPos)
        {
            gameObject.transform.position = new Vector3(endPos, player.transform.position.y, -10);
        }

        //Call for both conditions
        if(player.transform.position.x >= endPos && player.transform.position.y <= fallPos)
        {
            gameObject.transform.position = new Vector3(endPos, fallPos, -10);
        }
    }
}
