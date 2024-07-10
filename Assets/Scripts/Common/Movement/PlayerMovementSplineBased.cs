using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public class PlayerMovementSplineBased : MonoBehaviour, SimpleUserTapInputListener
{
    // Settings
    public float speed = 10;
    public float roadBorder = 2;

    // Connections
    SplineFollower splineFollower;
    SimpleUserTapInput simpleUserTapInput;
    // State variables
    public bool canMove = true;
    public bool canSwerve = true;

    // Start is called before the first frame update

    void Awake()
    {
        InitConnections();
    }

    void InitConnections()
    {
        simpleUserTapInput = GetComponent<SimpleUserTapInput>();
        simpleUserTapInput.SetListener(this);
        splineFollower = GetComponent<SplineFollower>();
        splineFollower.followSpeed = speed;
    }

    void InitState()
    {
        // Ilk sahne acildiginda yapilmasini istedigimiz islemler
        canMove = true;
        canSwerve = true;
    }
    
    void Start()
    {
        InitState();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnDirectiveTap(Vector2 direction)
    {
        if (canSwerve)
        {
            direction.y = 0;
            if (Mathf.Abs(splineFollower.motion.offset.x + direction.x) <= roadBorder)
            {
                splineFollower.motion.offset += direction;
            }
                
        }
    }

    public void Disable()
    {
        canMove = false;
        canSwerve = false;
    }

    public void Enable()
    {
        canMove = true;
        canSwerve = true;
    }
}