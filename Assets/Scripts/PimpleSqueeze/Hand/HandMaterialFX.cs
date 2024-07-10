using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandMaterialFX : MonoBehaviour
{

    const string OUTLINE_PROP_NAME = "_Outline";

    //Settings
    public float outLineWidthPeakValue = 0.1f;
    public float outlinePunchDuration = 0.3f;
    // Connections
    public Renderer[] targetRenderers;
    public int targetMaterialIndex;
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


        if (Input.GetKeyDown(KeyCode.O))
        {
            OutlineFX();
        }
    }

    public void OutlineFX()
    {

        DOTween.To(
            GetOutlineWidth,
            SetOutlineWidth,
            outLineWidthPeakValue,
            outlinePunchDuration).OnComplete(StartRestoreTween);
                
    }


    void StartRestoreTween()
    {
        DOTween.To(
            GetOutlineWidth,
            SetOutlineWidth,
            0,
            outlinePunchDuration);
    }

    public void SetOutlineWidth(float outlineWidth)
    {
        for(int i=0; i< targetRenderers.Length; i++)
        {
            targetRenderers[i].materials[targetMaterialIndex].SetFloat(OUTLINE_PROP_NAME, outlineWidth);
        }
    }

    public float GetOutlineWidth()
    {
       float outlineWidth = targetRenderers[0].materials[targetMaterialIndex].GetFloat(OUTLINE_PROP_NAME);
        return outlineWidth;
    }

}

