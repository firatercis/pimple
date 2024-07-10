using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using DG.Tweening;
using Obi;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.XR;
using static TheraBytes.BetterUi.LocationAnimations;
using Random = UnityEngine.Random;

public class AcneManagerRev : MonoBehaviour
{
    public enum PimpleType
	{
		BlackHead,
		RegularPimple,
        BonusPimple
	}
	
	public enum BladeType
	{
		Needle,
		Scalpel,
		None
	}

    //  public CinemachineVirtualCamera pimpleCamera;



    // Settings
    public PimpleType pimpleType;
    public BladeType bladeType;
    public float maxAcneLength;
    public float maxPainPeak;
    public bool tapticFeedback = true;
    public float pimpleOpenTime = 1.0f;
    public float painIncreaseRate = 2.0f;
    public float dangerZonePercent = 0.75f;
    public float healedPimpleDestroyTime = 2.0f;
    public float ropeLifeTime = 4.0f;
    public float blackHeadExtraPercent = 0.25f;
    public float bonusReviveTimeMin = 2.0f;
    public float bonusReviveTimeMax = 3.0f;
    public float healingParticleFXDelay = 1.0f;
    public float hideTime;
    private float healingRate;
    public bool isRear;

    [Header("Tap Squeeze Mod")]
    public float tapSqueezeSimTime = 2.0f;
    public bool tapMode;

    // Connections
    
    public GameObject ropePrototype;
    [SerializeField] private ObiRopeCursor cursor;
    [SerializeField] private ObiRope rope;
    [SerializeField] PimpleAlphaManager alphaManager;
    public PimpleAnimation pimpleAnimation;
    
    public GameObject[] bandGOs;
    public GameObject[] dissappearedObjectsInExplosion;
    public Rigidbody ropeParticleAttachment;
    PimpleParticleFX pimpleFX;
    PimpleHaptic pimpleHaptic;
    PimpleBlackHead pimpleBlackHead;
    public GameObject blackDotGO;
    public GameObject scalpelGO;
    public GameObject needleGO;
    public TextureLengthAdaptor textureLengthAdaptor;
    public PimpleSound soundManager;
    public PimpleBonusClick pimpleBonusClick;
    public GameObject virtualCamGO;
    public PimpleToolResponse toolResponse;
    public Transform toolsNest;
    public Transform ropeJumpPoint;
    public ParticleSystem popBonusFX;
    public ParticleSystem healBonusFX;
    public Renderer pimpleOuterRenderer;
    // Events
    public event Action<float> OnPain;
    public event Action<float,float> OnRopeLength;
    public event Action OnKilled;
    public event Action OnIsInDanger;
    public Action OnDangerClear;
    public event Action OnHealedSuccessfully;
    public event Action OnBlackheadHealed;
    public event Action OnBonusHeal;
    public event Action OnReleased;
    public Action<Rigidbody> OnRopePopped;
    // State variables
    private float initialSqueezeValue;
    private float lastSqueezeValue;
    private float initialLength;
    public float currentLength;
    public float currentPain;

    private bool isStarted = false;
    public bool isSqueezable = false;
    public bool isOpening = false;
    private bool isSqueezedThisMoment = false;
    private bool isInDanger = false;
    private bool isFinished = false;
    public float blackHeadHp = 1.0f;
    private float blackHeadRemainingHP = 0;
    
