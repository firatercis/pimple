using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{

    // Settings
    
    // Connections
    public AudioClip enterSound;
    public AudioClip positiveSound;
    public AudioClip negativeSound;
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
        audioSource = GetComponent<AudioSource>();
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterButton()
    {
        audioSource.PlayOneShot(enterSound);
    }

    public void OnPositiveButton()
    {
        audioSource.PlayOneShot(positiveSound);
    }

}
