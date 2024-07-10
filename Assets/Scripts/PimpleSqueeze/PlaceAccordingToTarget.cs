using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceAccordingToTarget : MonoBehaviour
{
    //Settings

    // Connections
    public Transform defaultTarget;
    public Transform localMileStone = null;
    // State Variables

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaceOver();
        }
    }*/

    public void PlaceOver(Transform target, Transform localMileStone=null)
    {

        if (localMileStone == null) 
            localMileStone = this.localMileStone;
        transform.LookAt(transform.position + target.forward); // TODO: local milestone forward must be same as the transform target        
        Vector3 globalOffset = target.position - transform.TransformPoint(localMileStone.localPosition);
        transform.position = transform.position + globalOffset;
    }
    
    public void PlaceOver()
    {
        PlaceOver(defaultTarget, localMileStone);
    }
}

