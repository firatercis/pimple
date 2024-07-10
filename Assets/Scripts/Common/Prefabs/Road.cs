using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public float roadLength = 1f;
    public float roadWidth = 1f;
    public float xTileSize = 2f; //Bir birimin X ekseninde kaç tane tile var?
    public float yTileSize = 2f; //Bir birimin Y ekseninde kaç tane tile var?
    public bool setTilingOnStart = false;

    public void SetMaterialTiling()
    {
        GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(transform.localScale.x / xTileSize / roadWidth, transform.localScale.z / yTileSize / roadLength);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (setTilingOnStart)
        {
            SetMaterialTiling();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
