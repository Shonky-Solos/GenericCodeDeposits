using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    private AudioSource source;
    public AudioClip mainTheme;
    public AudioClip subTheme;
    public int worldLevelPosition;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //Tells the game which location to send the player on the world map post game
    private void Update()
    {
        PlayerPrefs.SetInt("LevelPosition", worldLevelPosition);
    }

    //these two are called in PlayerController when you go through a door
    public void PlayMainMusic()
    {
        source.Stop();
        source.clip = mainTheme;
        source.Play();
    }

    public void PlaySubMusic()
    {
        source.Stop();
        source.clip = subTheme;
        source.Play();
    }

}
