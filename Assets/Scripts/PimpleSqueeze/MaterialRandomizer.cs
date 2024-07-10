using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class MaterialAddress
{
    public Renderer renderer;
    public int index;
}

[Serializable]
public class MaterialGroup
{
    public string groupName;
    public MaterialAddress[] addressesToBeChanged;


    public Material[] materials;

    public void SetRandomMaterial()
    {
        int randomIndex = Random.Range(0, materials.Length);
        for(int i=0;i<addressesToBeChanged.Length; i++)
        {
            Renderer rendererToBeChanged = addressesToBeChanged[i].renderer;
            int materialIndex = addressesToBeChanged[i].index;
            Material[] temp = rendererToBeChanged.materials;
            temp[materialIndex] = materials[randomIndex];
            rendererToBeChanged.materials = temp;
        }

    }

}



public class MaterialRandomizer : MonoBehaviour
{
    //Settings

    // Connections
  
    public MaterialGroup[] materialGroups;
    
    // State Variables dafasdf
    
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


    public void Randomize()
    {
        for(int i=0; i< materialGroups.Length; i++)
        {
            materialGroups[i].SetRandomMaterial();
        }
    }
}