    private float testOutput = 0;
    private void Awake()
    {
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    private void InitConnections()
    {
        pimpleFX = GetComponent<PimpleParticleFX>();
        pimpleHaptic = GetComponent<PimpleHaptic>();
        pimpleBlackHead = GetComponent<PimpleBlackHead>();
        soundManager = GetComponent<PimpleSound>();
        toolResponse.OnScalpel += OnScalpel;
        toolResponse.OnNeedle += OnNeedle;
        toolResponse.OnExtractor += OnExtractor;
    }

   

    void InitState()
    {
        initialLength = rope.restLength;
        currentLength = 0;
        currentPain = 0;
        if(pimpleType == PimpleType.BlackHead)
        {
            blackHeadHp = maxAcneLength * blackHeadExtraPercent;
            blackHeadRemainingHP = blackHeadHp;
            blackDotGO.SetActive(true);
        }else if(pimpleType == PimpleType.BonusPimple)
        {
            pimpleBonusClick.gameObject.SetActive(true);
            pimpleBonusClick.OnColliderClicked += OnBonusColliderClicked;
        }

        //if(bladeType == BladeType.Needle)
        //{
        //    needleGO.SetActive(true);
        //}

        //if(bladeType == BladeType.Scalpel)
        //{
        //    scalpelGO.SetActive(true);
        //}
        Taptic.tapticOn = tapticFeedback;
        if(pimpleType != PimpleType.BonusPimple) // TODO: Karisik kod
            RefreshRope();
    }

    public void SetAsBlackHead()
    {
        pimpleType = PimpleType.BlackHead;
        blackDotGO.SetActive(true);
        blackHeadRemainingHP = blackHeadHp;
        pimpleBlackHead.SetBlackHeadInitial();
       // rope.gameObject.SetActive(false);
    }


    void CheckPimplePainAndLength()
    {
        CheckPain();

        CheckPimpleHealed();

        // Check is in danger
        CheckIsInDanger();
        CoolDown();
        alphaManager.SetPimpleAlpha(currentPain / maxPainPeak);
    }
    void SetAcneTexture()
    {
        if(!isFinished && isSqueezable)
            textureLengthAdaptor.SetLengthParameters(currentLength, maxAcneLength);
    }

    public void SetVirtualCamActive()
    {
        virtualCamGO.SetActive(true);
    }

    public void SetVirtualCamInactive()
    {
        virtualCamGO.SetActive(false);
    }

    private void CheckPain()
    {
        if (currentPain >= maxPainPeak) // Sivilceyi patlatti
        {
            isFinished = true;
            isSqueezable = false;
            KilledPimpleFX();
            OnKilled?.Invoke();

            pimpleHaptic.OnKilled();
            currentPain = 0;
            // gameObject.SetActive(false);
            isSqueezedThisMoment = false;
            soundManager.PlaySplashSound();
            
        }

        if (isSqueezedThisMoment)
        {
            pimpleHaptic.OnSqueeze();
        }
    }

    private void CheckPimpleHealed()
    {
        if (currentLength >= maxAcneLength && isSqueezable) // Sivilceyi bitirdi
        {
            Heal();

        }
    }

    public void Heal(bool ropeExists = true, bool hide = false)
    {
        isStarted = false;
        pimpleAnimation.SweepAnim();

        if(ropeExists)
            PopTheRope();

        if (pimpleType == PimpleType.RegularPimple)
        {
            OnHealedSuccessfully?.Invoke();
        }else if(pimpleType == PimpleType.BlackHead)
        {
            OnHealedSuccessfully?.Invoke(); // TODO: daha iyi bir yerde karar
        }else
        {
            //OnHealedSuccessfully?.Invoke(1.0f);
            OnBonusHeal?.Invoke();
        }

        isSqueezable = false;
        isFinished = true;
        isSqueezedThisMoment = false;

        
      // Invoke(nameof(DestroyRope), ropeLifeTime);
        

        soundManager.StartHealSound();
        pimpleHaptic.OnHealed();

        currentLength = 0;
        Invoke(nameof(PlayBonusHealingFX), healingParticleFXDelay);
        if (hide)
            Invoke(nameof(HideMesh), hideTime);
    }

    void HideMesh()
    {
        pimpleOuterRenderer.enabled = false;
    }



    void PopTheRope()
    {
        Vector3 popPosition = transform.up * 10.0f + transform.position;

        if(ropeJumpPoint != null)
        {
            popPosition = ropeJumpPoint.position;
        }
        // ropeParticleAttachment.transform.DOJump(popPosition, 1.0f, 1, 1.0f, snapping: true);
        DestroyRope();
        Invoke(nameof(NotifyRopePopped), 1.0f);
    }

    void NotifyRopePopped()
    {
        OnRopePopped?.Invoke(ropeParticleAttachment);
        
    }
    

    private void Revive()
    {
        healBonusFX.Stop();
        pimpleOuterRenderer.enabled = true;
        pimpleAnimation.ReviveAnim();
        //RefreshRope();
        currentPain = 0;
        pimpleBonusClick.SetColliderEnabled(true);
    }

    private void CheckIsInDanger()
    {
        if (currentPain >= maxPainPeak * dangerZonePercent)
        {
            isInDanger = true;
            OnIsInDanger?.Invoke();
            pimpleHaptic.OnSqueeze(inDanger: true);
        }
        else
        {
            if (isInDanger)
            {
                OnDangerClear?.Invoke();
                isInDanger = false;
            }
        }
    }

    public void OnStartSqueezeInput()
    {
        if(blackHeadRemainingHP > 0)
        {
            pimpleAnimation.PlayResistAnim();
        }
        else
        {
            pimpleAnimation.PlaySqueezeAnim();
        }
        
        initialSqueezeValue = currentPain;

        pimpleFX.SetHealingParticle(false);
        if(pimpleType != PimpleType.BonusPimple)
            PlayOpenFX();
    }

    public void OnSqueezeTapInput(float squeezePower)
    {
        if (!isSqueezable) OnStartSqueezeInput();
        if (!isOpening && isSqueezable)
        {
            ApplySqueeze(squeezePower, painIncreaseRate, tapSqueezeSimTime);
        } // TODO: Similarity to ContinueSqueezing function
    }

    // Tools
    private void OnScalpel()
    {
        if(pimpleType == PimpleType.BlackHead)
        {
            blackHeadRemainingHP = 0;
            OnBlackHeadDestroyed();
        }

        StrechTheRope((maxAcneLength - currentLength) / 2);

        Debug.Log("I am scalpelled");
        pimpleAnimation.PlayScalpel();
    }
    
    private void OnExtractor()
    {
        Debug.Log("I collided with extractor");
    }
    private void OnNeedle()
    {
        Debug.Log("I am needled");
        pimpleAnimation.PlayNeedle();
        StrechTheRope(maxAcneLength);
        Heal();


    }
    public void StopSqueezing()
    {
        pimpleAnimation.StopSqueezeAnim();
        lastSqueezeValue = currentPain;
        pimpleFX.SetSqueezingParticle(false);
        isSqueezedThisMoment = false;
        soundManager.StartHealSound();
        pimpleFX.SetHealingParticle(true);
    }

    public void OnSqueezeInput(float squeezePower)
    {
        if (!isOpening && isSqueezable && pimpleType != PimpleType.BonusPimple)
        {
            ApplySqueeze(squeezePower, painIncreaseRate,Time.deltaTime);
        }
    }

    public void ApplySqueeze(float power, float painRate, float simTime)
    {
       
        currentPain += painRate * simTime;
        float painPercentage = currentPain / maxPainPeak;
        OnPain?.Invoke(painPercentage);
        alphaManager.SetPimpleAlpha(painPercentage);
        if(blackHeadRemainingHP <= 0)
        {

            StrechTheRope(power * simTime); // Rope'u uzatma kismi
        }
        else
        {
            DamageBlackHead(power * simTime);
        }
        if (!isSqueezedThisMoment)
        {
            isSqueezedThisMoment = true;
            soundManager.PlaySqueezeSound(0); //TODO: will be associated with pain threshold
        }
        soundManager.SetDegree(currentLength / maxAcneLength);
        
    }

    

    void DamageBlackHead(float damage)
    {
        blackHeadRemainingHP -= damage;
        if(blackHeadRemainingHP <= 0)
        {
            OnBlackHeadDestroyed();
        }

    }

    void OnBonusColliderClicked()
    {
        popBonusFX.Play();
      //  StrechTheRope(maxAcneLength);
     //   PopTheRope();
        Heal(ropeExists:false);
        pimpleBonusClick.SetColliderEnabled(false);
        float reviveTime = Random.Range(bonusReviveTimeMin, bonusReviveTimeMax);
        
        Invoke(nameof(Revive), reviveTime);
    }

    void PlayBonusHealingFX()
    {
        healBonusFX.Play();
    }
   
    void OnBlackHeadDestroyed()
    {
        pimpleAnimation.PlayBlackHeadAnim();
        pimpleBlackHead.DestroyBlackHead();
        pimpleAnimation.PlayBlackDotFinished();
        OnBlackheadHealed?.Invoke();
        //rope.gameObject.SetActive(true);
    }

    

    void Update()
    {
        CheckPimplePainAndLength();
        SetAcneTexture();


    
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Heal();
            ClearEvents();
            Invoke(nameof(SetActiveFalse), 2);
        }

    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }    

