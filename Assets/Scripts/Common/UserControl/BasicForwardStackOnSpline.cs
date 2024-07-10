using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;


public class BasicForwardStackOnSpline : MonoBehaviour
{
    //Settings
    [SerializeField]    
    public float stackSpeedCoef; //How fast stack aligns with the previous member
    // Connections
    [SerializeField]

    public List<SplineFollower> followers;
    public SplineFollower follower;
    // State Variables
    [SerializeField]
    public float timeSpentUnmoving;
    Vector2 offsetVector;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
       
        follower = GetComponent<SplineFollower>();
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        offsetVector = follower.motion.offset - followers[0].motion.offset;
        followers[0].motion.offset += offsetVector * stackSpeedCoef * Time.deltaTime;

        for(int i = 1; i < followers.Count; i++)
        {
            offsetVector = followers[i - 1].motion.offset - followers[i].motion.offset;
            followers[i].motion.offset += offsetVector * stackSpeedCoef * Time.deltaTime; 
        }
    }
}
