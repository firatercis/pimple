using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolProperties
{
    public string animationTriggerName;
    public bool oneShot;
    public string animationContinueTriggerName; // Valid only if not one shot
    public string animationPauseTriggerName;// Valid only if not one shot
    public string animationStopTriggerName;

    public float maxUseTime; // Valid only if one shot
    public GameObject toolGO;
    public GameObject toolVirtualCamGO;

    // State variables
    public float remainingTime;

    public void StartTimer()
    {
        remainingTime = maxUseTime;
    }

    public void OnUsed()
    {
        remainingTime -= Time.deltaTime;
    }

    public bool IsDead()
    {
        return remainingTime <= 0;
    }
}

public class ToolsAnimationManager : MonoBehaviour
{

    // Settings
    public float toolAnimationDelay = 2.0f;
    public float toolEndTime = 1.0f;
    // Connections

    public ToolProperties scalpel;
    public ToolProperties needle;
    public ToolProperties extractor;
    public event Action OnToolEnded;

    public Animator toolsAnimator;
    // State variables
    ToolProperties currentTool;
    bool pimpleDestroyed;
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

    public void UseScalpel() // TODO can be done with dictionary, but there may be performance issues
    {
        PlayToolApproach(scalpel);
    }

    public void UseNeedle()
    {
        PlayToolApproach(needle);
    }

    public void UseExtractor()
    {
        PlayToolApproach(extractor);
    }

    public void StartExtractor()
    {
        PlayToolUseContinuous();
    }

    public void StopExtractor()
    {
        StopToolUseContinuous();
    }


    public void PlayToolApproach(ToolProperties toolAnimation) 
    {
        toolAnimation.toolGO.SetActive(true);
        if (toolAnimation.toolVirtualCamGO != null)
            toolAnimation.toolVirtualCamGO.SetActive(true);
        //  toolsAnimator.SetTrigger(toolAnimation.animationTriggerName);
        currentTool = toolAnimation;
        Invoke(nameof(TriggerAnimationDelayed), toolAnimationDelay);
    }

    public void PlayToolUseContinuous()
    {
        if (currentTool == null) return;

        if (currentTool.oneShot) return;
        toolsAnimator.SetTrigger(currentTool.animationContinueTriggerName);
    }

    public void StopToolUseContinuous()
    {
        if (currentTool == null) return; // TODO: Daha iyi kod
        if (currentTool.oneShot) return;
        toolsAnimator.SetTrigger(currentTool.animationPauseTriggerName);
       // Invoke(nameof(OnToolEnd), toolEndTime);
    }


    void TriggerAnimationDelayed()
    {
        toolsAnimator.SetTrigger(currentTool.animationTriggerName);
        if(currentTool.oneShot)
            Invoke(nameof(OnToolEnd), toolEndTime);
    }


    void OnToolEnd()
    {
        if (pimpleDestroyed) return;
        currentTool.toolGO.SetActive(false);
        currentTool.toolVirtualCamGO.SetActive(false);
        OnToolEnded?.Invoke();
    }

    public void EndToolManual()
    {
        OnToolEnd();
    }

    public ToolProperties GetTool()
    {
        return currentTool;
    }
    
    public void OnPimpleHealed()
    {
        pimpleDestroyed = true;
    }

    public void OnPimpleKilled()
    {
        pimpleDestroyed = true;
    }


}