    private void CoolDown()
    {
        float coolDownValue = tapMode ? healingRate * tapSqueezeSimTime : healingRate * Time.deltaTime;

        if (currentPain > 0 && !isSqueezedThisMoment)
        {
            currentPain -= healingRate * Time.deltaTime;
        }

        if(currentPain <= 0)
        {
            currentPain = 0;
            pimpleFX.SetHealingParticle(false);
            soundManager.StopSound();
        }
    }

    private void StrechTheRope(float value)
    {
        cursor.ChangeLength(rope.restLength + value);
        currentLength = rope.restLength - initialLength;
        OnRopeLength?.Invoke(currentLength, maxAcneLength);
    }

    

    public void PlayOpenFX()
    {
        //light.SetActive(true);
        //handManager.OpenAcnes();
  
        isOpening = true;
        isSqueezable = true; // TODO: for test => Burasi düzenlenecek.
        pimpleFX.OpeningFX();
        soundManager.PlayOpenSound();
        DOVirtual.DelayedCall(pimpleOpenTime, () =>
        {
            //handManager.gameObject.SetActive(true);
            //if (pimpleType == PimpleType.BlackHead)
            //{
            //    animatorManager.PlayBlackHeadAnim();
            //}

            //else
            //{
            //    pimpleAnimation.PlayResistAnim();
            //}
            isOpening = false;
            isSqueezable = true;
            pimpleFX.SetSqueezingParticle(true);
           
        });
    }

