using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimatedMoneyIndicator : MonoBehaviour
{
    public const int CENT_DOLLAR_COEFF = 100;

    // Settings
    public float iconSourcePosDeviation = 10;
    public int numberOfIconsSpawned = 10;
    public float initialSpreadDuration = 1.0f;
    public float finalRushDuration = 1.0f;
    public float additionalDurationToFinish = 1.0f;
    public float punchFactor = 0.5f;
    public float punchDuration = 1.0f;
    // Connections
    public TextMeshProUGUI moneyIndicatorText;
    public Transform moneyIconSpawnDestinationPoint;
    public Transform moneyIconSpawnCenterPoint;
    public GameObject moneyIconPrefab;

    // State variables
    int moneyToSet;
    Action OnAnimationEnd;
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

    public void SetMoney(int money, bool applyRushingAnimation = false)
    {
        if (applyRushingAnimation)
        {
            PlayAnimation();
            Invoke(nameof(SetMoneySaved), initialSpreadDuration + finalRushDuration + additionalDurationToFinish);
        }
        else
        {
            moneyIndicatorText.text = (money / CENT_DOLLAR_COEFF).ToString();
        }

    }

    private void SetMoneySaved()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayAnimation();
        }   
    }

    public void PlayAnimation(Action animationEndEvent = null )
    {
        PlayIconsRushAnimation(moneyIconSpawnCenterPoint.position, moneyIconSpawnDestinationPoint.position, iconSourcePosDeviation);
        OnAnimationEnd = animationEndEvent;
        Invoke(nameof(NotifyAnimationEnd), initialSpreadDuration + finalRushDuration + additionalDurationToFinish);
    }

    void NotifyAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
        OnAnimationEnd = null;
    }

    private void PlayIconsRushAnimation(Vector3 spawnPos,Vector3 destionationPos, float deviation)
    {
        for (int i = 0; i < numberOfIconsSpawned; i++)
        {
            //PlaySingleIconRush(screenPos);
            PlaySingleIconRushAnimation(spawnPos, destionationPos, deviation);
        }
    }

    private void PlaySingleIconRushAnimation(Vector3 spreadCenter, Vector3 finalDestination, float deviation)
    {
        Vector3 spreadPos = spreadCenter + (Vector3)Random.insideUnitCircle * deviation;
        GameObject createdGO = Instantiate(moneyIconPrefab, spreadCenter, Quaternion.identity, transform);
        RectTransform createdIconRect = createdGO.GetComponent<RectTransform>();
        createdIconRect.DOPunchScale(Vector3.one * punchFactor, punchDuration, vibrato:0);
        createdIconRect.DOMove(spreadPos, initialSpreadDuration).OnComplete(() =>
        {
            createdIconRect.DOMove(finalDestination, finalRushDuration).OnComplete(() =>
            {
                Destroy(createdGO);

            });
        });
    }

}
