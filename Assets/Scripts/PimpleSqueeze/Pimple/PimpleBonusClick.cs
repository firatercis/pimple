using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleBonusClick : MonoBehaviour
{

    // Settings
    public int targetLayerIndex;
    public float maxRayDistance = 100.0f;
    // Connections
    Collider clickCollider;
    public GameObject testSphere;
    public event Action OnColliderClicked;
    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){

        clickCollider = GetComponent<Collider>();
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        CheckClickRay();
    }

    public void CheckClickRay()
    {

        if (Input.GetMouseButtonDown(0))
        {
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            testSphere.SetActive(false);
        }
       
    }

    void OnMouseDown()
    {
  
       
        OnColliderClicked?.Invoke();
        
    }

    private void OnMouseUp()
    {
        
    }

    public void SetColliderEnabled(bool isEnabled)
    {
        clickCollider.enabled = isEnabled;
        
    }

}
