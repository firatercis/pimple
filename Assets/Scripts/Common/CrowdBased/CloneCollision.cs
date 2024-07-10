using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CloneCollision : MonoBehaviour
{
    //Settings
    public bool gateIsMultiplier;
    public int gateValue;
    // Connections
    public event Action<GameObject> DestroyClone;
    public event Action<bool,int> GatePassed;
    // State Variables
    
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy")){
            DestroyClone(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Collider>().enabled = false;
        GateManager gm = other.GetComponent<GateManager>();
        gateIsMultiplier = gm.isMultiplier;
        gateValue = gm.value;
        other.gameObject.SetActive(false);

        GatePassed(gateIsMultiplier, gateValue);
        
    }
}
