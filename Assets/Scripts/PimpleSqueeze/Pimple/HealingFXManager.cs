using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealingFXManager : MonoBehaviour
{
    //Settings
    public float minValue;
    public float maxValue;
    public float minParticleEmission;
    public float maxParticleEmission;
    // Connections
    public ParticleSystem healingParticles;
    // State Variables dafasdf

    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetValue(float value)
    {
        float percent = (value - minValue) / (maxValue - minValue);
        float emission = minParticleEmission + (maxParticleEmission - minParticleEmission) * percent;
        healingParticles.emissionRate = emission;
    }
}

