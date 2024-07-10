using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PimpleParticleFX : MonoBehaviour
{
    //Settings
    public float minHealingEmission;
    public float maxHealingEmission;

    // Connections
    public GameObject openingFXParticle;
    public GameObject squeezingFXParticle;
    public GameObject healingFXParticle;
    private ParticleSystem healingFXParticleSystem;
    // State Variables


    // Start is called before the first frame update
    private void Awake()
    {
        InitConnections();
       
    }

    void Start()
    {
        InitState();
    }
    void InitConnections(){
        healingFXParticleSystem = healingFXParticle.GetComponent<ParticleSystem>();
    }
    void InitState(){
        healingFXParticleSystem.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpeningFX()
    {
        openingFXParticle.SetActive(true);
    }

    public void SetSqueezingParticle(bool isOn)
    {
        squeezingFXParticle.SetActive(isOn);

    }
    public void SetHealingParticle(bool isOn)
    {
        healingFXParticle.GetComponent<ParticleSystem>().enableEmission = isOn;
    }

    public void SetHealingPower(float percent)
    {
 
        float emission = minHealingEmission + (maxHealingEmission - minHealingEmission) * percent;
        ParticleSystem.EmissionModule emissionModule = healingFXParticleSystem.emission;
        //.rateOverTime = emission;
    }


}

