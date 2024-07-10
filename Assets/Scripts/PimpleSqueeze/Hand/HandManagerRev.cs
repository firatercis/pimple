using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class HandManagerRev : MonoBehaviour, UserInputButtonListener
{
    const int N_TWEEN_REQUIRED_TO_PLACE = 2;

    const int ONBOARDING_NONE = -1;
    const int ONBOARDING_HOLD_SQUEEZE = 0;
    const int ONBOARDING_RELEASE_HEAL = 1;
    const int ONBOARDING_BUY_UPGRADES = 2;

    const int TOOL_ID_SCALPEL = 0;
    const int TOOL_ID_EXTRACTOR = 1;
    const int TOOL_ID_NEEDLE = 2;

    //Settings
    public float pimpleSqueezeDelay = 1.0f;
    public bool tapMode;
    public float constantSqueezePowerInTapMode = 1.0f;
    public float releaseTutorialTime = 1.0f;
    public float upgradeTutorialDelay = 2.0f;
    public float extractorCoeff = 2.0f;
    // Connections
    private HandAnimationManager animationManager;
    private HandMaterialFX handMaterialFX;
    public Transform leftHand;
    public Transform rightHand;
    public Transform[] ropeJumpPoints;
    public OnboardingList onboardingList;
    public event Action OnRopeLanded;
    ToolsAnimationManager toolsAnimationManager;
    public CinemachineVirtualCamera toolUsageVirtualCam;
    public Tool[] tools;

    // State Variables 
    AcneManagerRev currentPimple;
    bool canSqueeze;
    bool waitingRopePop;
    bool waitingSqueeze;
    int ropeJumpPointIndex;
    float squeezePower;
    bool showingTutorial = false;
    bool firstTutorialShown = false;
    bool firstDanger = true;
    bool bonusMode = false;
    bool isSqueezing = false;
    bool usingTool = false;
    ToolProperties currentToolProperties;
    Tool currentTool;
    // Start is called before the first frame update

    private void Awake()
    {
        InitConnections();
    }


    void Start()
    {
        
        InitState();
      
    }
    void InitConnections(){
        animationManager = GetComponent<HandAnimationManager>();
        handMaterialFX = GetComponent<HandMaterialFX>();

    }
    void InitState(){
        animationManager.SetVisible(false);
        firstTutorialShown = PlayerPrefs.GetInt("firstTutorialShown", 0) == 1;
        // Get the information about first tutorial shown
        // PlayerPrefs.GetInt("tutorialShownLast",Time.t)
        currentToolProperties = null;
        // ExitPimple();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToolTestCommands();

    }

    private void CheckToolTestCommands()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseTool(TOOL_ID_EXTRACTOR);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UseTool(TOOL_ID_SCALPEL);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            UseTool(TOOL_ID_NEEDLE);
        }

        if (currentTool != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentTool.OnContinuousUse();
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                currentTool.OnPauseUse();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentTool.OnStopUse();
                currentTool = null;
            }

        }

       
    }

    public void UseTool(int toolID)
    {
        currentTool = tools[toolID];
        currentTool.gameObject.SetActive(true);
        animationManager.SetVisible(false);
        NestToolOnPimple();
        if (currentTool.oneShot)
        {
            Invoke(nameof(StopTool), currentTool.oneShotTime);
        }
        currentTool.OnUse(currentPimple.transform);
        toolUsageVirtualCam.gameObject.SetActive(true);
    }

    public void PlayTool()
    {
        currentTool.OnContinuousUse();
    }

    public void PauseTool()
    {
        currentTool.OnPauseUse();
    }

    public void StopTool()
    {
        currentTool.OnStopUse();
        animationManager.SetVisible(true) ;
        animationManager.GoPosition();
        currentTool = null;
        toolUsageVirtualCam.gameObject.SetActive(false);
    }

    private void NestToolOnPimple()
    {
        currentTool.transform.position = currentPimple.toolsNest.transform.position;
        currentTool.transform.rotation = currentPimple.toolsNest.transform.rotation;
        currentTool.transform.localScale = currentPimple.toolsNest.transform.lossyScale;
    }

    public void GoToPimple(PimpleWayPoints pimpleWayPoints,float approachTime = 0)
    {
        currentToolProperties = null;
        currentPimple = pimpleWayPoints.GetComponent<AcneManagerRev>();
        if (currentPimple.pimpleType != AcneManagerRev.PimpleType.BonusPimple)
        {
            bonusMode = false; // If the next pimple is not bonus, switch off the bonus mode.
        }

        if (!bonusMode) // If bonus, do not move to pimple
        {
            PlaceOver(leftHand, pimpleWayPoints.leftPoint,approachTime);
            PlaceOver(rightHand, pimpleWayPoints.rightPoint, approachTime);
        }

        currentPimple.ropeJumpPoint = ropeJumpPoints[ropeJumpPointIndex];
        ropeJumpPointIndex = (ropeJumpPointIndex + 1) % ropeJumpPoints.Length;
        currentPimple.OnRopePopped += OnRopePopped;
        currentPimple.OnHealedSuccessfully += OnPimpleFinished;
        currentPimple.OnKilled += OnPimpleFinished;
        
        toolsAnimationManager = currentPimple.GetComponent<ToolsAnimationManager>();
        currentPimple.OnHealedSuccessfully += toolsAnimationManager.OnPimpleHealed;
    }

    public void GoToPimpleGroup(BonusPimpleGroup bonusPimpleGroup)
    {

    }

    public void SetSqueezePower(float squeezePower)
    {
        this.squeezePower = squeezePower;
    }

    public void ExitPimple()
    {
        currentPimple.ClearEvents();
        currentPimple = null;
        animationManager.SetVisible(false);
        waitingRopePop = false;
        if(toolsAnimationManager != null)
            toolsAnimationManager.gameObject.SetActive(false);
    }


    private void PlaceOver(Transform subject, Transform target,float approachTime = 0, Sequence sequenceToAppend = null)
    {
        Tween moveTween = subject.DOMove(target.position, approachTime);
        Tween rotateTween = subject.DORotateQuaternion(target.rotation, approachTime);
        if(sequenceToAppend != null)
        {
            sequenceToAppend.Join(moveTween);
            sequenceToAppend.Join(rotateTween);
        }
    }

    public void SetSqueezingEnabled(bool squeezingEnabled)
    {

        if (!firstTutorialShown)
        {
            onboardingList.SetTutorialActive(ONBOARDING_HOLD_SQUEEZE, currentPimple.transform);
            firstTutorialShown = true;
            showingTutorial = true;
            PlayerPrefs.SetInt("firstTutorialShown", 1);
        }

        canSqueeze = squeezingEnabled;
        animationManager.SetVisible(true);
        animationManager.GoPosition();
    }

   void OnPimpleFinished()
    {
        if(currentTool != null)
        {
            StopTool();
        }
        
    }

    void OnRopePopped(Rigidbody particleRigidBody)
    {
        ropeJumpPoints[ropeJumpPointIndex].gameObject.SetActive(true);
        animationManager.StopSqueeze();
        // animationManager.SetVisible(false);
        OnRopeLanded?.Invoke();
    }

   

    public void OnButtonPressed()
    {
        if (currentPimple != null && canSqueeze)
        {
            waitingSqueeze = true;
            isSqueezing = true;

            animationManager.PlaySqueeze();
            currentPimple.OnStartSqueezeInput();
            
            if(currentTool != null)
            {
                if(!currentTool.oneShot)
                    PlayTool();                
            } 
            
           // soundFXManager.StartSqueeze();

            // If showing tutorial, make it vanish
            if (showingTutorial)
            {
                HideTutorial();
            }
        }
    }

    void StartToolAnimation()
    {

    }

    public void OnHandApproached()
    {
        if (waitingSqueeze)
        {
            currentPimple.OnStartSqueezeInput();
            waitingSqueeze = false;
        }
    }
    public void OnButtonHeld()
    {
        if (!isSqueezing) return;

        if (currentPimple != null && canSqueeze && !showingTutorial)
        {
            float appliedPower = squeezePower;
            if(currentTool != null)
            {
                appliedPower *= currentTool.powerMultiplier;
            }
            if(appliedPower > 0)
                currentPimple.OnSqueezeInput(appliedPower);
        }
    }


    public void OnDanger()
    {
        if (firstDanger)
        {
            firstDanger = false;
            if (!firstTutorialShown)
            {

                onboardingList.SetTutorialActive(ONBOARDING_RELEASE_HEAL, currentPimple.transform);
                showingTutorial = true;
            }
        }
       
    }
    

    public void OnButtonReleased()
    {
        if (currentPimple != null && canSqueeze)
        {
            isSqueezing = false;
            currentPimple.StopSqueezing();
            if (!waitingRopePop)
            {
                if (currentTool == null)
                    animationManager.StopSqueeze();
                else
                {
                    PauseTool();
                    //currentTool = null;
                }
            }
        }

        if (showingTutorial && !firstTutorialShown)
        {
            Invoke(nameof(HideTutorial), releaseTutorialTime);
            Invoke(nameof(ShowUpgradeTutorial), upgradeTutorialDelay);
        }
        //soundFXManager.StopSqueeze();
    }

    void OnToolEnded()
    {
        currentTool.OnStopUse();
        animationManager.SetVisible(true);
        animationManager.GoPosition();
        currentTool = null;
    }

    public void DisplayUpgradeFX()
    {
        //handMaterialFX.OutlineFX();
    }

    void ShowTutorial(int tutorialIndex)
    {
        onboardingList.SetTutorialActive(tutorialIndex);
        showingTutorial = true;
    }

    void ShowUpgradeTutorial()
    {
        ShowTutorial(ONBOARDING_BUY_UPGRADES);
    }

    void HideTutorial()
    {
        onboardingList.HideTutorial();
        showingTutorial = false;
    }

    public void SetBonusMode(bool mode)
    {
        bonusMode = mode;
    }

    public void ResetTutorials()
    {
        PlayerPrefs.DeleteKey("firstTutorialShown");
    }

}

