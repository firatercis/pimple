using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PimpleAlphaManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve redAlphaCurve;
    [SerializeField] private Material hybridMat;

    private Color colour;

    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        hybridMat = myRenderer.materials[1];
        colour = hybridMat.color;

        hybridMat.color = colour;
    }

    public void SetPimpleAlpha(float alpha)
    {
        if (alpha != null && alpha >= 0)
        {
            colour = hybridMat.color;
            var curvedColour = redAlphaCurve.Evaluate(alpha );
            colour.a = curvedColour;
            hybridMat.color = colour;
        }
    }
}
