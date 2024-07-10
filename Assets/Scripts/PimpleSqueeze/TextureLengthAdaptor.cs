using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLengthAdaptor : MonoBehaviour
{
    const string MAIN_TEX_SHADER_PROPERTY_NAME = "_MainTex";

    // Settings
    public float gradientStartRatio = 0.5f;
    // Connections
    Renderer matRenderer;
    // State variables
    Vector2 initialTextureScale;
    Texture myTexture;
    void Awake(){
    	InitConnections();
    }
    void Start()
    {
	    InitState();
    }

    void InitConnections(){
        matRenderer = GetComponent<Renderer>();
    }
    void InitState(){
        initialTextureScale = matRenderer.material.GetTextureScale(MAIN_TEX_SHADER_PROPERTY_NAME);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLengthParameters(float currentLength, float maxLength)
    {
        float gradientStartLength = gradientStartRatio * maxLength;

        matRenderer.material.SetFloat("_CurrentAcneLength", currentLength); //TODO: Hata
        matRenderer.material.SetFloat("_GradientStartLength", gradientStartLength);
        matRenderer.material.SetFloat("_MaxAcneLength", maxLength);
    }
}
