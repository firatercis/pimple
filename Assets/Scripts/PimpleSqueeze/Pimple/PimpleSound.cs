

using UnityEngine;

public class PimpleSound : MonoBehaviour
{
    public AudioClip[] squeezeSounds;
    public AudioClip healSound;
    public AudioClip splashSound;
    public AudioClip openSound;
    public float[] possibleDegrees;

    AudioSource audioSource;

    public float healingSoundMinPitchFactor = 0.5f;
    public float healingSoundMaxPitchFactor = 2.0f;

    float initialPitch;

    private void Awake()
    {
        InitConnections();
    }

    void InitConnections()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitState();        
    }

    void InitState()
    {
        initialPitch = audioSource.pitch;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySqueezeSound(int value)
    {
        //audioSource.pitch = initialPitch;
        audioSource.clip = squeezeSounds[0];
        audioSource.enabled = true;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StartHealSound()
    {
        //float randomPitchFactor = Random.Range(healingSoundMinPitchFactor, healingSoundMaxPitchFactor);
        //audioSource.pitch = randomPitchFactor * initialPitch;  
        audioSource.loop = false;
        audioSource.clip = healSound;
        audioSource.Play();
    }

    public void PlayOpenSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(openSound);
    }

    public void PlaySplashSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(splashSound);
    }
    


    public void SetDegree(float degree)
    {
        int soundIndex = -1;
        for(int i=0; i < possibleDegrees.Length; i++)
        {
            if(degree >= possibleDegrees[i])
            {
                soundIndex = i;
            }
        }
        audioSource.clip = squeezeSounds[soundIndex];
    }

    public void StopSound()
    {
      //  audioSource.pitch = initialPitch;
        audioSource.enabled = false;
    }
}
