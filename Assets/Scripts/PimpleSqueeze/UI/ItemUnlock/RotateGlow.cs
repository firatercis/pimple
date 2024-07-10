using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGlow : MonoBehaviour
{

    // Settings
    public float rotateSpeed = 180;
    // Connections

    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    void ApplyGlow()
    {
        gameObject.SetActive(true);
    }
}
