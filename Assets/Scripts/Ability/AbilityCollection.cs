using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// This script allows us to create a List of AbilityBase as a ScriptableObject
namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "AbilityBaseCollection.asset",
        menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "AbilityBase")]
    public class AbilityCollection : Collection<AbilityBase>
    {
    
    }
}


