using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlashFX : MonoBehaviour
{
    public const float FULL_COLOR = .25f;
    public const float TRANSPARENT_COLOR = 0.0f;

    //Settings
    public float fadeInTime;
    public float fadeOutTime;
    public float opacity = 0.25f;
    // Connections
    Image fxImage;
    // State Variables
    bool inFX;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections()
    {
        fxImage = GetComponent<Image>();
    }
    void InitState()
    {
        inFX = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ApplyFlashFX();
        }
    }
    public void ApplyFlashFX()
    {
        if (!inFX)
        {
            inFX = true;
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(fxImage.DOFade(opacity, fadeInTime));
            fadeSequence.Append(fxImage.DOFade(TRANSPARENT_COLOR, fadeOutTime)).OnComplete(() => inFX = false);
        }
    }
}