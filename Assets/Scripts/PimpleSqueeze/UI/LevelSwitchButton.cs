using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelSwitchButton : MonoBehaviour
{
    const int CENT_DOLAR_FACTOR = 100;

    //Settings

    // Connections
    public TextMeshProUGUI caption;
    public TextMeshProUGUI priceText;
    public Button clickableButton;
    public GameObject moneyIconGO;
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
        
    }

    public void SetPrice(int price)
    {
        int dollarPrice = price / CENT_DOLAR_FACTOR;
        if (dollarPrice > 0)
        {
            moneyIconGO.SetActive(true);
            priceText.text = "$" + dollarPrice; // TODO: Cent'i .00 diye yazmak gerekirse eklenir.
        }
        else
        {
            moneyIconGO.SetActive(false);
            priceText.text = "";
        }
    }

    public void SetInteractable(bool isEnabled)
    {
        clickableButton.interactable = isEnabled;
    }

    public void SetMaxed(bool isMaxed)
    {
        gameObject.SetActive(!isMaxed); 
    }


}

