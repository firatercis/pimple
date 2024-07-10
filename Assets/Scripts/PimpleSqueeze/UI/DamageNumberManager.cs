using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class DamageNumberManager : MonoBehaviour
{
    //Settings
    //public Color positiveMessageColor = Color.green;
    //public Color negativeMessageColor = Color.red;

    public string[] possiblePositiveMessages;
    public string[] possibleNegativeMessages;

    // Connections
    public DamageNumber damageNumberMoneyPrefab;
    public DamageNumber negativeMessagePrefab;
    public DamageNumber positiveMessagePrefab;
    public DamageNumber damageNumberBigMoneyPrefab;
    // State Variables 

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

    public void SpawnMoneyNumber(float money, bool isBig = false)
    {
        DamageNumber damageNumber;
        if (!isBig)
        {
             damageNumber = damageNumberMoneyPrefab.Spawn(transform.position,money);
        }
        else
        {
            damageNumber = damageNumberBigMoneyPrefab.Spawn(transform.position, money);
        }
        damageNumber.SetAnchoredPosition(transform, damageNumberMoneyPrefab.transform.position);
    }
    public void SpawnMessage(bool isPositive = true, string message = null)
    {
        if (message == null)
        {
            string[] possibleMessages = isPositive ? possiblePositiveMessages : possibleNegativeMessages;
            int messageIndex = Random.Range(0,possibleMessages.Length);
            message = possibleMessages[messageIndex];
        }

        DamageNumber messagePrefab = isPositive ? positiveMessagePrefab : negativeMessagePrefab;

        DamageNumber damageNumber = messagePrefab.Spawn(transform.position);
        damageNumber.SetAnchoredPosition(transform, negativeMessagePrefab.transform.position);
       // damageNumber.SetColor(isPositive ? positiveMessageColor : negativeMessageColor);
        damageNumber.leftText = message;
    }
}

