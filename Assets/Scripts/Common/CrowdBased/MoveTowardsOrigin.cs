using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveTowardsOrigin : MonoBehaviour
{
    //Settings
    public float timeSinceInstantiate;
    public float speed;
    // Connections
   
    // State Variables
    public bool isMovingToOrigin = true;
    public Vector3 offsetVector;
    Vector3 offsetNormalized;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
        timeSinceInstantiate = 0;
    }
    void InitConnections(){
        
    }
    void InitState(){
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offsetVector = transform.parent.position - transform.position;
        transform.Translate(offsetVector * speed * Time.deltaTime);
    }

    
}
