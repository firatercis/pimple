using SoftwareKingdom.Static;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
   
    // Connections
    public AudioClip inGameBackgroundMusic;
    public AudioClip startMenuMusic;
    public AudioClip nextCustomerMenuMusic;
    private AudioSource audioSource; // Will be taken from main cam
    private void Awake()
    {
        InitConnections();
    }
    void InitConnections()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

 
    public void InGameMusic()
    {
        StartBGMusicClip(inGameBackgroundMusic);
    }
   
    public void StartMenuMusic()
    {
        StartBGMusicClip(startMenuMusic);
    }

    public void NextCustomerMenuMusic()
    {
        StartBGMusicClip(nextCustomerMenuMusic);
    }

    void StartBGMusicClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }

}
