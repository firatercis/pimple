using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicForwardStack : MonoBehaviour
{

    //Must add on main player gameobject
    //Needs an event that's called whenever player is moved horizontally :: PlayerMove();

    //Settings
    [SerializeField]
    public float followDistanceZ; //Z distance between stack members
    public float stackSpeedCoef; // How fast the stack members will move to their new positions
    public float timerThreshold; // Time lag before stack members will start their movement to their new position
    // Connections
    [SerializeField]
    public GameObject stackStart;
    public List<GameObject> stackMembers;

    MoveForward moveForward; //:: Used For Example(Movement Script)
    // State Variables
    [SerializeField]
    float timeSpentUnmoving;
    Vector3 stackStarTailPosition;
    Vector3 memberTailPosition;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
        stackStart = gameObject;

        
        
        //     Example:
        moveForward = GetComponent<MoveForward>();
        moveForward.PlayerMove += ResetStackTimer;
    }
    void InitState(){
        timeSpentUnmoving = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        stackStarTailPosition = stackStart.transform.position;
        stackStarTailPosition.z += followDistanceZ;

        stackMembers[0].transform.position = Vector3.Lerp(stackMembers[0].transform.position, stackStarTailPosition,  timeSpentUnmoving * stackSpeedCoef);

        for(int i = 1; i < stackMembers.Count; i++)
        {
            memberTailPosition = stackMembers[i - 1].transform.position;
            memberTailPosition.z += followDistanceZ;

            stackMembers[i].transform.position = Vector3.Lerp(stackMembers[i].transform.position, memberTailPosition, timeSpentUnmoving * stackSpeedCoef);
        }

        timeSpentUnmoving += Time.deltaTime;
        if(timeSpentUnmoving >= 1)
        {
            timeSpentUnmoving = 1;
        }
    }
    
    void ResetStackTimer()
    {
        if(timeSpentUnmoving >= timerThreshold)
        timeSpentUnmoving = 0;
    }
}
