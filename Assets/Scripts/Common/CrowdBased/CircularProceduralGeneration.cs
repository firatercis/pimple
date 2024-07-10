using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircularProceduralGeneration : MonoBehaviour
{

    
    //Settings
    [SerializeField]
    float radius;
    [SerializeField]
    float radiusIncrease;
    [SerializeField]
    float angle;
    float angleIncrease;
    
    // Connections
    public List<GameObject> clones;

    public GameObject cloneMain;
    // State Variables
    bool changeRadius;
    Vector3 nextSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();

    }
    void InitConnections(){
        
    }
    void InitState(){
        nextSpawnPosition.y = 0;
        angleIncrease = 7.5f;

        nextSpawnPosition.x = transform.position.x + radius * Mathf.Cos(angle);
        nextSpawnPosition.z = transform.position.z + radius * Mathf.Sin(angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            AddOneClone();
        }

        for(int i = 0; i < clones.Count; i++)
        {
            if(clones[i].GetComponent<MoveTowardsOrigin>().offsetVector.magnitude >= radius)
            {
                if(clones[i].GetComponent<MoveTowardsOrigin>().offsetVector.magnitude >= radius * 2)
                {
                    
                }
                clones[i].GetComponent<MoveTowardsOrigin>().isMovingToOrigin = true;
                clones[i].GetComponent<MoveTowardsOrigin>().timeSinceInstantiate = 0;
                
                changeRadius = true;
            }
        }
    }

    void AddOneClone()
    {
       
        GameObject cloneGO = Instantiate(cloneMain, nextSpawnPosition, Quaternion.identity);
        cloneGO.transform.parent = transform;
        clones.Add(cloneGO);

        if (changeRadius)
        {
            radius += radiusIncrease;
            changeRadius = false;
        }
        
        angle += angleIncrease;

        nextSpawnPosition.x = transform.position.x + radius * Mathf.Cos(angle);
        nextSpawnPosition.z = transform.position.z + radius * Mathf.Sin(angle);

        Debug.Log("CircleProcedural: AddOneClone");
        
    }
}
