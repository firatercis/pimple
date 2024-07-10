using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MoveForward : MonoBehaviour
{
    //Settings
    [SerializeField]
    public float speed;
    public bool stackStart;
    // Connections
    public event Action PlayerMove;
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
        if (stackStart)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                PlayerMove();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                PlayerMove();
            }
        }
        

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
