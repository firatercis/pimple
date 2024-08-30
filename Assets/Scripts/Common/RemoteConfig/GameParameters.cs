using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SoftwareKingdom.Common
{
    [CreateAssetMenu(fileName = "GameParametersData", menuName = "SoftwareKingdom/GameParametersData", order = 1)]
    public class GameParameters : ScriptableObject
    {
        public string[] keys;
        public string[] values; // default values
    }
}

   
