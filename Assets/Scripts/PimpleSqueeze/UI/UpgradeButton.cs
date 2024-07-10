using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeButton : MonoBehaviour
{
    const int CENT_DOLAR_FACTOR = 100;

    //Settings

    // Connections
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;
    public GameObject isMaxedImage;
    public Button clickableButton;
    public GameObject disabledImage;
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Disabling button:" + gameObject.name);

            clickableButton.interactable = false;
        }
    }

    public void SetLevel(int level)
    {
        levelText.text = "LEVEL " + level;
    }
    public void SetPrice(int price)
    {
        int dollarPrice = price / CENT_DOLAR_FACTOR;
        if(dollarPrice > 0)
            priceText.text = "$" + dollarPrice; // TODO: Cent'i .00 diye yazmak gerekirse eklenir.
    }

    public void SetInteractable(bool isEnabled)
    {
        clickableButton.interactable = isEnabled;
        disabledImage.SetActive(!isEnabled);
        //clickableButton.act
    }

    public void SetMaxed(bool isMaxed)
    {
     
        isMaxedImage.SetActive(isMaxed);
    }

    
}

