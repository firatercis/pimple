using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPoissonTest : MonoBehaviour
{
    //Settings
    [SerializeField]
    float cloneRaidus = 1;
    public Vector2 regionSize;
    float regionDensityCap;
    int rejectionSamples = 10;
    // Connections
    List<GameObject> clones;
    List<Vector2> points;
    // State Variables
    float regionDensity;
    
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddOneClone();
        }
    }

    void AddOneClone()
    {
        points = PoissonDiscSampling.GeneratePoints(cloneRaidus, regionSize, rejectionSamples);
        
    }
}
