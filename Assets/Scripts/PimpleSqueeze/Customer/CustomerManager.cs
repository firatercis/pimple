using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Cinemachine;

[System.Serializable]


public class CustomerManager : MonoBehaviour
{
    const int N_HAIR_GROUP = 1;

    //Settings
    public float layDownTime = 2.0f;
    public float standUpTime = 2.0f;
    public float turnAroundTime = 1.0f;
    public bool hasHair = false;
    // Connections
    public Animator animator;
    public ObjectPermutator pimplesPermutator;
    public MaterialRandomizer matRandomizer;
    public GirlSound girlSound;
    public ObjectPermutator hairPermutator;
    public event Action OnLayDownPlacementEnd;
    public event Action OnNextPimpleReady;
    public List<BonusPimpleGroup> bonusLevelPimpleGroups;
    private CustomerWalkingToStart customerWalkingToStart;
    // State Variables
    private int currentPimpleIndex = 0;
    public AcneManagerRev currentPimple;
    public List<AcneManagerRev> pimples = new List<AcneManagerRev>(); // TODO: State variable is public
                                                                      // Start is called before the first frame update

    private BonusPimpleGroup currentBonusPimpleGroup;

    bool dangerScreamDone = false;
    private Vector3 positionToLay;
    bool firstBackPimpleArrived = false;
    bool pimpleOptimizeMode = false; // TODO: For only test purposes.

    bool turnedAround;
    bool bonusMode;
    CinemachineVirtualCamera bonusLevelCam = null;
    private void Awake()
    {
        InitConnections();
       
    }

    void Start()
    {
        InitState();
    }
    void InitConnections(){
       
    }
    void InitState()
    {
        if(hasHair)
            InitHair();
        firstBackPimpleArrived = false;
        turnedAround = false;
    }

