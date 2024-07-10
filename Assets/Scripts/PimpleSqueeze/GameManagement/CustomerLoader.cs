using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class CustomerLoader : MonoBehaviour
{
    const int CUSTOMER_INDEX_MAN = 1;
    // Settings
    public bool testingAllPimples = false;
    public float blackDotProbabilityWhenUnlocked = 0.2f;
    // Connections
    public Transform customerWalkStartPoint;
    public Transform customerWalkEndPoint;
    
    public GameObject[] possibleCustomers;

    // State Variables


    public CustomerManager LoadCustomer(Transform girlPlacementPoint, int minNumberOfPimples, int maxNumberOfPimples, bool isBonusLevel=false, bool manUnlocked=false, bool blackHeadUnlocked = false)
    {
        int nPossibleCustomers = 1; // only girl in default

        int firstManLoadedVal = PlayerPrefs.GetInt("firstManLoadedVal", 0);

        if (manUnlocked) nPossibleCustomers = 2;

        int customerIndex = Random.Range(0, nPossibleCustomers);

        if(manUnlocked && firstManLoadedVal == 0)
        {
            firstManLoadedVal++;
            PlayerPrefs.SetInt("firstManLoadedVal", firstManLoadedVal);
            customerIndex = CUSTOMER_INDEX_MAN;
        }
       
        // Create girl
        GameObject customerGO = Instantiate(possibleCustomers[customerIndex]);
        customerGO.transform.position = girlPlacementPoint.position;

        customerGO.transform.rotation = girlPlacementPoint.rotation;
        CustomerManager girl = customerGO.GetComponent<CustomerManager>();

        int nPimples = Random.Range(minNumberOfPimples, maxNumberOfPimples);
        girl.Randomize();
        if (testingAllPimples)
        {
            girl.LoadAllPimples();
        }
        else if (!isBonusLevel)
        {
            girl.LoadRandomPimples(nPimples, blackHeadUnlocked ? blackDotProbabilityWhenUnlocked : 0);
        }
        else
            girl.LoadBonusLevelPimples();
        return girl;
    }

   

}
