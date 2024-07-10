using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoissonDiscTest : MonoBehaviour
{
    //Settings
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    List<Vector2> points;
    // Connections
    
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
        
    }

    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        //for(int i = 0; i < points.Count; i++)
        //{
        //    //points[i] += new Vector2(transform.position.x, transform.position.y);
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionSize/2, regionSize);
        if(points!= null)
        {
            foreach(Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