    private void InitHair()
    {
        List<GameObject> hairGOs = hairPermutator.SelectRandomNDifferentGroups(N_HAIR_GROUP);
        for (int i = 0; i < hairGOs.Count; i++)
        {
            hairGOs[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LayDown(Vector3.zero);
        //}

    }

    public void LoadAllPimples()
    {
        currentPimpleIndex = 0;
        if (pimplesPermutator == null) InitConnections();

        List<GameObject> pimpleGOs = pimplesPermutator.SelectAll();

        List<AcneManagerRev> pimpleList = new List<AcneManagerRev>();

        for (int i = 0; i < pimpleGOs.Count; i++)
        {
            pimpleGOs[i].SetActive(false); // Active false for optimization

            AcneManagerRev currentPimple = pimpleGOs[i].GetComponent<AcneManagerRev>();
            currentPimple.OnKilled += OnPimpleKilled;
            currentPimple.OnHealedSuccessfully += OnPimpleHealed;
            currentPimple.OnIsInDanger += OnPimpleIsInDanger;
            currentPimple.OnDangerClear += OnPimpleDangerClear;
            pimpleList.Add(currentPimple);
            
        }
        pimples.AddRange(pimpleList);
        SortPimplesDueToBack();
        currentPimple = pimples[0];
        currentPimple.gameObject.SetActive(true);
        pimpleOptimizeMode = true;
    }

    public void LoadRandomPimples(int n, float blackDotProb = 0)
    {
        currentPimpleIndex = 0;
        if (pimplesPermutator == null) InitConnections();

        List<GameObject> pimpleGOs = pimplesPermutator.SelectRandomNDifferentGroups(n);
        
        List<AcneManagerRev> pimpleList = new List<AcneManagerRev>();

        for (int i = 0; i < pimpleGOs.Count; i++)
        {
            pimpleGOs[i].SetActive(true);
            //GameObject createdPimple = Instantiate(pimpleGOs[i], pimpleGOs[i].transform.parent);
            //createdPimple.SetActive(true);
            AcneManagerRev currentPimple = pimpleGOs[i].GetComponent<AcneManagerRev>();
            currentPimple.OnKilled += OnPimpleKilled;
            currentPimple.OnHealedSuccessfully += OnPimpleHealed;
            currentPimple.OnIsInDanger += OnPimpleIsInDanger;
            currentPimple.OnDangerClear += OnPimpleDangerClear;

            // Roll dice for blackdot
            if (Random.value < blackDotProb)
                currentPimple.SetAsBlackHead();
            pimpleList.Add(currentPimple);
        }
        pimples.AddRange( pimpleList);
        SortPimplesDueToBack();
        currentPimple = pimples[0];
        currentPimple.gameObject.SetActive(true);
        bonusMode = false;
        
    }

    void SortPimplesDueToBack()
    {
        pimples = pimples.OrderBy(x => x.isRear ? 1 : 0).ToList();
    }

    public void LoadBonusLevelPimples()
    {
        
        int bonusLevelPimpleGroupIndex = Random.Range(0, bonusLevelPimpleGroups.Count);
        currentBonusPimpleGroup = bonusLevelPimpleGroups[bonusLevelPimpleGroupIndex];
        AcneManagerRev[] bonusLevelPimples = bonusLevelPimpleGroups[bonusLevelPimpleGroupIndex].pimples;
        bonusLevelCam = bonusLevelPimpleGroups[bonusLevelPimpleGroupIndex].pimplesTopCam;
        for (int i = 0; i < bonusLevelPimples.Length; i++)
        {
            bonusLevelPimples[i].gameObject.SetActive(true);
            bonusLevelPimples[i].pimpleType = AcneManagerRev.PimpleType.BonusPimple;
            pimples.Add(bonusLevelPimples[i]);

            currentPimple = pimples[0]; // TODO: Repetition
        }
        
        bonusMode = true;
    }

    public CinemachineVirtualCamera GetBonusLevelCam()
    {
        return bonusLevelCam;
    }

    public void Randomize()
    {

        matRandomizer.Randomize(); //TODO: matRandomizer gelince acilacak.
    }

    public bool NextPimple() // Returns false if all pimples ended
    {
        bool result = true;
        currentPimpleIndex++;
        if(currentPimpleIndex < pimples.Count)
        {
            currentPimple = pimples[currentPimpleIndex];

            if (pimpleOptimizeMode) currentPimple.gameObject.SetActive(true);

            RequestPimple();

        }
        else
        {
            result = false;
        }

        return result;
    }

    public void RequestPimple()
    {
        if (currentPimple.isRear && !firstBackPimpleArrived)
        {
            firstBackPimpleArrived = true;
            TurnAround();
            Invoke(nameof(OnTurnAroundCompleted), turnAroundTime);
        }
        else
        {
            OnNextPimpleReady();
        }
    }

    public void HealAllBonusPimples()
    {
        currentBonusPimpleGroup.HealAll();
       //for(int currentBonusPimpleGroup.pimples
    }

    void TurnAround()
    {
        animator.SetTrigger("TurnAround");
        turnedAround = true;
    }

    void TurnBack()
    {
        animator.SetTrigger("TurnBack");
        turnedAround = false;
    }


    public void OnTurnAroundCompleted()
    {
        OnNextPimpleReady();
    }

    public void OnPimpleHealed()
    {
        girlSound.PlayRandomGoodSound();
        dangerScreamDone = false;
    }

    public void OnPimpleKilled()
    {
       //  girlSound.PlayRandomBadSound();
        dangerScreamDone = false;
    }

    public void OnPimpleIsInDanger()
    {
        if (!dangerScreamDone)
        {
            girlSound.PlayRandomBadSound();
            dangerScreamDone = true;
            animator.SetTrigger("GetHurt");
        }
    }

    public void OnPimpleDangerClear()
    {
        dangerScreamDone = false;
    }



    public void PlaceToBed(Transform bedPoint, Vector3 positionToLay)
    {
        // Lay Down artik yataga gidip yurumeyi de iceriyor
        customerWalkingToStart = GetComponent<CustomerWalkingToStart>();
        animator.enabled = true;
        customerWalkingToStart.PlaceToBed(animator, bedPoint, positionToLay);
    }

    public void OnSitDownEnd()
    {
        Debug.Log("LayDown ended");
        OnLayDownPlacementEnd();
        //transform.DOJump(positionToLay,200,2, layDownTime).OnComplete(()=>OnLayDownPlacementEnd?.Invoke());
    }
    public void StandUp(Vector3 position)
    {
        animator.SetTrigger("StandUp");
        //transform.DOJump(position, 20, 1, standUpTime);
    }

    public void OnOperationsEnd(float successValue)
    {
        if (turnedAround)
        {
            TurnBack();
        }
        Debug.Log("Success value: " + successValue);
        // TODO: Yuz ifadeleri
        if (successValue >= 0.5) // TODO: Magic number
        {
            animator.SetTrigger("Smile");
        }
        else
        {
            animator.SetTrigger("GetAngry");
        }

    }

    public void OnSuccess()
    {
        girlSound.PlaySuccessSound();
    }

    public void BeHappy()
    {
        animator.SetTrigger("BeHappy");
    }

    public void SadWalk()
    {
        animator.SetTrigger("SadWalk");
    }

    public void ClearEvents()
    {
        OnLayDownPlacementEnd = null;
    }



}

