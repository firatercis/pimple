using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BonusInGameCanvas : MonoBehaviour
{

    // Settings
    public float onboardingTime = 4.0f;
    // Connections
    public TextMeshProUGUI customerNumberText;
    public TextMeshProUGUI nPimplesText;
    public Image timerPercentFiller;
    public GameObject onboardingGO;
    // State variables
    Vector3 nPimplesTextInitialScale;
    bool punchingScale = false;
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
        nPimplesTextInitialScale = nPimplesText.transform.localScale; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCustomerNumber(int customerNumber)
    {
        customerNumberText.text = "Patient " + customerNumber.ToString();
    }

    public void SetBonusPimplesHealed(int bonusPimpleHealed)
    {
        nPimplesText.text = bonusPimpleHealed.ToString();
        if (!punchingScale)
        {
            punchingScale = true;
            nPimplesText.transform.DOPunchScale(nPimplesTextInitialScale * 1.0f, 0.5f, vibrato: 0).OnComplete(() => punchingScale = false);
        }

    }

    public void SetTimerPercent(float percent)
    {
        timerPercentFiller.fillAmount = percent;
    }

    private void OnEnable()
    {
        onboardingGO.SetActive(true);
        Invoke(nameof(HideOnboarding), onboardingTime);
    }

    void HideOnboarding()
    {
        onboardingGO.SetActive(false);
    }

}
