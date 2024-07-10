using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlSound : MonoBehaviour
{

    // Settings

    // Connections
    public AudioClip[] goodSounds;
    public AudioClip[] badSounds;
    public AudioClip[] customerSuccessSounds;
    AudioSource audioSource;
    // State variables
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        audioSource= GetComponent<AudioSource>();
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomGoodSound()
    {
        PlayRandomSoundFrom(goodSounds);
    }

    public void PlayRandomBadSound()
    {
        PlayRandomSoundFrom(badSounds);
    }

    public void PlaySuccessSound()
    {

        PlayRandomSoundFrom(customerSuccessSounds);
    }

    private void PlayRandomSoundFrom(AudioClip[] audioSet)
    {
            int randomSoundIndex = Random.Range(0, audioSet.Length);
            AudioClip clipOfSound = audioSet[randomSoundIndex]; // KN: Sesin birden fazla kez acilabilmesi.
            audioSource.PlayOneShot(clipOfSound);
    }
}