    public void KilledPimpleFX()
    {
        for(int i = 0; i < bandGOs.Length; i++)
        {
            bandGOs[i].SetActive(true);
        }

        for(int i = 0; i < dissappearedObjectsInExplosion.Length; i++)
        {
            dissappearedObjectsInExplosion[i].SetActive(false);
        }

    }
    
    void DestroyRope() // TODO: Kullanilmasi gerek
    {
        Destroy(rope.gameObject);
    }

    void RefreshRope()
    {
        GameObject newRopeGO = Instantiate(ropePrototype, ropePrototype.transform.parent);
        newRopeGO.SetActive(true);
        textureLengthAdaptor = newRopeGO.GetComponent<TextureLengthAdaptor>();
        rope = newRopeGO.GetComponent<ObiRope>();
        cursor = newRopeGO.GetComponent<ObiRopeCursor>();
    }

    public void ClearEvents()
    {
        OnPain = null;
        OnRopeLength = null;
        OnKilled = null;
        OnIsInDanger = null;
        OnHealedSuccessfully = null;
        OnReleased = null;
        OnRopePopped = null;

    }
    public void SetHealingRate(float healingRate)
    {
        this.healingRate = healingRate;
    }

    public void SetMaxAcneLength(float maxAcneLength)
    {
        this.maxAcneLength = maxAcneLength;
    }


}
