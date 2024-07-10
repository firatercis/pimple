using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PimpleBlackHead : MonoBehaviour
{

    // Settings
    public float incomeMultiplier = 2.0f;
    public int blackHeadInitialBlendKeyIndex = 0;
    public float blackHeadInitialBlendValue = 52;
    // Connections
    public SkinnedMeshRenderer pimpleOuterRenderer;
    public Material blackHeadAdditiveMat;
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
        
    }

    public void SetBlackHeadInitial()
    {
        // Set material
        List<Material> materialsTemp = pimpleOuterRenderer.materials.ToList();
        materialsTemp.Add(blackHeadAdditiveMat);
        pimpleOuterRenderer.materials = materialsTemp.ToArray();
      //  pimpleOuterRenderer.SetBlendShapeWeight(blackHeadInitialBlendKeyIndex, blackHeadInitialBlendValue);
    }

    public void DestroyBlackHead()
    {
        List<Material> materialsTemp = pimpleOuterRenderer.materials.ToList();
        Material blackHeadMat = pimpleOuterRenderer.materials[materialsTemp.Count - 1];
        materialsTemp.Remove(blackHeadMat);
        pimpleOuterRenderer.materials = materialsTemp.ToArray();
    }

}
